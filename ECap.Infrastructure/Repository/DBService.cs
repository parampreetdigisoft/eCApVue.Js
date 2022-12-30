using Dapper;
using Microsoft.Data.SqlClient;
using ECap.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECap.Infrastructure.Repository
{
    public class DBService : IDbService
    {
        private readonly IDbConnection _db;

        public DBService(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<T> GetAsync<T>(string command, object parms, CommandType commandType)
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms, commandType: commandType).ConfigureAwait(false)).FirstOrDefault();

            return result;

        }

        public async Task<List<T>> GetAll<T>(string command, object parms, CommandType commandType)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms, commandType: commandType)).ToList();

            return result;
        }

        public async Task<int> EditData(string command, object parms)
        {
            int result;

            result = await _db.ExecuteAsync(command, parms);

            return result;
        }

        public async Task<object> GetQueryMultipleAsync(string command, object parms, CommandType commandType = CommandType.Text)
        {
            return await _db.QueryMultipleAsync(command, parms, commandType: commandType);
        }

        public async Task<int> DeleteUserAsync(string command, object parms, CommandType commandType = CommandType.Text)
        {
            return await _db.ExecuteAsync(command, parms, commandType: commandType);
        }
    }
}
