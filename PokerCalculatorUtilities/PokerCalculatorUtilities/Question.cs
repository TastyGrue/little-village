using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizUtilities
{
    class Question
    {
        public string QuestionText { get; protected set; }
        public List<Answer> PotentialAnswers { get; protected set; }

        public Question(string text, List<Answer> Answers)
        {
            QuestionText = text;
            PotentialAnswers = Answers;
        }

        public void ReplaceAnswers(List<Answer> newAnswers)
        {
            PotentialAnswers = newAnswers;
        }

        public void ReplaceText(string newText)
        {
            QuestionText = newText;
        }
    }
}
