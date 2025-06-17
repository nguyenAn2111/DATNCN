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
            this.storage_id = row.Table.Columns.Contains("storage_id") && row["storage_id"] != DBNull.Value ? Convert.ToInt32(row["storage_id"]) : 0;
            this.str_quantity = row.Table.Columns.Contains("str_quantity") && row["str_quantity"] != DBNull.Value ? Convert.ToInt32(row["str_quantity"]) : 0;
            this.device_name = row.Table.Columns.Contains("device_name") && row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            this.FK_device_id = row.Table.Columns.Contains("FK_device_id") && row["FK_device_id"] != DBNull.Value ? row["FK_device_id"].ToString() : "";
            this.storage_date = row.Table.Columns.Contains("storage_date") && row["storage_date"] != DBNull.Value ? (DateTime)row["storage_date"] : DateTime.MinValue;
            this.FK_room_id_from = row.Table.Columns.Contains("FK_room_id_from") && row["FK_room_id_from"] != DBNull.Value ? row["FK_room_id_from"].ToString() : "";
            this.FK_room_id_to = row.Table.Columns.Contains("FK_room_id_to") && row["FK_room_id_to"] != DBNull.Value ? row["FK_room_id_to"].ToString() : "";
        }
    }
}
