namespace Hospital_Test.Models
{
	public class DeviceCreateModel
	{
		public string device_id { get; set; }
		public string device_name { get; set; }
		public string device_manufacturer { get; set; }
		public string device_seri { get; set; }
		public string device_type { get; set; }
		public string device_group { get; set; }
		public DateTime? device_maintenance_start { get; set; }
		public int? device_maintenance_cycle { get; set; }
		public DateTime? device_stockout_date { get; set; }
		public string device_condition { get; set; }
		public DateTime? device_received_date { get; set; }
		public string device_note { get; set; }
		public string FK_status_id { get; set; }
		public string? FK_room_id { get; set; }
		public string? FK_contact_id { get; set; }
	}
}
