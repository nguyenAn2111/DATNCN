using System;

namespace Hospital_Test.Models
{
	public class RepairCreateModel
	{
		// Các trường gửi từ MobileApp
		public string FK_device_id { get; set; } = null!;
		public string contact_address { get; set; } = null!;
		public string contact_finance { get; set; } = "0";
		public DateTime repair_broken { get; set; }
		public int repair_priority { get; set; }
		public DateTime repair_date { get; set; }
		public string repair_note { get; set; } = "";
		public string? repair_picture { get; set; }

		// ⚠️ GIỮ các trường mặc định để controller có thể truy cập — nhưng cho phép nullable
		public string? FK_status_id { get; set; }
		public string? FK_room_id { get; set; }
		public string? repair_update_status { get; set; }
		public string? repair_update_note { get; set; }
		public DateTime? repair_update_date { get; set; }
	}
}



