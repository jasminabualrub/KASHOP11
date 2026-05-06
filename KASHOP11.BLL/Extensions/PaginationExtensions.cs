using KASHOP11.DAL.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PagentaionResponse<T>> ToPaginationAsync<T>(this IQueryable<T>query,int page,int limit)
        {
            var totalcount = await query.CountAsync();
            var data = await query.Skip((page-1)*limit).Take(limit).ToListAsync();
            return new PagentaionResponse<T>
            {
              Data=data,
              TotalCount=totalcount,
              Page=page,
              Limit=limit,

            };
        }
    }
}
