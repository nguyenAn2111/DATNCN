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
        public int repair_update_status { get; set; }
        public string repair_note { get; set; }
        public byte[] repair_picture { get; set; }
        public string repair_update_note { get; set; }
        public string FK_contact_id { get; set; }
        public string FK_room_id { get; set; }
        public string FK_device_id { get; set; }
        public string FK_status_id { get; set; }
        public string FK_finance_id { get; set; }
        public string device_name { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }

        public Repair(DataRow row)
        {
            this.FK_device_id = (string)row["FK_device_id"];
            this.repair_broken = (DateTime)row["repair_broken"];
            this.repair_date = (DateTime)row["repair_date"];
            this.FK_room_id = (string)row["FK_room_id"];

            this.device_name = row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            this.room_name = row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            this.status_name = row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";

        }
    }
}
