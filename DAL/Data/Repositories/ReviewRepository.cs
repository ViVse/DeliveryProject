using Dapper;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories
{
    public class ReviewRepository: GenericRepository<Review>, IReviewRepository
    {
        protected SqlConnection _sqlConnection;
        protected IDbTransaction _dbTransaction;
        private readonly string _tableName;

        public ReviewRepository(Context context): base(context)
        {
            _sqlConnection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            _dbTransaction = (IDbTransaction)context.Database.CurrentTransaction;
            _tableName = "Review";
        }

        public override async Task<IEnumerable<Review>> GetAsync()
        {
            return await _sqlConnection.QueryAsync<Review>($"Select * FROM {_tableName}",
                transaction: _dbTransaction);
        }

        public override async Task<Review> GetByIdAsync(int id)
        {
            var result = await _sqlConnection.QuerySingleOrDefaultAsync<Review>($"SELECT * FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            return result;
        }

        public override async Task DeleteAsync(int id)
        {
            await _sqlConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
        }

        public override async Task InsertAsync(Review t)
        {
            var query = GenerateInsertQuery();
            await _sqlConnection.ExecuteAsync(query, param: t, transaction: _dbTransaction);
        }

        public override async Task UpdateAsync(Review t)
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
