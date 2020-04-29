using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Helpers
{
    public class UserParams
    {
        // to set the upper limit otherwise user can set page size to 1 million and then there is no use for pagination
        private const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;

        // by default page size is 10
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
