namespace Hospital_Test.Models
{
    public class StorageDetail
    {
        public List<Storage> str_id { get; set; }
        public List<Device> devices_import { get; set; }  // Thiết bị có thể nhập vào kho (không ở kho)
        public List<Device> devices_export { get; set; }  // Thiết bị có thể xuất ra khỏi kho (đang ở kho)
        public List<Room> rooms_str { get; set; }
    }
}
