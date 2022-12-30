using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ECap.Core.Domain.Interfaces
{
    public interface IDbService
    {
        Task<T> GetAsync<T>(string command, object parms, CommandType commandType = CommandType.Text);
        Task<List<T>> GetAll<T>(string command, object parms, CommandType commandType = CommandType.Text);
        Task<int> EditData(string command, object parms);
        Task <object> GetQueryMultipleAsync(string command, object parms, CommandType commandType = CommandType.Text);
        Task<int> DeleteUserAsync(string command, object parms, CommandType commandType = CommandType.Text);
    }
}
