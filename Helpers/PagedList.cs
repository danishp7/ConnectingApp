using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Helpers
{
    // t will be list of items i.e users or messages
    public class PagedList<T>: List<T>
    {
        // we have the list props and additional pagging props here
        // we need to send this info back to client
        // we need to add this info in the request header

        // total count of items
        public int TotalCount { get; set; }

        // page number 
        public int PageNumber { get; set; }

        // page size
        public int PageSize { get; set; }

        // total pages
        public int TotalPages { get; set; }

        // we set the items in constructor
        public PagedList(List<T> items,int totalCount, int pageNumber, int pageSize, int totalPages)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;

            // we also addrange method to add the list of items
            this.AddRange(items);
        }

        // now we create the static method that'll return the new instance of this class
        // user dont need to worry about total items and page size, we'll calculate in the body
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // source is list of items i.e users or messages
            // first we count the total items
            var totalCount = await source.CountAsync();

            // now we can calculate the total pages
            // total pages will be (total no of items / page size)
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // now we filter the items
            // if pageNumber is 1
            // pagesize is 5
            // then (1-1)*5.take(5) it means we'll skip none and take first 5 items

            // if pageNumber is 2
            // pagesize is 5
            // then (2-1)*5.take(5) it means we'll skip first 5 and take next 5 items
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // now we return the instance of class
            return new PagedList<T>(items, totalCount, pageNumber, pageSize, totalPages);
        }
    }
}
