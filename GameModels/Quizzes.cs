using ServerKaLoop.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.GameModels
{
    public class Quizzes: BaseModel
    {
        public string name { get; set; }

        public string description { get; set; }

        public string owner_email { get; set; }

        public List<Question> questions { get; set; }


    }
    public class Question
    {
        public string question_id { get; set; }

        public string question_text { get; set; }

        public List<string> options { get; set; }
    }
}
