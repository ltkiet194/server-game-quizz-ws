using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerKaLoop.Applications.Messaging.Constants;
using ServerKaLoop.GameModels;
using ServerKaLoop.GameModels.Handlers;
using ServerKaLoop.Rooms.Constants;

namespace ServerKaLoop.Rooms.Handlers
{
    public class  GameRoom : BaseRoom
    {
        private readonly int _time;
        private Quizzes Quizzes;
        public bool _gameStarted;
        private int _currentQuestionIndex;
        private Timer _questionTimer;
        public long lastQuestionTime { get; set; }
        public string QuizzId { get; set; }
        public QuizzesHandler quizzHandler { get; set; }
        public RoomModel roomModel { get; set; }
        public RoomHandler roomHandler;

        public int MaxPlayers { get; set; }

        public GameRoom(string quizzId, int time = 10, int maxPlayers = 50) : base(RoomType.Game)
        {
            _time = time +1;
            QuizzId = quizzId;
            MaxPlayers = maxPlayers;
        }
        public void UpdateOnlineRoom()
        {
            this.roomModel.Online = true;
            roomHandler.UpdateByRoomCode(roomModel.RoomCode,roomModel);
        }
        public void StartGame()
        {
            if (!_gameStarted)
            {
                Quizzes = GetQuestionsFromDatabase(QuizzId);
                _gameStarted = true;
                SendMessageStart();
                _questionTimer = new Timer(state => SendNextQuestion(), null, TimeSpan.FromSeconds(6), TimeSpan.Zero);

            }
        }
        void SendMessageStart()
        {
            _currentQuestionIndex = 0;
            var startdata = new StartGameData();
            startdata.Message = "Tro Choi Bat Dau";
            this.SendMessage<StartGameData>(new WsMessage<StartGameData>(WsTags.StartGame, startdata));
        }
      
        public bool Answer(AnswerData answer)
        {
            if (_currentQuestionIndex <= Quizzes.questions.Count)
            {
                Question question = Quizzes.questions[_currentQuestionIndex-1];
                if (ConvertAnswer(answer.Answer) == question.options[4])
                {
                    return true;
                }
            }
            return false;
        }



        // func convert 0 1 2 3 TO A B C D
        public string ConvertAnswer(int answer)
        {
            switch (answer)
            {
                case 0:
                    return "A";
                case 1:
                    return "B";
                case 2:
                    return "C";
                case 3:
                    return "D";
            }
            return "";
        }
  
        public Quizzes GetQuestionsFromDatabase(string quizzId)
        {
            return quizzHandler.Find(quizzId);
        }

        void SendQuiz()
        {
            var nextQuestion = Quizzes.questions[_currentQuestionIndex];
            _currentQuestionIndex++;
            lastQuestionTime = DateTime.Now.Ticks;

            QuizData quizData = new QuizData
            {
                question_id = nextQuestion.question_id,
                question_text = nextQuestion.question_text,
                time = _time,
                indexQuiz = $"{_currentQuestionIndex}/{Quizzes.questions.Count}",
                AnswerA = nextQuestion.options[0],
                AnswerB = nextQuestion.options[1],
                AnswerC = nextQuestion.options[2],
                AnswerD = nextQuestion.options[3],
            };
            this.SendMessage<QuizData>(new WsMessage<QuizData>(WsTags.SendQuiz, quizData));
            this.SendMessage(GameHelper.ParseString(new WsMessage<string>(WsTags.FlipCard, "Flip card!")));
            ResetCanAnswerForAllPlayer();
        }
        void ResetCanAnswerForAllPlayer()
        {
            foreach(Player player in Players.Values)
            {   if(player.canAnswer)
                    player.SendMessage(GameHelper.ParseString(new WsMessage<string>(WsTags.FlipCard, "Flip card!")));
                player.canAnswer = true;         
            }
        }
            
       
        private void SendNextQuestion()
        {
            if (_currentQuestionIndex < Quizzes.questions.Count)
            {

                SendQuiz();

                _questionTimer = new Timer(state => SendNextQuestion(), null, TimeSpan.FromSeconds(_time), TimeSpan.Zero);

            }
            else
            {
                    NotifyData messageNotify = new NotifyData { type = "info", title = "Info", text = "Game ended!" };
                    this.SendMessage<NotifyData>(new WsMessage<NotifyData>(WsTags.Notify, messageNotify));
            }
        }
    }
}
