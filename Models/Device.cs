using System.Data;
using System;
using System.IO;

namespace Hospital_Test.Models
{
    public class Device
    {
        public string device_id { get; set; }
        public string device_name { get; set; }
        public string device_manufacturer { get; set; }
        public string device_seri { get; set; }
        public string device_type { get; set; }
        public string device_group { get; set; }
        public int device_maintenance_cycle { get; set; }
        public DateTime? device_maintenance_start { get; set; } // Thay đổi ở đây
        public DateTime? device_stockout_date { get; set; }   // Thay đổi ở đây
        public string device_condition { get; set; }
        public DateTime? device_received_date { get; set; }    // Thay đổi ở đây
        public string device_note { get; set; }
        public string FK_contact_id { get; set; }
        public string FK_status_id { get; set; }
        public string FK_room_id { get; set; }
        public string room_type { get; set; }
        public string contact_address { get; set; } // Thay đổi từ string sang byte[]
        public string room_name { get; set; }
        public string status_name { get; set; }
		public string contact_finance { get; set; }
		public string contact_type { get; set; }
	
		public Device(DataRow row)
        {
            this.device_id = row["device_id"] != DBNull.Value ? row["device_id"].ToString() : "";
            this.device_name = row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
            this.device_manufacturer = row["device_manufacturer"]?.ToString();
            this.device_seri = row["device_seri"]?.ToString();
            this.device_type = row["device_type"]?.ToString();
            this.device_group = row["device_group"]?.ToString();
            this.device_condition = row["device_condition"] != DBNull.Value ? row["device_condition"].ToString() : "";

            this.device_maintenance_cycle = row["device_maintenance_cycle"] != DBNull.Value ? Convert.ToInt32(row["device_maintenance_cycle"]) : 0;

            // Cập nhật cách gán giá trị cho các trường DateTime?
            this.device_maintenance_start = row["device_maintenance_start"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["device_maintenance_start"]) : null;
            this.device_stockout_date = row["device_stockout_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["device_stockout_date"]) : null;
            this.device_received_date = row["device_received_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["device_received_date"]) : null;

            this.device_note = row["device_note"]?.ToString();

            FK_status_id = row["FK_status_id"]?.ToString();
            FK_contact_id = row["FK_contact_id"]?.ToString();
            FK_room_id = row["FK_room_id"]?.ToString();

			this.contact_address = row.Table.Columns.Contains("contact_address") && row["contact_address"] != DBNull.Value
				  ? row["contact_address"].ToString()
				  : null;

			contact_type = row.Table.Columns.Contains("contact_type") && row["contact_type"] != DBNull.Value
			  ? row["contact_type"].ToString()
			  : "";

			contact_finance = row.Table.Columns.Contains("contact_finance") && row["contact_finance"] != DBNull.Value
			  ? row["contact_finance"].ToString()
			  : "";

			room_name = row.Table.Columns.Contains("room_name") && row["room_name"] != DBNull.Value
				? row["room_name"].ToString()
				: "";

			room_type = row.Table.Columns.Contains("room_type") && row["room_type"] != DBNull.Value
				? row["room_type"].ToString()
				: "";

			status_name = row.Table.Columns.Contains("status_name") && row["status_name"] != DBNull.Value
				? row["status_name"].ToString()
				: "";
			
		}
    }
}