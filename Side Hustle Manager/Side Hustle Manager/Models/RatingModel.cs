using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Models
{
    public class RatingModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string RatedByUsername { get; set; }  // tko daje ocjenu
        public string RatedToUsername { get; set; }  // tko je ocijenjen
        public int Stars { get; set; }               // 1-5
        public string Comment { get; set; }          // opcionalno
    }
}