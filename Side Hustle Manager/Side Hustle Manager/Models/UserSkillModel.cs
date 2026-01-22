using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class UserSkillModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserUsername { get; set; } // veza s UserModel
        public string SkillName { get; set; }
    }
}
