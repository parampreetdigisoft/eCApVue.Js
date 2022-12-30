using ECap.Core.Domain.Interfaces;
using ECap.Core.Entities;
using ECap.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbService _dbService;
        public UserRepository(IDbService dbService)
        {
            this._dbService = dbService;
        }
        public async Task<int> AddAsync(User entity)
        {
            var result =
          await _dbService.EditData(
              "INSERT INTO public.employee (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
              entity);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var deleteEmployee = await _dbService.EditData("DELETE FROM public.employee WHERE id=@Id", new { id });
            return deleteEmployee;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            var employeeList = await _dbService.GetAll<User>("SELECT * FROM public.employee", new { });
            return employeeList;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var employeeList = await _dbService.GetAsync<User>("SELECT * FROM public.employee where id=@id", new { id });
            return employeeList;
        }

        public async Task<int> UpdateAsync(User entity)
        {
            var updateEmployee =
                await _dbService.EditData(
                    "Update public.employee SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
                    entity);
            return updateEmployee;
        }

        public async Task<Tuple<int, User?, string?, string?>> ValidateLoginAsync(User entity)
        {
            var resultAsync = await _dbService.GetQueryMultipleAsync("spUser_Login_Test", new { Login = entity.UserName, Password = entity.Password },
                       commandType: CommandType.StoredProcedure);

            var result = (Dapper.SqlMapper.GridReader)resultAsync;

            var validEntity = await result.ReadFirstOrDefaultAsync();
            if (validEntity != null)
            {
                string? has90DaysExpired = null, has365DaysExpired = null;
                // Get User Valid or not
                int code = -1;
                var data = (IDictionary<string, object>)validEntity;
                // as column name is empty check with empty key. 
                if (data != null)
                {
                    code = Convert.ToInt32(data[""]);
                }

                User userObject = null;

                if (code == 0)
                {
                    // User Entity
                    userObject = await result.ReadFirstOrDefaultAsync<User>();
                    //Get 90days Expiry
                    var has90DaysColumn = await result.ReadFirstOrDefaultAsync();
                    if (has90DaysColumn != null)
                    {
                        var daysExpired = (IDictionary<string, object>)has90DaysColumn;
                        if (daysExpired != null)
                        {
                            has90DaysExpired = Convert.ToString(daysExpired["IsExpire"]);
                        }
                    }

                    //Get 365days Expiry
                    var has365DaysColumn = await result.ReadFirstOrDefaultAsync();
                    if (has365DaysColumn != null)
                    {
                        var daysExpired = (IDictionary<string, object>)has365DaysColumn;
                        if (daysExpired != null)
                        {
                            has365DaysExpired = Convert.ToString(daysExpired["IsExpire"]);
                        }
                    }
                }

                return Tuple.Create(code, userObject, has90DaysExpired, has365DaysExpired);
            }
            return null;
        }
        public async Task<int> CheckAdminAsync(int userId)
        {
                return  await _dbService.GetAsync<int>("spCheckCompanyAdmin", new { CID = userId }, commandType: CommandType.StoredProcedure);
        }
        public async Task<string> GetUserLangAsync(int userId)
        {
            return await _dbService.GetAsync<string>("spUserGetLang", new { UserID = userId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<ManageUserDTO>> GetAllManageUserAsync()
        {
            return await _dbService.GetAll<ManageUserDTO>("spUserList", new { }, commandType: CommandType.StoredProcedure);
        }
       
        public async Task<UserInfoDTO> GetManageUserDetailAsync(int userId)
        {
            //UserInfoDTO userInfoDTOList = new UserInfoDTO();
            UserInfoDTO userInfoDTO = new UserInfoDTO();
            AddonDTO addonDTO = new AddonDTO();
            try
            {
                var resultAsync = await _dbService.GetQueryMultipleAsync("spUser_Product", new { UserId = userId }, commandType: CommandType.StoredProcedure);
                var result = (Dapper.SqlMapper.GridReader)resultAsync;

                var user = await result.ReadFirstOrDefaultAsync<ManageUserDTO>();
                var products = await result.ReadAsync<ProductDTO>();
                var addons = await result.ReadAsync<AddonDTO>();
                userInfoDTO.User = user;
                userInfoDTO.Products = products;
                userInfoDTO.Addons = addons;

                foreach (var item in products)
                {
                    item.StatusCode = await _dbService.GetAsync<int>("spUser_GetStatus", new { UserId = userId, TestId = item.PurchaseId }, commandType: CommandType.StoredProcedure);
                    item.Score = await _dbService.GetAsync<string>("spUser_UDate", new { UserId = item.PurchaseId, DName = "Score Sheet" }, commandType: CommandType.StoredProcedure);

                    // item.ProductLang = !string.IsNullOrEmpty(item.ProductLang) ?
                    // item.ProductLang.StartsWith("e") ? "English" : item.ProductLang.StartsWith("j")
                    //    ? "Japanese" : item.ProductLang.StartsWith("k") ? "Korean" : null : null;
                    if (item.ProductLang.ToLower().StartsWith("e"))
                    {
                        item.ProductLang = "English";
                    }
                    else if (item.ProductLang.ToLower().StartsWith("j"))
                    {
                        item.ProductLang = "Japanese";
                    }
                    else if (item.ProductLang.ToLower().StartsWith("k"))
                    {
                        item.ProductLang = "Korean";
                    }

                    if (userInfoDTO.Addons.Any())
                    {
                        var hasAddOns = userInfoDTO.Addons.Where(x => x.Test_Id == item.PurchaseId);

                        if (hasAddOns.Count() > 0)
                        {
                            if (hasAddOns.Count() == 2)
                            {
                                item.Full = await _dbService.GetAsync<string>("spUser_UDate", new { UserId = userId, DName = "Analysis Report" }, commandType: CommandType.StoredProcedure);
                                item.Pro = await _dbService.GetAsync<string>("spUser_UDate", new { UserId = userId, DName = "Pronunciation Profile" }, commandType: CommandType.StoredProcedure);
                            }
                        }
                        else
                        {
                            var addOnEntity = userInfoDTO.Addons.FirstOrDefault();
                            if (addOnEntity!=null && addOnEntity.UA_AddonId == 1)
                            {
                                item.Full = await _dbService.GetAsync<string>("spUser_UDate", new { UserId = userId, DName = "Analysis Report" }, commandType: CommandType.StoredProcedure);
                            }
                            else
                            {
                                item.Pro = await _dbService.GetAsync<string>("spUser_UDate", new { UserId = userId, DName = "Pronunciation Profile" }, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return userInfoDTO;
        }
        public async Task<int> DeleteUserAsync(int id)
        {
           return await _dbService.DeleteUserAsync("spUserDelete", new { UserID = id }, commandType: CommandType.StoredProcedure); ;
           
        }
        public async Task<List<ManageUserDTO>> GetAllUserAsync()
        {
            return await _dbService.GetAll<ManageUserDTO>("spNormalUser", new { }, commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> MarkTestComplete(int purchaseId, int userId)
        {
            bool isActive = false;
            isActive = await _dbService.GetAsync<bool>("select User_IsActive from [ecap].[tblUser_Product] where UP_Id=@UP_Id", new { UP_Id= purchaseId });
            var AddDBMessage = await _dbService.GetAsync<string>("SpdebMsg", new { Msg = purchaseId + "" + isActive }, commandType: CommandType.StoredProcedure);
            var sUpdateTestStatus = await _dbService.GetAsync<string>("spUpdateTestDate", new { TestId = purchaseId, IsActive = isActive, TestCompleted=1 }, commandType: CommandType.StoredProcedure);
            if (isActive)
            {
                AddDBMessage = await _dbService.GetAsync<string>("SpdebMsg", new { Msg = userId }, commandType: CommandType.StoredProcedure);
            }
            return isActive;
        }


        public async Task ChangePassword(string password, int UserId)
        {
            await _dbService.GetAsync<int>("spUserUpdatePassword", new {UserId= UserId ,Password = password }, commandType: CommandType.StoredProcedure);
        }
    }
}
