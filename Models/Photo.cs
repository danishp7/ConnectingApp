using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }

        // for display picture selection
        public bool IsMain { get; set; }

        // to tell relationship of user and photo to efcore
        public User User { get; set; } // for eager loading
        public int UserId { get; set; } // for cascading
    }
}