using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class MaintainDetail
    {
        public List<Device> devices { get; set; }
        public List<Room> rooms { get; set; }
    }
}
