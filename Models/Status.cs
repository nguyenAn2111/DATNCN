using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }

        public Status(DataRow row)
        {
            status_id = row["status_id"] != DBNull.Value ? row["status_id"].ToString() : "";
            status_name = row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";
        }

        public Status() { }
    }
}
