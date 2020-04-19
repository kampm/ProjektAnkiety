﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SurveyTool.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public int SurveyId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Body { get; set; }

        public int Priority { get; set; }
        public string ABCDQuestions { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public List<Answer> Answers { get; set; }
        public IList<String> SplitQues
        {
            get
            {

                if (ABCDQuestions != null)
                {
                    List<string> s = new List<string>(
                   ABCDQuestions.Split(new string[] { ";;" }, StringSplitOptions.None));
                    return s;
                }
                else
                {
                    List<string> c = new List<string>();
                    c.Add("i");
                    return c;
                }



            }
        }

    }
}