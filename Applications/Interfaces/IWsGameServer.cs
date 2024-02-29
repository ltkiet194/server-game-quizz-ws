using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Applications.Interfaces
{
    public interface IWsGameServer
    {
        void StartServer();
        void StopServer();

        void RestartServer();

    }
}
