using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Models
{
    public class Message
    {
        public int Id { get; set; }

        // this is follower and followee relation
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public User Sender { get; set; }
        public User Recipient { get; set; }

        public string Content { get; set; } // msg content simple text

        public bool IsRead { get; set; } // if msg is read or not
        public DateTime? DateRead { get; set; } // date when msg read like 10 days ago etc
        public DateTime MessageSent { get; set; }
        public bool SenderDelted { get; set; } // if sender deletes msg
        public bool RecipientDeleted { get; set; } // if reciever deletes msg

        // we'll delete the msg if both deletes from their respective msgs list
    }
}
