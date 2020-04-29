using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Dtos
{
    public class AuthDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength =4, ErrorMessage = "Password must have at least 4 characters")]
        public string Password { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Counntry { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        // we set the created and last active when user registered
        public AuthDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
