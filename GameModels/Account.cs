using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerKaLoop.GameModels.Base;

namespace ServerKaLoop.GameModels
{
    public class Account: BaseModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public Token Token { get; set; }

        public Account(string email, string password, Token token)
        {
            this.email = email;
            this.password = password;
            this.Token = token;
        }
        public Account(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
  

    }
    public class Token
    {
        public string TokenId { get; set; }
        public DateTime Expired { get; set; }
    }
    
}
