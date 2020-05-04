using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Counntry { get; set; }
        public ICollection<Photo> Photos { get; set; }

        // for like entity user must have both the liker collection and likee collection
        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }

        // for msg entity
        // relation is
        // 1 user can send * msgs to 1 user
        // * msgs can be recieved by 1 user
        // so * msg * users
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

    }
}
