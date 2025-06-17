namespace Hospital_Test.Models
{
    public class RepairDetail
    {
        public List<Repair> repairs_id { get; set; }
        public List<Device> devices_repair { get; set; }
        public List<Room> rooms_repair { get; set; }
        public List<Contact> contacts_repair { get; set; }
    }
}