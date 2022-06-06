using DAL.Entities;
using DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Data.Repositories
{
    public class DeliveryManRepository : GenericRepository<DeliveryMan>, IDeliveryManRepository
    {
        protected string connectionString;
        protected IDbTransaction _dbTransaction;

        public DeliveryManRepository(Context context): base(context)
        {
            connectionString = context.Database.GetDbConnection().ConnectionString;
            _dbTransaction = (IDbTransaction)context.Database.CurrentTransaction;
        }

        private DeliveryMan MapToDeliveryMan(SqlDataReader reader)
        {
            return new DeliveryMan()
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Phone = (string)reader["Phone"],
            };
        }

        public override async Task<IEnumerable<DeliveryMan>> GetAsync()
        {
            using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllDeliveryMen", _sqlConnection, (SqlTransaction)_dbTransaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<DeliveryMan>();
                    await _sqlConnection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDeliveryMan(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public override async Task<DeliveryMan> GetByIdAsync(int id)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetDeliveryMan", _sqlConnection, (SqlTransaction)_dbTransaction))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    DeliveryMan response = null;
                    await _sqlConnection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToDeliveryMan(reader);
                        }
                    }

                    return response;
                }
            }
        }

        public override async Task InsertAsync(DeliveryMan deliveryMan)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddDeliveryMan", _sqlConnection, (SqlTransaction)_dbTransaction))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@firstName", deliveryMan.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", deliveryMan.LastName));
                    cmd.Parameters.Add(new SqlParameter("@phone", deliveryMan.Phone));
                    await _sqlConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public override async Task DeleteAsync(int id)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteDeliveryMan", _sqlConnection, (SqlTransaction)_dbTransaction))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    await _sqlConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public override async Task UpdateAsync(DeliveryMan deliveryMan)
        {
            using (SqlConnection _sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateDeliveryMan", _sqlConnection, (SqlTransaction)_dbTransaction))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", deliveryMan.Id));
                    cmd.Parameters.Add(new SqlParameter("@firstName", deliveryMan.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", deliveryMan.LastName));
                    cmd.Parameters.Add(new SqlParameter("@phone", deliveryMan.Phone));
                    await _sqlConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}
