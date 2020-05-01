using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CsvHelper;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;
using SurveyTool.Models;

namespace SurveyTool.Controllers
{
    [Authorize]
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SurveysController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var surveys = _db.Surveys.ToList();
            return View(surveys);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var survey = new Survey
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };

            return View(survey);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Survey survey, string action)
        {
            if (ModelState.IsValid)
            {
                survey.Questions.ForEach(q => q.CreatedOn = q.ModifiedOn = DateTime.Now);
                _db.Surveys.Add(survey);
                _db.SaveChanges();
                TempData["success"] = "Ankieta została utworzona!";
                return RedirectToAction("Edit", new { id = survey.Id });
            }
            else
            {
                TempData["error"] = "Wystąpił błąd podczas próby utworzenia ankiety.";
                return View(survey);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var survey = _db.Surveys.Include("Questions").Single(x => x.Id == id);
            survey.Questions = survey.Questions.OrderBy(q => q.Priority).ToList();
            return View(survey);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Survey model)
        {
            foreach (var question in model.Questions)
            {
                question.SurveyId = model.Id;

                if (question.Id == 0)
                {
                    question.CreatedOn = DateTime.Now;
                    question.ModifiedOn = DateTime.Now;
                    _db.Entry(question).State = EntityState.Added;
                }
                else
                {
                    question.ModifiedOn = DateTime.Now;
                    _db.Entry(question).State = EntityState.Modified;
                    _db.Entry(question).Property(x => x.CreatedOn).IsModified = false;
                }
            }

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        public ActionResult Delete(Survey survey)
        {
            _db.Entry(survey).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GeneratePDF(int id)
        {
            var survey = _db.Surveys.Include("Questions").Single(x => x.Id == id);
            survey.Questions = survey.Questions.OrderBy(q => q.Priority).ToList();
            //PDF generate
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"PDF\\{id}.pdf";
            string path = System.IO.Path.Combine(basePath, filePath);
            string fontPath = System.IO.Path.Combine(basePath, "fonts\\TIMES.TTF");
            string fontPathBox = System.IO.Path.Combine(basePath, "fonts\\WINGDING.TTF");
            var item = survey.Questions;
            var writer = new PdfWriter(path);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            // Create a PdfFont

            var font = PdfFontFactory.CreateFont(fontPath, "CP1250");
            var fontBox = PdfFontFactory.CreateFont(fontPathBox, "CP1252");
            // Add a Paragraph
            //document.Add(new Paragraph($"Tytuł ankiety:  {item[0].Title}").SetFont(font));
            document.Add(new Paragraph($"Tytuł ankiety:  {survey.Name}").SetFont(font));
            foreach (var i in item)
            {
                if (i.IsActive)
                {
                    document.Add(new Paragraph($"Pytanie {i.Priority + 1}:  {i.Body}").SetFont(font));
                    if (i.Type == "Yes/No")
                    {
                        Cell cell = new Cell();
                        Paragraph p = new Paragraph();
                        Text t2 = new Text("o");
                        t2.SetFont(fontBox);
                        t2.SetFontSize(12);
                        p.Add(t2);
                        Text t1 = new Text(" Tak ");
                        t1.SetFont(font);
                        t1.SetFontSize(9);
                        p.Add(t1);
                        p.Add(t2);
                        t1 = new Text(" Nie ");
                        t1.SetFont(font);
                        t1.SetFontSize(9);
                        p.Add(t1);
                        document.Add(cell.Add(p));

                    }
                    else if (i.Type == "ABCD")
                    {
                        foreach (var i2 in i.SplitQues)
                        {
                            Cell cell = new Cell();
                            Paragraph p = new Paragraph();
                            Text t2 = new Text("o");
                            t2.SetFont(fontBox);
                            t2.SetFontSize(12);
                            p.Add(t2);
                            Text t1 = new Text(" " + i2);
                            t1.SetFont(font);
                            t1.SetFontSize(9);
                            p.Add(t1);

                            document.Add(cell.Add(p));
                        }
                        //i.SplitQues.ToList().ForEach(s => { document.Add(new Paragraph(s).SetFont(font)); });
                    }
                    else
                    {
                        document.Add(new Paragraph(String.Concat(Enumerable.Repeat(". ", 80))).SetFont(font));
                    }
                }
            }

            document.Close();
            ViewBag.PDF = "Stworzono PDF!";

            return View(survey);
        }
        public ActionResult DownloadPDF(int id)
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $"PDF\\{id}.pdf";
            string path = System.IO.Path.Combine(basePath, filePath);
            return File(path, "application/pdf");
        }
        public ActionResult ExportResult(int id)
        {

            var questions = new List<Question>();
            _db.Questions
                          .Where(q => q.SurveyId == id)
                          .OrderBy(q => q.Priority)
                          .Select(q => new
                          {
                              q.Title,
                              q.Body,
                              q.Type,
                              q.ABCDQuestions,
                              Answers = _db.Answers.Where(a => a.QuestionId == q.Id)
                          })
                          .ToList()
                          .ForEach(r => questions.Add(new Question
                          {
                              Title = r.Title,
                              Body = r.Body,
                              Type = r.Type,
                              ABCDQuestions = r.ABCDQuestions,
                              Answers = r.Answers.ToList()
                          })); ;

            var resp = new List<Response>();
            var survey = _db.Surveys.Single(s => s.Id == id);
            _db.Responses
                .Where(q => q.SurveyId == id)
                .Select(q => new
                {
                    q.CreatedOn
                })
                .ToList()
                .ForEach(r => resp.Add(new Response
                {
                    CreatedOn = r.CreatedOn
                }));
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, System.Text.Encoding.UTF8))
                {
                    using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        csv.WriteField(survey.Name);
                        csv.NextRecord();
                        csv.WriteField("Data udzielania odpowiedzi");
                        csv.WriteField("Id ankietowanego");
                        csv.WriteField("Tytuł");
                        csv.WriteField("Pytanie");
                        csv.WriteField("Typ pytania");
                        csv.WriteField("Odpowiedz");
                        
                        csv.WriteField("Odp A");
                        csv.WriteField("Odp B");
                        csv.WriteField("Odp C");
                        csv.WriteField("Odp D");
                        csv.NextRecord();
                        for (int i = 0; i < questions.Count; i++)
                        {
                            for (int j = 0; j < questions[i].Answers.Count; j++)
                            {
                                csv.WriteField(resp[j].CreatedOn);
                                csv.WriteField(questions[i].Answers[j].ResponseId);
                                csv.WriteField(questions[i].Title);
                                csv.WriteField(questions[i].Body);
                                csv.WriteField(questions[i].Type);
                                csv.WriteField(questions[i].Answers[j].Value);
                                 if (questions[i].Type == "ABCD")
                                {
                                    csv.WriteField(questions[i].ABCDQuestions.Split(new string[] { ";;" }, StringSplitOptions.None));
                                }
                                csv.NextRecord();
                            }
                        }
                    }
                }

                var arr = memoryStream.ToArray();
                return File(arr, "text/csv", $"{id}.csv");

            }


            //return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", $"{id}.csv");

        }
    }
}
