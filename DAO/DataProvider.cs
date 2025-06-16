using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.InkML;
using Hospital_Test.Models;

namespace Hospital_Test.DAO
{
    public class DataProvider<T>
    {
        private static DataProvider<T> instance;
        public static DataProvider<T> Instance
        {
            get { if (instance == null) instance = new DataProvider<T>(); return DataProvider<T>.instance; }
            private set { DataProvider<T>.instance = value; }
        }
        private DataProvider() { }

        //private string connectionSTR = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=HTQLTBYT;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        //private string connectionSTR = @"Data Source=NGOXUANHINH2801;Initial Catalog=Hethongquanlylab;Integrated Security=True";
        //private string connectionSTR = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=HTQLVTYT;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        private string connectionSTR = @"Data Source= WARMACHINE-2137;Initial Catalog=HTQLTBYT;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public DataTable ExcuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                SqlConnection connection = new SqlConnection(connectionSTR);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }


        public List<T> GetListItem(string tableName)
        {

            string query = "select * from dbo." + tableName;
            DataTable data = ExcuteQuery(query);

            var list = new List<T>();
            foreach (DataRow dr in data.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), dr);
                list.Add(item);
            }
            return list;
        }
        public List<T> GetListItemQuery(string query)
        {
            DataTable data = ExcuteQuery(query);

            var list = new List<T>();
            foreach (DataRow dr in data.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), dr);
                list.Add(item);
            }
            return list;
        }
        public List<T> GetListItem(string col, string val, string tableName = "")
        {
            if (tableName == "") tableName = typeof(T).Name;
            string query = String.Format("select * from dbo.{0} where [{1}] = N'{2}'", tableName, col, val);
            DataTable data = ExcuteQuery(query);

            var list = new List<T>();
            foreach (DataRow dr in data.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), dr);
                list.Add(item);
            }
            return list;
        }

        public List<T> GetListItem(string col, int val, string tableName = "")
        {
            if (tableName == "") tableName = typeof(T).Name;
            string query = String.Format("select * from dbo.{0} where [{1}] = {2}", tableName, col, val);
            DataTable data = ExcuteQuery(query);

            var list = new List<T>();
            foreach (DataRow dr in data.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), dr);
                list.Add(item);
            }
            return list;
        }


        public T GetItem(string col, string val, string tableName)
        {
            string query = String.Format("select * from dbo.{0} where [{1}] = N'{2}'", tableName, col, val);
            DataTable data = ExcuteQuery(query);

            try
            {
                return (T)Activator.CreateInstance(typeof(T), data.Rows[0]);
            }
            catch
            {
                return default(T);
            }
        }

        public T GetItem(string col, int val, string tableName)
        {
            string query = String.Format("select * from dbo.{0} where [{1}] = {2}", tableName, col, val);
            DataTable data = ExcuteQuery(query);

            try
            {
                return (T)Activator.CreateInstance(typeof(T), data.Rows[0]);
            }
            catch
            {
                return default(T);
            }
        }

        
        public DataTable LoadData(string tableName = "")
        {
            if (tableName == "") tableName = typeof(T).Name;
            string query = "select * from " + tableName;
            DataTable data = DataProvider<T>.Instance.ExcuteQuery(query);
            return data;
        }

        public void UpdateData(DataTable dt, string tableName = "")
        {
            if (tableName == "") tableName = typeof(T).Name;
            string query = "select * from " + tableName;
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                SqlConnection connection = new SqlConnection(connectionSTR);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                builder.GetInsertCommand();

                adapter.Update(dt);
                connection.Close();
            }
        }

        public void DeleteItem(string col, string val, string tableName = "")
        {
            if (tableName == "") tableName = typeof(T).Name;
            string query = String.Format("delete from dbo.{0} where [{1}] = '{2}'", tableName, col, val);
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                SqlConnection connection = new SqlConnection(connectionSTR);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}