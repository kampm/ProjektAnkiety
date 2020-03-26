using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testankiety.Models
{
    public class MultipleChoiceAnswer
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string Text { get; set; }
    }
}