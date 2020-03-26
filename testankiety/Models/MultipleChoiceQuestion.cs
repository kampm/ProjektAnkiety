using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testankiety.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public int Answer { get; set; }
        public List<MultipleChoiceAnswer> PossibleAnswers { get; set; }
    }
}