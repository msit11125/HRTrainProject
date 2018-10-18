using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace HRTrainProject.Web
{
    public static class PageListExtensions
    {
        public static IPagedList<T> ToPageList<T>(this IEnumerable<T> data, int pageNumber, int pageSize)
        {
            var pageData = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new StaticPagedList<T>(pageData, pageNumber, pageSize, data.Count());
        }
    }
}
