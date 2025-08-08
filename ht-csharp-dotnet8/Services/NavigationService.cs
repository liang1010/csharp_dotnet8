using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Helpers;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Identity;

namespace ht_csharp_dotnet8.Services
{
    public interface INavigationService
    {
        Task<Response<PagedListingResponse<Navigation>>> GetPagedListingAsync(PageListingRequest req);
        Task<Response> EditAsync(NavigationRolesModel req);
        Task<Response> DeleteAsync(Navigation req);
        Task<Response> GetNaviRoles(Guid req);
        Task<Response<List<Navigation>>> GetNavigation();
    }
    [ServiceDependencies]
    public class NavigationService(IRepository<Navigation> _repo, IRepository<NavigationRoles> _repoNaviRoles, IUserHelper _userHelper, RoleManager<ApplicationRole> _roleManager) : INavigationService
    {
        public async Task<Response<PagedListingResponse<Navigation>>> GetPagedListingAsync(PageListingRequest req)
        {
            PagedListingResponse<Navigation> list;

            if (string.IsNullOrEmpty(req.Search))
            {
                list = await _repo.GetPagedListing(req);
            }
            else
            {
                list = await _repo.GetPagedListing(req, x => (x.Label.Contains(req.Search) || x.Route.Contains(req.Search)));
            }
            PagedListingResponse<Navigation> result = new PagedListingResponse<Navigation>()
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

            return new Response<PagedListingResponse<Navigation>>(message: "Load Successful.")
            {
                Data = result
            };
        }

        public async Task<Response> EditAsync(NavigationRolesModel req)
        {
            var navigationRoles = await _repoNaviRoles.Find(x => x.NavigationId.Equals(req.NavigationId));
            List<Guid> existingRoleIds = navigationRoles.Select(x => x.RoleId).ToList();
            List<Guid> newRoleIds = req.RoleIds;

            var toAdd = newRoleIds.Except(existingRoleIds).ToList();
            var toRemove = existingRoleIds.Except(newRoleIds).ToList();

            foreach (var roleId in toAdd)
            {
                var newNavRole = new NavigationRoles
                {
                    NavigationId = req.NavigationId,
                    RoleId = roleId
                };
                await _repoNaviRoles.AddAsync(newNavRole);
            }
            foreach (var roleId in toRemove)
            {
                var navRole = navigationRoles.FirstOrDefault(x => x.RoleId == roleId);
                if (navRole != null)
                    await _repoNaviRoles.RemoveAsync(navRole.Id);
            }


            return new Response("Edit Successful");
        }

        public async Task<Response> DeleteAsync(Navigation req)
        {
            await _repo.RemoveAsync(req.Id);
            return new Response("Delete Successful");
        }

        public async Task<Response> GetNaviRoles(Guid req)
        {
            return new Response<List<NavigationRoles>>(message: "Load Successful.")
            {
                Data = await _repoNaviRoles.Find(x => x.NavigationId == req)
            };
        }

        public async Task<Response<List<Navigation>>> GetNavigation()
        {
            var roles = _userHelper.GetRolesFromJwtToken();
            var role = await _roleManager.FindByNameAsync(roles);
            var accessibleNavigationRoles = await _repoNaviRoles.Find(x => x.RoleId == role.Id);
            var accessibleNavigationIds = accessibleNavigationRoles
                .Select(x => x.NavigationId)
                .Distinct()
                .ToList();

            var navigations = await _repo.Find(x => accessibleNavigationIds.Contains(x.Id));

            // 构建字典，方便根据 Id 查找
            var navigationDict = navigations.ToDictionary(x => x.Id);

            // 最终返回的顶级节点
            var result = new List<Navigation>();

            foreach (var nav in navigations)
            {
                if (nav.ParentId != null && navigationDict.ContainsKey(nav.ParentId.Value))
                {
                    // 有 parent，则放入 parent's children 中
                    var parent = navigationDict[nav.ParentId.Value];
                    parent.Children ??= new List<Navigation>();
                    parent.Children.Add(nav);
                }
                else
                {
                    // 没有 parent，是顶级节点
                    result.Add(nav);
                }
            }

            return new Response<List<Navigation>>(message: "Load Successful.")
            {
                Data = result
            };
        }



    }
}
