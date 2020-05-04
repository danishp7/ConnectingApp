using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Helpers
{
    public class MessageParams
    {
        // to set the upper limit otherwise user can set page size to 1 million and then there is no use for pagination
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        // by default page size is 10
        int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        // this userid will be used as filter with message container
        public int UserId { get; set; }

        public string MessageContainer { get; set; } = "Unread";/*this will be unread, inbox or outbox*/


        // if unread then userid will be recipient id with unread status
        // if inbox userid will be recip id but with read status
        // if outbox then userid will be sender id i.e currently logged in user
    }
}
