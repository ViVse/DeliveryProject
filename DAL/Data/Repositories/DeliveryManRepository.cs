using DAL.Entities;
using DAL.Interfaces.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Data.Repositories
{
    public class DeliveryManRepository : IDeliveryManRepository
    {
        protected SqlConnection _sqlConnection;
        protected IDbTransaction _dbTransaction;
        private readonly string _tableName;

        public DeliveryManRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _sqlConnection = sqlConnection;
            _dbTransaction = dbTransaction;
            _tableName = "DeliveryMen";
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

        public async Task<IEnumerable<DeliveryMan>> GetAllAsync()
        {
            using (_sqlConnection)
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

        public async Task<DeliveryMan> GetByIdAsync(int id)
        {
            using (_sqlConnection)
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

        public async Task InsertAsync(DeliveryMan deliveryMan)
        {
            using (_sqlConnection)
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

        public async Task DeleteAsync(int id)
        {
            using (_sqlConnection)
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

        public async Task UpdateAsync(DeliveryMan deliveryMan)
        {
            using (_sqlConnection)
            {
                using (SqlCommand cmd = new SqlCommand("UpdateShop", _sqlConnection, (SqlTransaction)_dbTransaction))
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
