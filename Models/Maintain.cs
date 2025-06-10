using System.Data;
using System;


namespace Hospital_Test.Models
{
    public class Maintain
    {
        public string maintain_id { get; set; }
        public DateTime? maintain_date { get; set; }
        public string maintain_maintenance { get; set; }
        public string maintain_maintenance_phone { get; set; }
        public string maintain_delivery { get; set; }
        public string maintain_delivery_phone { get; set; }
        public string FK_device_id { get; set; }
        public string FK_status_id { get; set; }
        public string FK_room_id { get; set; }
        public string FK_contact_id { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }

        public Maintain(DataRow row)
        {
            if (row["maintain_date"] != DBNull.Value)
                maintain_date = (DateTime)row["maintain_date"];
            else
                maintain_date = null;

            maintain_id = row["maintain_id"] != DBNull.Value ? row["maintain_id"].ToString() : "";
            device_id = row["device_id"] != DBNull.Value ? row["device_id"].ToString() : null;
            FK_room_id = row["FK_room_id"] != DBNull.Value ? row["FK_room_id"].ToString() : null;
            device_name = row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            room_name = row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            status_name = row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";
            maintain_maintenance = row["maintain_maintenance"] != DBNull.Value ? row["maintain_maintenance"].ToString() : "";
            maintain_maintenance_phone = row["maintain_maintenance_phone"] != DBNull.Value ? row["maintain_maintenance_phone"].ToString() : "";

        }

    }

}
