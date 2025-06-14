using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Hospital_Test.Models;
using System.IO;
using Hospital_Test.DAO;
using Hospital_Test.Models.Login;

namespace Hospital_Test.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }

        public List<Account> GetAccountList()
        {
            List<Account> items = DataProvider<Account>.Instance.GetListItem("tbl_account");
            return items;
        }

        public List<Account> GetAccountList(string col, string val)
        {
            List<Account> items = DataProvider<Account>.Instance.GetListItem(col, val);
            return items;
        }

        public Account GetAccountbyUsername(string accname)
        {
            return DataProvider<Account>.Instance.GetItem("account_username", accname, "tbl_account");
        }

        public void DeleteAccount(String accname)
        {
            DataProvider<Account>.Instance.DeleteItem("account_username", accname);
        }

        public void AddAccount(Account account)
        {
            DataTable data = DataProvider<Account>.Instance.LoadData();
            DataRow newAccount = data.NewRow();

            var allAttr = typeof(Account).GetProperties(); // Lấy danh sách attributes của class Account

            foreach (var attr in allAttr)
                newAccount[attr.Name] = attr.GetValue(account);


            data.Rows.Add(newAccount);

            DataProvider<Account>.Instance.UpdateData(data);
        }

        public void ChangePassword(string accname, string newPass)
        {
            DataTable data = DataProvider<Account>.Instance.LoadData();
            DataRow newAccount = data.Select("Account_Username=" + accname).FirstOrDefault();

            newAccount["Account_Password"] = newPass;

            DataProvider<Account>.Instance.UpdateData(data);
        }
    }
}
