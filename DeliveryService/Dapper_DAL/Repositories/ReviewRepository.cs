using Dapper_DAL.Interfaces;
using System.Text;
using System.Data;
using Dapper_DAL.Entites;
using System.Reflection;
using System.ComponentModel;
using Dapper_DAL.Pagination;
using Dapper_DAL.Parameters;
using System.Data.SqlClient;
using Dapper;

//TODO: pagination
namespace Dapper_DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        protected SqlConnection _sqlConnection;
        protected SqlTransaction _dbTransaction;
        private readonly string _tableName;

        public ReviewRepository(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _tableName = "Review";
        }


        public Task<PagedList<Review>> GetAsync(PaginationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Review>> GetAsync()
        {
            return await _sqlConnection.QueryAsync<Review>($"Select * FROM {_tableName}",
                transaction: _dbTransaction);
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            var result = await _sqlConnection.QuerySingleOrDefaultAsync<Review>($"SELECT * FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _sqlConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
        }

        public async Task InsertAsync(Review t)
        {
            var query = GenerateInsertQuery();
            await _sqlConnection.ExecuteAsync(query, param: t, transaction: _dbTransaction);
        }

        public async Task UpdateAsync(Review t)
        {
            var query = GenerateUpdateQuery();
            await _sqlConnection.ExecuteAsync(query, param: t, transaction: _dbTransaction);
        }

        //Model properties
        private IEnumerable<PropertyInfo> GetProperties => typeof(Review).GetProperties();
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return listOfProperties.Where(p =>
            {
                var attributes = p.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore";
            }).Select(p => p.Name).ToList();
        }

        //Generate Update Query
        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);
            properties.Remove("Customer");
            properties.Remove("Shop");
            properties.ForEach(p =>
            {
                if (!p.Equals("Id"))
                    updateQuery.Append($"{p}=@{p},");
            });
            updateQuery.Remove(updateQuery.Length - 1, 1); //Remove last coma
            updateQuery.Append(" WHERE Id=@Id");
            return updateQuery.ToString();
        }

        //Generate Insert Query
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} (");
            var properties = GenerateListOfProperties(GetProperties);
            //If Id is autoincremented
            properties.Remove("Id");
            properties.Remove("Customer");
            properties.Remove("Shop");

            properties.ForEach((p) => insertQuery.Append($"[{p}],"));
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");
            properties.ForEach((p) => insertQuery.Append($"@{p},"));
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");
            Console.WriteLine(insertQuery.ToString());
            return insertQuery.ToString();
        }
    }
}
