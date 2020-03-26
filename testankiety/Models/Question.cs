using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testankiety.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
    }
}