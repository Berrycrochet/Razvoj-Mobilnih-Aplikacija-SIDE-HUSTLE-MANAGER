using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class AdminProfileModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Name { get; set; } = "";     // Ime admina
        public string CompanyName { get; set; } = "";    // Naziv firme
        public string Address { get; set; } = "";         // Adresa
        public string ContactInfo { get; set; } = "";    // Telefon/email
        public string ProfileImagePath { get; set; } = "";   // Profilna slika
    }
}
