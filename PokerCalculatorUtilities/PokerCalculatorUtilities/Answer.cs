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

        /// <summary>
        /// Priority level: from 0 to 10, 0 being the top and 10 being bottom of the answers. Defaults to 5.
        /// </summary>
        public int PriorityLevel { get; protected set; } = 5;

        public Answer(string AnswerText, bool IsCorrect)
        {
            text = AnswerText;
            isCorrect = IsCorrect;
        }

        public Answer(string AnswerText, bool IsCorrect, int priorityLevel)
        {
            text = AnswerText;
            isCorrect = IsCorrect;
            PriorityLevel = priorityLevel;
        }
    }
}
