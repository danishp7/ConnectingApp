using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Helpers
{
    public class UserParams
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

        // to not get the user in list which is logged in
        public int UserId { get; set; }

        // to set the gender filter
        public string Gender { get; set; }

        // for age filter
        public int MinAge { get; set; } = 7;
        public int MaxAge { get; set; } = 99;

        // for sorting
        public string OrderBy { get; set; }
    }
}
