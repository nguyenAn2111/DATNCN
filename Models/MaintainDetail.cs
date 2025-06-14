using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class MaintainDetail
    {
        public List<Maintain> maintains_id { get; set; }
        public List<Device> devices_maintain { get; set; }
        public List<Room> rooms_maintain { get; set; }
        public List<Contact> contacts_maintain { get; set; }
    }
}