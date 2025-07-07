using System;

namespace Hospital_Test.Models
{
	public class MaintainCreateModel
	{

		// Các trường mà client CẦN gửi lên để tạo một bản ghi bảo trì mới
		public string FK_device_id { get; set; } // Đây là trường bắt buộc trong logic của bạn
		public DateTime maintain_date { get; set; } // Đây là trường bắt buộc trong logic của bạn

		public string maintain_maintenance { get; set; }
		public string maintain_maintenance_phone { get; set; }
		public string maintain_delivery { get; set; }
		public string maintain_delivery_phone { get; set; }

		// Các trường liên quan đến contact mà bạn nhận từ request body
		public string contact_address { get; set; }
		public string contact_finance { get; set; }
	}
}
