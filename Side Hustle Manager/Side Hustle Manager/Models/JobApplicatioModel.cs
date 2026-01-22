using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class JobApplicationModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int SideHustleId { get; set; }
        public string SideHustleTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;  // za sada fake (npr. 1)

        public string Status { get; set; } = "Pending";
        // Pending | Accepted | Rejected

        public DateTime AppliedAt { get; set; }

        public DateTime? FinishedAt { get; set; }
    }
}
