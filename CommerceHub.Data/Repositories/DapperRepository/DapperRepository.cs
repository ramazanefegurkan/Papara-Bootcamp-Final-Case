using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Repositories.DapperRepository
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IDbConnection _dbConnection;

        public DapperRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            return await _dbConnection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await _dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
