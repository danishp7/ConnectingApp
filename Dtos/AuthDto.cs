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
    }
}
