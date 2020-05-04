using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Dtos
{
    public class MessageToReturn
    {
        public int Id { get; set; }
        // this is follower and followee relation
        
        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        public string SenderUserName { get; set; }
        public string RecipientUserName { get; set; }

        public string SenderPhotoUrl { get; set; }
        public string RecipientPhotoUrl { get; set; }

        public string Content { get; set; } // msg content simple text
        public bool IsRead { get; set; } // if msg is read or not
        
        public DateTime? DateRead { get; set; } // date when msg read like 10 days ago etc
        public DateTime MessageSent { get; set; }
    }
}
