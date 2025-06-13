using System.Data;
using System;


namespace Hospital_Test.Models
{
    public class Repair
    {
        public string repair_id { get; set; }
        public DateTime repair_broken { get; set; }
        public int repair_priority { get; set; }
        public DateTime repair_date { get; set; }
        public DateTime repair_update_date { get; set; }
        public string repair_update_status { get; set; }
        public string repair_note { get; set; }
        public string repair_picture { get; set; }
        public string repair_update_note { get; set; }
        public string FK_contact_id { get; set; }
        public string FK_room_id { get; set; }
        public string FK_device_id { get; set; }
        public string FK_status_id { get; set; }
        public string device_name { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }
        public int contact_finance { get; set; }
        public string contact_address { get; set; }

        public Repair(DataRow row)
        {
            this.FK_room_id = row.Table.Columns.Contains("FK_room_id") && row["FK_room_id"] != DBNull.Value ? row["FK_room_id"].ToString() : "";
            this.repair_id = row.Table.Columns.Contains("repair_id") && row["repair_id"] != DBNull.Value ? row["repair_id"].ToString() : "";

            this.repair_broken = row.Table.Columns.Contains("repair_broken") && row["repair_broken"] != DBNull.Value ? (DateTime)row["repair_broken"] : DateTime.MinValue;
            this.repair_date = row.Table.Columns.Contains("repair_date") && row["repair_date"] != DBNull.Value ? (DateTime)row["repair_date"] : DateTime.MinValue;
            this.repair_update_date = row.Table.Columns.Contains("repair_update_date") && row["repair_update_date"] != DBNull.Value ? (DateTime)row["repair_update_date"] : DateTime.MinValue;

            this.repair_picture = row.Table.Columns.Contains("repair_picture") && row["repair_picture"] != DBNull.Value ? row["repair_picture"].ToString() : "";
            this.repair_update_note = row.Table.Columns.Contains("repair_update_note") && row["repair_update_note"] != DBNull.Value ? row["repair_update_note"].ToString() : "";
            this.repair_update_status = row.Table.Columns.Contains("repair_update_status") && row["repair_update_status"] != DBNull.Value ? row["repair_update_status"].ToString() : "";
            this.repair_note = row.Table.Columns.Contains("repair_note") && row["repair_note"] != DBNull.Value ? row["repair_note"].ToString() : "";

            this.FK_device_id = row.Table.Columns.Contains("FK_device_id") && row["FK_device_id"] != DBNull.Value ? row["FK_device_id"].ToString() : "";

            this.repair_priority = row.Table.Columns.Contains("repair_priority") && row["repair_priority"] != DBNull.Value ? Convert.ToInt32(row["repair_priority"]) : 0;
            this.contact_finance = row.Table.Columns.Contains("contact_finance") && row["contact_finance"] != DBNull.Value ? Convert.ToInt32(row["contact_finance"]) : 0;

            this.device_name = row.Table.Columns.Contains("device_name") && row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            this.room_name = row.Table.Columns.Contains("room_name") && row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            this.status_name = row.Table.Columns.Contains("status_name") && row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";
            this.contact_address = row.Table.Columns.Contains("contact_address") && row["contact_address"] != DBNull.Value ? row["contact_address"].ToString() : "";
        }

    }
}
