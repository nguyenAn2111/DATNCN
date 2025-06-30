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
        public string FK_finace_id { get; set; }
        public string FK_contact_id { get; set; }
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string room_name { get; set; }
        public string status_name { get; set; }
		public string contact_finance { get; set; }
		public string contact_address { get; set; }
		public DateTime? device_maintenance_start { get; set; }
		public int? device_maintenance_cycle { get; set; }
		public DateTime? expiration_date { get; set; }
		public string DeviceStatusId { get; set; }
		public Maintain(DataRow row)
        {
            if (row["maintain_date"] != DBNull.Value)
                maintain_date = (DateTime)row["maintain_date"];
            else
                maintain_date = null;

            maintain_id = row["maintain_id"] != DBNull.Value ? row["maintain_id"].ToString() : "";
            device_id = row.Table.Columns.Contains("device_id") && row["device_id"] != DBNull.Value ? row["device_id"].ToString() : "";
            FK_room_id = row["FK_room_id"] != DBNull.Value ? row["FK_room_id"].ToString() : null;
            device_name = row.Table.Columns.Contains("device_name") && row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            room_name = row.Table.Columns.Contains("room_name") && row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            status_name = row.Table.Columns.Contains("status_name") && row["status_name"] != DBNull.Value ? row["status_name"].ToString() : "";
            maintain_maintenance = row["maintain_maintenance"] != DBNull.Value ? row["maintain_maintenance"].ToString() : "";
            maintain_maintenance_phone = row["maintain_maintenance_phone"] != DBNull.Value ? row["maintain_maintenance_phone"].ToString() : "";
            maintain_delivery = row["maintain_delivery"] != DBNull.Value ? row["maintain_delivery"].ToString() : "";
            maintain_delivery_phone = row["maintain_delivery_phone"] != DBNull.Value ? row["maintain_delivery_phone"].ToString() : "";
			contact_finance = row.Table.Columns.Contains("contact_finance") && row["contact_finance"] != DBNull.Value
			  ? row["contact_finance"].ToString()
			  : "";
			contact_address = row.Table.Columns.Contains("contact_address") && row["contact_address"] != DBNull.Value
			  ? row["contact_address"].ToString()
			  : "";
			device_maintenance_start = row.Table.Columns.Contains("device_maintenance_start") && row["device_maintenance_start"] != DBNull.Value
	? (DateTime?)row["device_maintenance_start"]
	: null;

			device_maintenance_cycle = row.Table.Columns.Contains("device_maintenance_cycle") && row["device_maintenance_cycle"] != DBNull.Value
				? Convert.ToInt32(row["device_maintenance_cycle"])
				: null;

			expiration_date = row.Table.Columns.Contains("expiration_date") && row["expiration_date"] != DBNull.Value
				? (DateTime?)row["expiration_date"]
				: null;
			FK_status_id = row.Table.Columns.Contains("FK_status_id") && row["FK_status_id"] != DBNull.Value
	? row["FK_status_id"].ToString()
	: "";
		}
    }

}
