using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Storage
    {
        public string storage_id { get; set; }
        public int storage_quantity { get; set; }
        public int storage_min { get; set; }
        public string FK_device_id { get; set; }
        public string FK_status_id { get; set; }
    }
}
