using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class UserExperienceModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserUsername { get; set; }
        public string Title { get; set; }

        public string Company { get; set; }
        public string Description { get; set; }
    }
}
