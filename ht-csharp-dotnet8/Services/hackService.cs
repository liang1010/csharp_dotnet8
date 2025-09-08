using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ht_csharp_dotnet8.Services
{
    public interface IhackService
    {
        Task<Response> throwex();
        void Test();
    }
    [ServiceDependencies]
    public class hackService(IRepository<Navigation> _repo, IRepository<NavigationRoles> _repoNaviRole, RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager) : IhackService
    {

        public async Task<Response> throwex()
        {
            var executed = false;
            executed = (await _repo.Find(x => x.Label == "Home")).Count > 0;
            if (!executed)
                await _repo.AddAsync(new Navigation()
                {
                    Label = $"Home",
                    Icon = "home",
                    Route = "/private",
                });

            executed = (await _repo.Find(x => x.Label == "Settings")).Count > 0;
            if (!executed)
                await _repo.AddAsync(new Navigation()
                {
                    Label = $"Settings",
                    Icon = "settings",
                    Route = "",
                    Children = new List<Navigation>()
                    {
                        new Navigation()
                        {
                            Label = $"Navigation",
                            Icon = "menu",
                            Route = "/private/maintenance/navigation",
                        },

                        new Navigation()
                        {
                            Label = $"Application Role",
                            Icon = "security",
                            Route = "/private/maintenance/application-role",
                        },

                        new Navigation()
                        {
                            Label = $"Staff",
                            Icon = "group",
                            Route = "/private/maintenance/staff",
                        }
                    }
                });

            executed = (await _repoNaviRole.GetAllAsync()).Count > 0;
            if (!executed)
            {

                await laimsPrincipalFactory();
                var navigationList = await _repo.GetAllAsync();

                List<Guid> navigationIds = navigationList.Select(x => x.Id).ToList();

                var AdminRoleId = (await _roleManager.FindByNameAsync("Admin")).Id;

                foreach (var item in navigationIds)
                {
                   await _repoNaviRole.AddAsync(new NavigationRoles()
                    {
                        RoleId = AdminRoleId,
                        NavigationId = item
                    });
                }
                Test();
            }

            return new Response();
        }


        private async Task laimsPrincipalFactory()
        {
            RegisterModel model = new RegisterModel()
            {
                Email = "hongliang7622@gmail.com",
                Password = "devpassword",
                Username = "devusername"
            };
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists == null)
            {
                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.Append(error.Description + " ");
                    }

                }
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Manager));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Supervisor))
                    await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Supervisor));
                if (!await _roleManager.RoleExistsAsync(UserRoles.Checker))
                    await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Checker));
                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new ApplicationRole(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
            }
        }

        public void Test()
        {
            string connectionString = "Data Source=chestersystem.com\\MSSQLSERVER2019,1434;Initial Catalog=chester_db;User ID=chester;Password=246612321aA!;TrustServerCertificate=True";

            var staffList = new List<aaStaffModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT StaffId, NickName, FullName, Outlet, FootRate, BodyRate, GuaranteeIncome, 
                                    Role, BankName, BankAccName, BankAccNo, PhoneNo, CheckIn, CheckOut, 
                                    Nationality, HostelName, HostelRoom, Reference, Status, ModifyBy, 
                                    LastUpdated, CommissionBase,Gender
                             FROM dbo.Staff";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        staffList.Add(new aaStaffModel
                        {
                            StaffId = reader.GetInt32(0),
                            NickName = reader["NickName"] as string,
                            FullName = reader["FullName"] as string,
                            Outlet = reader["Outlet"] as int?,
                            Gender = reader["Gender"] as int?,
                            FootRate = reader["FootRate"] as int?,
                            BodyRate = reader["BodyRate"] as int?,
                            GuaranteeIncome = reader["GuaranteeIncome"] as decimal?,
                            Role = reader["Role"] as int?,
                            BankName = reader["BankName"] as int?,
                            BankAccName = reader["BankAccName"] as string,
                            BankAccNo = reader["BankAccNo"] as string,
                            PhoneNo = reader["PhoneNo"] as string,
                            CheckIn = reader["CheckIn"] as DateTime?,
                            CheckOut = reader["CheckOut"] as DateTime?,
                            Nationality = reader["Nationality"] as int?,
                            HostelName = reader["HostelName"] as int?,
                            HostelRoom = reader["HostelRoom"] as int?,
                            Reference = reader["Reference"] as string,
                            Status = reader["Status"] as int?,
                            ModifyBy = reader["ModifyBy"] as string,
                            LastUpdated = reader["LastUpdated"] as DateTime?,
                            CommissionBase = reader["CommissionBase"] as int?,
                        });
                    }
                }
            }

            // Print result
            foreach (var s in staffList)
            {
                var props = typeof(aaStaffModel).GetProperties();
                foreach (var prop in props)
                {
                    Console.Write($"{prop.GetValue(s) ?? "NULL"} | ");
                }
                Console.WriteLine(); // New line after each staff
            }


            string connectionString1 = "Data Source=chestersystem.com\\MSSQLSERVER2019,1434;Initial Catalog=devdb;User ID=chester;Password=246612321aA!;TrustServerCertificate=True";
            //string connectionString1 = "Data Source=localhost;Initial Catalog=devdb;Persist Security Info=True;User ID=sa;Password=Password1;TrustServerCertificate=True";
            using (SqlConnection conn = new SqlConnection(connectionString1))
            {
                conn.Open();

                foreach (var s in staffList)
                {
                    string insertSql = @"
                    INSERT INTO [dbo].[Staffs]
                    ([Id],[NickName],[FullName],[Gender],[Nationality],[Remark],
                     [IsConsultant],[IsTherapist],[LastUpdatedBy],[LastUpdatedTime],[Status])
                    VALUES
                    (@Id,@NickName,@FullName,@Gender,@Nationality,@Remark,
                     @IsConsultant,@IsTherapist,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@NickName", s.NickName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FullName", s.FullName ?? (object)DBNull.Value);

                        // Example: Map Role to Gender (You might need actual Gender in your data)
                        string gender = string.Empty;
                        if (s.Gender == 94) gender = "Male";
                        if (s.Gender == 95) gender = "Female";
                        if (s.Gender == 96) gender = "";
                        cmd.Parameters.AddWithValue("@Gender", gender); // No gender info in old table
                                                                        //ComboBoxId Category    Value
                                                                        //47  NATIONALITY THAILAND
                                                                        //48  NATIONALITY CHINA
                                                                        //49  NATIONALITY INDONESIA
                                                                        //50  NATIONALITY MALAYSIA
                                                                        //51  NATIONALITY MYANMAR
                        string nation = string.Empty;
                        if (s.Nationality == 47) nation = "THAILAND";
                        if (s.Nationality == 48) nation = "CHINA";
                        if (s.Nationality == 49) nation = "INDONESIA";
                        if (s.Nationality == 50) nation = "MALAYSIA";
                        if (s.Nationality == 51) nation = "MYANMAR";
                        cmd.Parameters.AddWithValue("@Nationality", nation ?? (object)DBNull.Value);

                        // Example: Put Role into Remark column
                        cmd.Parameters.AddWithValue("@Remark", s.Reference ?? (object)DBNull.Value);
                        // Map IsConsultant / IsTherapist based on Role
                        cmd.Parameters.AddWithValue("@IsConsultant", s.Role == 45);
                        cmd.Parameters.AddWithValue("@IsTherapist", s.Role == 46);
                        cmd.Parameters.AddWithValue("@LastUpdatedBy", s.ModifyBy ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }

                    string query = @"SELECT Id
                             FROM Staffs Where NickName = @NickName and FullName =@FullName;";
                    Guid? id = Guid.NewGuid();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NickName", s.NickName);
                        cmd.Parameters.AddWithValue("@FullName", s.FullName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader["Id"] as Guid?;

                            }
                        }
                    }

                    string insertSql1 = @"
                         INSERT INTO [dbo].[StaffAccommodations]
                         ([Id],[StaffId]
                ,[HostelName]
                ,[HostelRoom]
                ,[LastUpdatedBy]
                ,[LastUpdatedTime]
                ,[Status])
                         VALUES
                         (@Id,@StaffId,@HostelName,@HostelRoom,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql1, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@StaffId", id);

                        string hostelName = string.Empty;
                        if (s.HostelName == 26) hostelName = "RIA PAHANG";
                        if (s.HostelName == 27) hostelName = "RIA SELANGOR";
                        if (s.HostelName == 28) hostelName = "KAYANGAN";


                        string hostelrOOM = string.Empty;
                        if (s.HostelRoom == 29) hostelrOOM = "--NO HOSTEL ROOM--";
                        if (s.HostelRoom == 30) hostelrOOM = "1414-A";
                        if (s.HostelRoom == 31) hostelrOOM = "1414-B";
                        if (s.HostelRoom == 32) hostelrOOM = "1414-D";
                        if (s.HostelRoom == 33) hostelrOOM = "1423-A";
                        if (s.HostelRoom == 34) hostelrOOM = "1423-H";
                        if (s.HostelRoom == 35) hostelrOOM = "2414-C";
                        if (s.HostelRoom == 36) hostelrOOM = "2414-E";
                        if (s.HostelRoom == 37) hostelrOOM = "1423-F";
                        if (s.HostelRoom == 38) hostelrOOM = "3412-HALL";
                        if (s.HostelRoom == 39) hostelrOOM = "3414-C";
                        if (s.HostelRoom == 40) hostelrOOM = "4412-A";
                        if (s.HostelRoom == 41) hostelrOOM = "4412-E";
                        if (s.HostelRoom == 42) hostelrOOM = "12408-D";
                        if (s.HostelRoom == 43) hostelrOOM = "12408-C";
                        if (s.HostelRoom == 44) hostelrOOM = "12408-A";
                        if (s.HostelRoom == 77) hostelrOOM = "12408-B";
                        if (s.HostelRoom == 78) hostelrOOM = "1423-B";
                        if (s.HostelRoom == 79) hostelrOOM = "1423-C";
                        if (s.HostelRoom == 80) hostelrOOM = "2414-A";
                        if (s.HostelRoom == 81) hostelrOOM = "2414-D";
                        if (s.HostelRoom == 82) hostelrOOM = "4412-D";
                        if (s.HostelRoom == 83) hostelrOOM = "4412-C";
                        if (s.HostelRoom == 107) hostelrOOM = "1403-1 (A)";
                        if (s.HostelRoom == 108) hostelrOOM = "1403-2(B) ";
                        if (s.HostelRoom == 109) hostelrOOM = "1403-3 (C)";
                        if (s.HostelRoom == 110) hostelrOOM = "1403-4 (D)";
                        if (s.HostelRoom == 111) hostelrOOM = "1403-5 (E)";

                        cmd.Parameters.AddWithValue("@HostelName", hostelName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@HostelRoom", hostelrOOM ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@LastUpdatedBy", "System");
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }

                    string insertSql2 = @"
                                         INSERT INTO [dbo].[StaffBanks]
                                         ([Id],[StaffId]
                           ,[BankName]
                           ,[BankAccName]
                           ,[BankAccNo]
                                ,[LastUpdatedBy]
                                ,[LastUpdatedTime]
                                ,[Status])
                                         VALUES
                                         (@Id,@StaffId,@BankName,@BankAccName,@BankAccNo,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql2, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@StaffId", id);

                        string BANKName = string.Empty;
                        if (s.BankName == 26) BANKName = "RIA PAHANG";
                        if (s.BankName == 19) BANKName = "--NO--";
                        if (s.BankName == 20) BANKName = "PB";
                        if (s.BankName == 21) BANKName = "CIMB";
                        if (s.BankName == 22) BANKName = "RHB";
                        if (s.BankName == 23) BANKName = "MAYB";
                        if (s.BankName == 24) BANKName = "HLB";
                        if (s.BankName == 84) BANKName = "ALLIANCE BANK MALAYSIA BERHAD";
                        if (s.BankName == 85) BANKName = "AM BANK";
                        if (s.BankName == 86) BANKName = "BSN BANK";
                        if (s.BankName == 87) BANKName = "PB - PETTY CASH";
                        if (s.BankName == 97) BANKName = "BANK ISLAM";
                        if (s.BankName == 98) BANKName = "AFFIN BANK";
                        if (s.BankName == 106) BANKName = "TOUCH N GO";


                        cmd.Parameters.AddWithValue("@BankName", BANKName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BankAccName", s.BankAccName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BankAccNo", s.BankAccNo ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@LastUpdatedBy", "System");
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }

                    string insertSql3 = @"
                                         INSERT INTO [dbo].[StaffContacts]
                                         ([Id],[StaffId]
                           ,[PhoneNo]
                                ,[LastUpdatedBy]
                                ,[LastUpdatedTime]
                                ,[Status])
                                         VALUES
                                         (@Id,@StaffId,@PhoneNo,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql3, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@StaffId", id);

                        cmd.Parameters.AddWithValue("@PhoneNo", s.PhoneNo ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@LastUpdatedBy", "System");
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }

                    string insertSql4 = @"
                                                   INSERT INTO [dbo].[StaffLabourTypes]
                                                   ([Id],[StaffId]
                ,[IsRate]
                ,[FootRate]
                ,[BodyRate]
                ,[IsGuaranteeIncome]
                ,[GuaranteeIncomeAmount]
                ,[IsPercentage]
                ,[PercentageRate]
                                          ,[LastUpdatedBy]
                                          ,[LastUpdatedTime]
                                          ,[Status])
                                                   VALUES
                                                   (@Id,@StaffId,@IsRate,@FootRate,@BodyRate,@IsGuaranteeIncome,@GuaranteeIncomeAmount,@IsPercentage,@PercentageRate,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql4, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@StaffId", id);


                        cmd.Parameters.AddWithValue("@IsRate", s.CommissionBase == 72);
                        decimal footrate = 0;
                        if (s.FootRate == 9) footrate = 0;
                        if (s.FootRate == 10) footrate = 15;
                        if (s.FootRate == 11) footrate = 20;
                        if (s.FootRate == 12) footrate = 25;
                        if (s.FootRate == 13) footrate = 30;
                        if (s.FootRate == 100) footrate = 5;
                        if (s.FootRate == 101) footrate = 10;
                        if (s.FootRate == 105) footrate = 18;
                        cmd.Parameters.AddWithValue("@FootRate", footrate);

                        decimal BodyRate = 0;
                        if (s.BodyRate == 14) BodyRate = 0;
                        if (s.BodyRate == 15) BodyRate = 20;
                        if (s.BodyRate == 16) BodyRate = 25;
                        if (s.BodyRate == 17) BodyRate = 30;
                        if (s.BodyRate == 18) BodyRate = 35;
                        if (s.BodyRate == 76) BodyRate = 28;
                        if (s.BodyRate == 99) BodyRate = 5;
                        if (s.BodyRate == 103) BodyRate = 15;
                        if (s.BodyRate == 104) BodyRate = 18;
                        cmd.Parameters.AddWithValue("@BodyRate", BodyRate);

                        cmd.Parameters.AddWithValue("@IsGuaranteeIncome", s.GuaranteeIncome > 0);
                        cmd.Parameters.AddWithValue("@GuaranteeIncomeAmount", s.GuaranteeIncome);

                        cmd.Parameters.AddWithValue("@IsPercentage", s.CommissionBase != 72);

                        decimal PercentageRate = 0;
                        if (s.CommissionBase == 72) PercentageRate = 0;
                        if (s.CommissionBase == 73) PercentageRate = 20;
                        if (s.CommissionBase == 74) PercentageRate = 30;
                        cmd.Parameters.AddWithValue("@PercentageRate", PercentageRate);


                        cmd.Parameters.AddWithValue("@LastUpdatedBy", "System");
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }


                    string insertSql5 = @"
                                         INSERT INTO [dbo].[StaffStatuss]
                                         ([Id],[StaffId]
                           ,[CheckIn],[CheckOut]
                                ,[LastUpdatedBy]
                                ,[LastUpdatedTime]
                                ,[Status])
                                         VALUES
                                         (@Id,@StaffId,@CheckIn,@CheckOut,@LastUpdatedBy,@LastUpdatedTime,@Status)";

                    using (SqlCommand cmd = new SqlCommand(insertSql5, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                        cmd.Parameters.AddWithValue("@StaffId", id);

                        cmd.Parameters.AddWithValue("@CheckIn", s.CheckIn ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CheckOut", s.CheckOut ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@LastUpdatedBy", "System");
                        cmd.Parameters.AddWithValue("@LastUpdatedTime", s.LastUpdated ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@Status", s.Status);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Console.WriteLine("✅ Data migration completed.");
        }
    }
    public class aaStaffModel
    {
        public int StaffId { get; set; }
        public string NickName { get; set; }
        public string FullName { get; set; }
        public int? Outlet { get; set; }
        public decimal? FootRate { get; set; }
        public decimal? BodyRate { get; set; }
        public decimal? GuaranteeIncome { get; set; }
        public int? Role { get; set; }
        public int? BankName { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Nationality { get; set; }
        public int? HostelName { get; set; }
        public int? HostelRoom { get; set; }
        public string Reference { get; set; }
        public int? Status { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? CommissionBase { get; set; }
        public int? Gender { get; set; }
    }

}
