using Dapper;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace DAL.Data.Repositories
{
    public class ReviewRepository: IReviewRepository
    {
        protected SqlConnection _sqlConnection;
        protected IDbTransaction _dbTransaction;
        private readonly string _tableName;

        public ReviewRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _sqlConnection = sqlConnection;
            _dbTransaction = dbTransaction;
            _tableName = "Review";
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _sqlConnection.QueryAsync<Review>($"Select * FROM {_tableName}",
                transaction: _dbTransaction);
        }

        public async Task<Review> GetById(int id)
        {
            var result = await _sqlConnection.QuerySingleOrDefaultAsync<Review>($"SELECT * FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            return result;
        }

        public async Task Delete(int id)
        {
            await _sqlConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: _dbTransaction);
        }

        public async Task Insert(Review t)
        {
            var query = GenerateInsertQuery();
            await _sqlConnection.ExecuteAsync(query, param: t, transaction: _dbTransaction);
        }

        public async Task Update(Review t)
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

            properties.ForEach((p) => insertQuery.Append($"[{p}],"));
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");
            properties.ForEach((p) => insertQuery.Append($"@{p},"));
            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");
            Console.WriteLine(insertQuery.ToString());
            return insertQuery.ToString();
        }
    }
}
