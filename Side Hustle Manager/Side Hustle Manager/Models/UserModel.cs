using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }


        public string ProfileImagePath { get; set; }

        public string Location { get; set; }
    }
}
