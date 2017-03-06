using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizUtilities;

namespace QABank
{
    public class QABank
    {
        List<Question> QBank;

        public QABank()
        {
            QBank = new List<Question>();
        }

        public void AddQuestion(string QuestionText, int QuestionPriority, List<Answer> Answers)
        {
            QBank.Add(new Question(QuestionText, Answers, QuestionPriority));
            
        }
    }
}
