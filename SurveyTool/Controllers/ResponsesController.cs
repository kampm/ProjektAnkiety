using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyTool.Models;

namespace SurveyTool.Controllers
{
    [Authorize]
    public class ResponsesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResponsesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index(int surveyId)
        {
            var responses = _db.Responses
                               .Include("Survey")
                               .Include("Answers")
                               .Include("Answers.Question")
                               .Where(x => x.SurveyId == surveyId)
                               .Where(x => x.CreatedBy == User.Identity.Name)
                               .OrderByDescending(x => x.CreatedOn)
                               .ThenByDescending(x => x.Id)
                               .ToList();

            return View(responses);
        }

        [HttpGet]
        public ActionResult Details(int surveyId, int id)
        {
            var response = _db.Responses
                              .Include("Survey")
                              .Include("Answers")
                              .Include("Answers.Question")
                              .Where(x => x.SurveyId == surveyId)
                              .Where(x => x.CreatedBy == User.Identity.Name)
                              .Single(x => x.Id == id);

            response.Answers = response.Answers.OrderBy(x => x.Question.Priority).ToList();
            return View(response);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Create(int surveyId)
        {
            HttpCookie cookie = Request.Cookies.Get("resplist");
            if (cookie != null)
            {
                string dataAsString = cookie.Value;
                List<string> data = new List<string>();
                //string guidCookie = cookie.Value.Split('|').FirstOrDefault();
                dataAsString = dataAsString.Substring(dataAsString.IndexOf("|") + 1);
                data.AddRange(dataAsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                data.Contains(surveyId.ToString());
                if (data.Contains(surveyId.ToString()) == true)
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            var survey = _db.Surveys
                            .Where(s => s.Id == surveyId)
                            .Select(s => new
                            {
                                Survey = s,
                                Questions = s.Questions
                                                 .Where(q => q.IsActive)
                                                 .OrderBy(q => q.Priority)
                            })
                             .AsEnumerable()
                             .Select(x =>
                                 {
                                     x.Survey.Questions = x.Questions.ToList();
                                     return x.Survey;
                                 })
                             .Single();

            return View(survey);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(int surveyId, string action, Response model)
        {
            if (model.Answers == null)
            {
                ViewBag.Message = "Brak pytań!";
                return View("Error");
            }
            HttpCookie cookie = Request.Cookies.Get("resplist");
            List<string> data = new List<string>();
            string guidCookie = "";
            if (cookie != null)
            {
                guidCookie = cookie.Value.Split('|').FirstOrDefault();
                var cookieV = cookie.Value.Substring(cookie.Value.IndexOf("|") + 1);
                data.AddRange(cookieV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                cookie = new HttpCookie("resplist");
                guidCookie = Guid.NewGuid().ToString();
            }

            data.Add(surveyId.ToString());
            string dataAsString = data.Aggregate((a, b) => a = a + "," + b);

            cookie.Value = guidCookie + "|" + dataAsString;

            Response.Cookies.Add(new HttpCookie("resplist", cookie.Value.ToString()));

            model.Answers = model.Answers.Where(a => !String.IsNullOrEmpty(a.Value)).ToList();
            model.SurveyId = surveyId;
            model.CreatedBy = User.Identity.IsAuthenticated ? User.Identity.Name : guidCookie;
            model.CreatedOn = DateTime.Now;
            _db.Responses.Add(model);
            _db.SaveChanges();

            TempData["success"] = "Twoje odpowiedzi zostały zapisane!";

            return action == "Next"
                       ? RedirectToAction("Create", new { surveyId })
                       : RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Delete(int surveyId, int id, string returnTo)
        {
            var response = new Response() { Id = id, SurveyId = surveyId };
            _db.Entry(response).State = EntityState.Deleted;
            _db.SaveChanges();
            return Redirect(returnTo ?? Url.RouteUrl("Root"));
        }
    }
}