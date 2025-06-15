using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test.Models.Login
{
    public class Account
    {
        private string account_username;
        private string account_password;
        public string Username { get => account_username; set => account_username = value; }
        public string Password { get => account_password; set => account_password = value; }

        public Account() { }
        public Account(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public Account(DataRow row)
        {
            this.Username = (string)row["account_username"];
            this.Password = (string)row["account_password"];
        }
    }
}
