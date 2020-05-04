using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Dtos
{
    public class MessageCreatingDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        // we set the messagesent date to now whenever message will send
        public MessageCreatingDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}
