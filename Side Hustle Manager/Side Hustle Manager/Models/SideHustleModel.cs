using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class SideHustleModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Pay { get; set; }
        public string PayDisplay => $"{Pay} KM";
        public string Category { get; set; } = "";
        public string EmployerName { get; set; } = "";
        public string ImagePath { get; set; }

    }
}