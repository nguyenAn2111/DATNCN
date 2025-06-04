using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Device
    {
        public string device_id { get; set; } //mã id, làm theo hướng mapping dictionary 
        public string device_name { get; set; }  //Tên thiết bị, viết full
        public string device_manufacturer { get; set; } //Nhà sản xuất, viết tắt 
        public string device_seri { get; set; } //mã series, chọn 4 số cuối để làm ID
        public string device_type { get; set; } // Loại thiết bị, viết tắt 
        public string device_group { get; set; } // Nhóm thiết bị, chọn theo select thay Hãng = nhóm thiết bị
        public int device_maintenance_cycle { get; set; } // Vòng đời bảo trì
        public DateTime device_maintenance_start { get; set; } //Ngày nhập kho
        public DateTime device_stockout_date { get; set; } // Ngày xuất kho, nếu chưa xuất kho lần nào, giá trị nhập vào csdl sẽ hiển thị là NULL, trong database có nút để tick, cho phép nó là NULL
        public int device_condition { get; set; } // tình trạng nhập về, có thể làm select
        public DateTime device_received_date { get; set; } // Ngày nhập về
        public string device_note { get; set; } //text bình thường, có thể NULL
        public string FK_contact_id { get; set; } // Sẽ thêm cùng với giá nhập vào, đã có hàm bên Maintain, hãy tham khảo
        public string FK_status_id { get; set; } // Tình trạng thiết bị - Mặc định là "Đang sử dụng" 
        public string FK_room_id { get; set; } //với mỗi một khoa [room-type], chọn ra các [room-id] có cùng [room-type]


        public Device(DataRow row)
        {
            this.device_id = row["device_id"] != DBNull.Value ? row["device_id"].ToString() : "";
            this.device_name = row["device_name"] != DBNull.Value ? row["device_name"].ToString() : "";
        }
        public Device() { }
    }
}
