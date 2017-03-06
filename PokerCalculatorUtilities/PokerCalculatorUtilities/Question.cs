using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizUtilities
{
    public class Question
    {
        public string QuestionText { get; protected set; }
        public List<Answer> PotentialAnswers { get; protected set; }

        /// <summary>
        /// Priority level: from 0 to 10, 0 being the top and 10 being bottom of the answers. Defaults to 5
        /// </summary>
        public int PriorityLevel { get; protected set; } = 5;

        public Question(string text, List<Answer> Answers)
        {
            QuestionText = text;
            PotentialAnswers = Answers;
        }
        
        public Question(string text, List<Answer> Answers, int priorityLevel)
        {
            QuestionText = text;
            PotentialAnswers = Answers;
            PriorityLevel = priorityLevel;
        }

        public void ReplaceAnswers(List<Answer> newAnswers)
        {
            PotentialAnswers = newAnswers;
        }

        public void ReplaceText(string newText)
        {
            QuestionText = newText;
        }

        public void ReplacePriorityLevel(int newLevel)
        {
            PriorityLevel = newLevel;
        }
    }
}
