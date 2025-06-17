using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Storage
    {
        public string device_name { get; set; }
        public int str_quantity { get; set; }
        public int storage_id { get; set; }
        public DateTime storage_date { get; set; }
        public string FK_device_id { get; set; }
        public string FK_room_id_from { get; set; }
        public string FK_room_id_to { get; set; }

        public Storage() { }
        public Storage(DataRow row)
        {
            this.str_quantity = row.Table.Columns.Contains("str_quantity") && row["str_quantity"] != DBNull.Value ? Convert.ToInt32(row["str_quantity"]) : 0;
            this.device_name = row.Table.Columns.Contains("device_name") && row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
        }
    }
}
