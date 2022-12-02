﻿using Dapper_DAL.Entites;
using Dapper_DAL.Interfaces;
using Dapper_DAL.Pagination;
using Dapper_DAL.Parameters;
using System.Data;
using System.Data.SqlClient;

//TODO: pagination
namespace Dapper_DAL.Repositories
{
    public class DeliveryManRepository: IDeliveryManRepository
    {
        protected string connectionString;
        protected SqlTransaction _dbTransaction;

        public DeliveryManRepository(string _connectionString)
        {
            connectionString = _connectionString;
        }

        private DeliveryMan MapToDeliveryMan(SqlDataReader reader)
        {
            return new DeliveryMan()
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Phone = (string)reader["Phone"],
                UserId = (string)reader["CustomerId"]
            };
        }


        public Task<PagedList<DeliveryMan>> GetAsync(PaginationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeliveryMan>> GetAsync()
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

        public async Task<DeliveryMan> GetByIdAsync(int id)
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

        public async Task InsertAsync(DeliveryMan deliveryMan)
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
                    cmd.Parameters.Add(new SqlParameter("@customerId", deliveryMan.UserId));
                    await _sqlConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task DeleteAsync(int id)
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

        public async Task UpdateAsync(DeliveryMan deliveryMan)
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
                    cmd.Parameters.Add(new SqlParameter("@customerId", deliveryMan.UserId));
                    await _sqlConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}