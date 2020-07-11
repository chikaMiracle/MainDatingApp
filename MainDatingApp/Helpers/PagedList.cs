using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainDatingApp.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);

        }


        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //to return a new instance of paged list
            //first we need to return how many items there are
            var count = await source.CountAsync();

            //Skip method is used to bypass a specify number of element in a sequence and return the remaining element
            //example if the pageNumber is (2 -1) * 5 =5 the we skip the first and take the next 5
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //we return a list of users with the count etc

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
