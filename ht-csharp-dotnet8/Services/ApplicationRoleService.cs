using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ht_csharp_dotnet8.Services
{
    public interface IApplicationRoleService
    {

        Task<Response<PagedListingResponse<ApplicationRole>>> GetPagedListingAsync(PageListingRequest req);
    }
    [ServiceDependencies]
    public class ApplicationRoleService(RoleManager<ApplicationRole> _roleManager) : IApplicationRoleService
    {
        public async Task<Response<PagedListingResponse<ApplicationRole>>> GetPagedListingAsync(PageListingRequest req)
        {
            PagedListingResponse<ApplicationRole> list;

            if (string.IsNullOrEmpty(req.Search))
            {
                list = await this.GetPagedRolesAsync(req);
            }
            else
            {
                list = await this.GetPagedRolesAsync(req, x => (x.Name.Contains(req.Search) || x.NormalizedName.Contains(req.Search)));
            }
            PagedListingResponse<ApplicationRole> result = new PagedListingResponse<ApplicationRole>()
            {
                PageNumber = list.PageNumber,
                PageSize = list.PageSize,
                TotalPages = list.TotalPages,
                TotalRecords = list.TotalRecords,
            };
            foreach (var item in list.Result)
            {
                result.Result.Add(item);
            }

            return new Response<PagedListingResponse<ApplicationRole>>(message: "Load Successful.")
            {
                Data = result
            };
        }
        public async Task<PagedListingResponse<ApplicationRole>> GetPagedRolesAsync(
        PageListingRequest filter,
        Expression<Func<ApplicationRole, bool>> expression = null)
        {
            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;

            IQueryable<ApplicationRole> query = _roleManager.Roles;

            if (expression != null)
                query = query.Where(expression);

            var totalRecords = await query.CountAsync();

            var pagedData = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedListingResponse<ApplicationRole>
            {
                Result = pagedData,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            };
        }
    }
}
