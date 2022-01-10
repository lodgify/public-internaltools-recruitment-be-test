using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperPanel.App.Data
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            // Sync
            // Note: works fine for the test case
            /*
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            */

            // Async
            // Workaround to make sure the operations are done async 

            Task<int> t1 = new Task<int>(() =>
            {
                var count = source.Count();
                return count;
            });
            t1.Start();

            Task<List<T>> t2 = new Task<List<T>>(() =>
            {
                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return items;
            });
            t2.Start();

            await t1;
            await t2;

            var count = t1.Result;
            var items = t2.Result;

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}