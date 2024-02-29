using ServerKaLoop.GameModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.GameModels
{
    public class User: BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        public string Avatar { get; set; }

        public long Score { get; set; }

        public long Amount { get; set; }

        public bool IsOnline { get; set; } = false;

        public string email = "";
        public bool IsAdmin { get; set; }

        public User(string username, string password, string displayName)
        {
            Username = username;
            Password = GameHelper.hashPasswordMD5(password);
            DisplayName = displayName;
            Avatar= "https://cdn-icons-png.flaticon.com/512/149/149071.png";
            Score = 0l;
            Amount = 0;
        }
        public User()
        {
            Avatar = "https://cdn-icons-png.flaticon.com/512/149/149071.png";
            Score = 0l;
            Amount = 0;
        }

        public User(string username, string password, string displayName,string avatar)
        {
            Username = username;
            Password = GameHelper.hashPasswordMD5(password);
            DisplayName = displayName;
            Avatar = avatar;
    
        }
    }
}