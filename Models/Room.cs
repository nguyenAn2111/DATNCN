using System.Data;
using System;

namespace Hospital_Test.Models
{
    public class Room
    {
        public string room_id { get; set; }
        public string room_type { get; set; }
        public string room_name { get; set; }

        public Room(DataRow row)
        {
            this.room_id = row["room_id"] != DBNull.Value ? row["room_id"].ToString() : "";
            this.room_name = row["room_name"] != DBNull.Value ? row["room_name"].ToString() : "";
            this.room_type = row["room_type"] != DBNull.Value ? row["room_type"].ToString() : "";
        }

        public Room() { }
    }
}
