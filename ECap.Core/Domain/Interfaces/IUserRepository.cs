using ECap.Core.Entities;
using ECap.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.Core.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<Tuple<int, User?, string?, string?>> ValidateLoginAsync(User entity);
        Task<int> CheckAdminAsync(int id);
        Task<string> GetUserLangAsync(int id);
        Task<List<ManageUserDTO>> GetAllManageUserAsync();
        Task<UserInfoDTO> GetManageUserDetailAsync(int id);
        Task<int> DeleteUserAsync(int id);
        Task<List<ManageUserDTO>> GetAllUserAsync();
        Task<bool> MarkTestComplete(int purchaseId, int userId);
        Task ChangePassword(string password, int userId);
    }
}
