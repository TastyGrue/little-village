using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizUtilities
{
    public class Answer
    {
        public string text { get; protected set; }
        public bool isCorrect { get; protected set; }

        public Answer(string AnswerText, bool IsCorrect)
        {
            text = AnswerText;
            isCorrect = IsCorrect;
        }
    }
}
