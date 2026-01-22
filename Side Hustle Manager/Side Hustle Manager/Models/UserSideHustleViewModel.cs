using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Side_Hustle_Manager.Models
{
    public class UserSideHustleViewModel
    {
        public int ApplicationId { get; set; }

        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public string EmployerUsername { get; set; }

        // ORIGINALNI STATUS (iz baze)
        public string Status { get; set; }

        // STATUS ZA PRIKAZ
        public string StatusDisplay =>
            Status switch
            {
                "Accepted" => "Prihvaćen",
                "Rejected" => "Odbijen",
                "Pending" => "Na čekanju",
                _ => "Nepoznato"
            };

        public Color StatusColor =>
            Status switch
            {
                "Accepted" => Colors.Green,
                "Rejected" => Colors.Red,
                "Pending" => Colors.Goldenrod,
                _ => Colors.Gray
            };

        public DateTime AppliedAt { get; set; }
        public decimal Pay { get; set; }
        public string PayDisplay => $"{Pay} KM";
    }
}
