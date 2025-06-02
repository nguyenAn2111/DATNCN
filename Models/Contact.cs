using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Contact
    {
        public string contact_id { get; set; }
        public int contact_type { get; set; }
        public string contact_address { get; set; }
        public int contact_finance { get; set; }

        public Contact(DataRow row)
        {
            this.contact_id = row["contact_id"] != DBNull.Value ? row["contact_id"].ToString() : "";
        }
        public Contact() { }
    }
}
