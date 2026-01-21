using SQLite;

namespace Side_Hustle_Manager.Models
{
    public class AdminProfileModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Name { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactInfo { get; set; } = "";
        public string ProfileImagePath { get; set; } = "";
    }
}
