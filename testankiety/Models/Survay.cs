using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testankiety.Models
{
    public class Survay
    {

        public int SurvayID { get; set; }
        public string Title { get; set; }
        public List<YesNoQuestion> YesNoQuestions { get; set; }
        public List<TextQuestion> TextQuestions { get; set; }
        public List<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }

        [ForeignKey("UserAcc")]
        public int UserAccRefId { get; set; }
        public UserAcc UserAcc { get; set; }
    }
}