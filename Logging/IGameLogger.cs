using System;

namespace ServerKaLoop.Logging
{
    public interface IGameLogger
    {
        void Print(string msg);
        void Info(string info);
        void Warning(string ms, Exception exception);
        void Error(string error, Exception exception);
        void Error(string error);
    }
}