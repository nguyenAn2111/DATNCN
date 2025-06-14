using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class DeviceDetail
    {
        public List<Device> devices_id { get; set; }
        public List<Status>statuses_device { get; set; }
        public List<Room> rooms_device { get; set; }
        public List<Contact> contacts_device { get; set; }
    }
}