using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Messaging.Constants
{
    public struct QuizData
    {
        public string question_id { get; set; }

        public string question_text { get; set; }

        public int time { get; set; }
        public string indexQuiz { get; set; }

        public string AnswerA { get; set; }

        public string AnswerB { get; set; }

        public string AnswerC { get; set; }

        public string AnswerD { get; set; }
    }
}
