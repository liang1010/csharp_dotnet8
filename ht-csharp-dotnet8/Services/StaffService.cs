using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;

namespace ht_csharp_dotnet8.Services
{
    public interface IStaffService
    {
        Task<Response<PagedListingResponse<Staff>>> GetPagedListingAsync(PageListingRequest req);
        Task<Response<List<StaffModel>>> GetLazyLoad(List<Guid> ids);
    }

    [ServiceDependencies]
    public class StaffService(IRepository<Staff> _repo, IRepository<StaffAccommodation> _repoStaffAccommodation, IRepository<StaffBank> _repoStaffBank, IRepository<StaffContact> _repoStaffContact, IRepository<StaffLabourType> _repoStaffLabourType, IRepository<StaffStatus> _repoStaffStatus) : IStaffService
    {
        public async Task<Response<PagedListingResponse<Staff>>> GetPagedListingAsync(PageListingRequest req)
        {
            PagedListingResponse<Staff> list;

            if (string.IsNullOrEmpty(req.Search))
            {
                list = await _repo.GetPagedListing(req, orderBy: q => q.OrderByDescending(x => x.Status).ThenBy(x=>x.NickName));
            }
            else
            {
                list = await _repo.GetPagedListing(req, x => (x.NickName.Contains(req.Search) || x.FullName.Contains(req.Search)), orderBy: q => q.OrderByDescending(x => x.Status).ThenBy(x => x.NickName));
            }
            PagedListingResponse<Staff> result = new PagedListingResponse<Staff>()
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

            return new Response<PagedListingResponse<Staff>>(message: "Load Successful.")
            {
                Data = result
            };
        }

        public async Task<Response<List<StaffModel>>> GetLazyLoad(List<Guid> ids)
        {
            var staffBank = await _repoStaffBank.Find(x => ids.Contains(x.StaffId));
            var staffAccommodation = await _repoStaffAccommodation.Find(x => ids.Contains(x.StaffId));
            var staffContact = await _repoStaffContact.Find(x => ids.Contains(x.StaffId));
            var staffLabourType = await _repoStaffLabourType.Find(x => ids.Contains(x.StaffId));
            var staffStatus = await _repoStaffStatus.Find(x => ids.Contains(x.StaffId));

            // 转成字典，提高查找效率 O(1)
            var bankDict = staffBank.ToDictionary(x => x.StaffId, x => x);
            var accommodationDict = staffAccommodation.ToDictionary(x => x.StaffId, x => x);
            var contactDict = staffContact.ToDictionary(x => x.StaffId, x => x);
            var labourTypeDict = staffLabourType.ToDictionary(x => x.StaffId, x => x);
            var statusDict = staffStatus.ToDictionary(x => x.StaffId, x => x);

            List<StaffModel> result = new List<StaffModel>();
            foreach (var id in ids)
            {
                bankDict.TryGetValue(id, out var bank);
                accommodationDict.TryGetValue(id, out var accommodation);
                contactDict.TryGetValue(id, out var contact);
                labourTypeDict.TryGetValue(id, out var labour);
                statusDict.TryGetValue(id, out var status);
                result.Add(new StaffModel()
                {
                    Id = id,
                    BankAccName = bank?.BankAccName,
                    BankAccNo = bank?.BankAccNo,
                    BankName = bank?.BankName,
                    BodyRate = labour?.BodyRate ?? 0,
                    CheckIn = status?.CheckIn,
                    CheckOut = status?.CheckOut,
                    Email = contact?.Email,
                    FootRate = labour?.FootRate ?? 0,
                    GuaranteeIncomeAmount = labour?.GuaranteeIncomeAmount ?? 0,
                    HostelName = accommodation?.HostelName,
                    HostelRoom = accommodation?.HostelRoom,
                    IsGuaranteeIncome = labour?.IsGuaranteeIncome ?? false,
                    IsPercentage = labour?.IsPercentage ?? false,
                    IsRate = labour?.IsRate ?? false,
                    PercentageRate = labour?.PercentageRate ?? 0,
                    PhoneNo = contact?.PhoneNo,
                });
            }

            return new Response<List<StaffModel>>(message: "Load Successful.")
            {
                Data = result
            };
        }
    }
}
