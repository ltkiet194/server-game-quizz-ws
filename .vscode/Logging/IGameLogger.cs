

namespace ServerKaLoop.Logging
{
    public interface IGameLogger
    {
        void Print(string message);
        void Info(string message);
        void Warning(string message,Exception exception);

        void Error(string message , Exception exception);
        void Error(string message);
    }
}
