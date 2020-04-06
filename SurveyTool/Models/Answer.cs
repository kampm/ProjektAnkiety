using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SurveyTool.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        public int ResponseId { get; set; }

        public Response Response { get; set; }

        public int QuestionId { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public Question Question { get; set; }

        public int Score
        {
            get
            {
                if (Question != null)
                {
                    if (Question.Type == "ABCD")
                    {
                        if (Value == "A")
                            return 1;
                        if (Value == "B")
                            return 2;
                        if (Value == "C")
                            return 3;
                        if (Value == "D")
                            return 4;
                        else
                            return 0;
                    }


                    if (Question.Type == "Text")
                        return Value != null ? 1 : 0;

                    if (Question.Type == "Yes/No")
                        return Value == "Yes" ? 1 : 0;

                    if (Question.Type == "Number")
                    {
                        int num;
                        Int32.TryParse(Value, out num);
                        return num > 0 ? 1 : 0;
                    }
                }

                return 0;
            }
        }
    }
}