using System.Data;
using System;


namespace Hospital_Test.Models
{
    public class Maintain
    {
        public string maintain_id { get; set; }
        public DateTime maintain_date { get; set; }
        public string maintain_maintenance { get; set; }
        public string maintain_maintenance_phone { get; set; }
        public string maintain_delivery { get; set; }
        public string maintain_delivery_phone { get; set; }
        public string FK_device_id { get; set; }
        public string FK_status_id { get; set; }
        public string FK_room_id { get; set; }
        public string FK_finace_id { get; set; }
        public string FK_contact_id { get; set; }
        public string device_name { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }

        public Maintain(DataRow row)
        {
            FK_device_id = (string)row["FK_device_id"];
            maintain_date = (DateTime)row["maintain_date"];
            FK_room_id = (string)row["FK_room_id"];

            device_name = row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            room_name = row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            status_name = row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";

        }
    }

}
