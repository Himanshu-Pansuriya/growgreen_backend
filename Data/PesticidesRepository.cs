using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class PesticideRepository
    {
        #region Fields and Constructor

        private readonly IConfiguration _configuration;

        public PesticideRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        #endregion

        #region Get All Pesticides

        public List<PesticidesModel> GetAllPesticides()
        {
            string connectionString = GetConnectionString();
            List<PesticidesModel> pesticides = new List<PesticidesModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Pesticides_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pesticides.Add(new PesticidesModel
                            {
                                PesticideID = reader.GetInt32(reader.GetOrdinal("PesticideID")),
                                PesticidesName = reader.GetString(reader.GetOrdinal("PesticidesName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                ManufacturedDate = reader.IsDBNull(reader.GetOrdinal("ManufacturedDate"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("ManufacturedDate")),
                                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
                            });
                        }
                    }
                }
            }
            return pesticides;
        }

        #endregion

        #region Get Pesticide By ID

        public PesticidesModel GetPesticideByID(int pesticideID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Pesticides_SelectByID";
                    command.Parameters.AddWithValue("@PesticideID", pesticideID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PesticidesModel
                            {
                                PesticideID = reader.GetInt32(reader.GetOrdinal("PesticideID")),
                                PesticidesName = reader.GetString(reader.GetOrdinal("PesticidesName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                ManufacturedDate = reader.IsDBNull(reader.GetOrdinal("ManufacturedDate"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("ManufacturedDate")),
                                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("ExpiryDate"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Insert Pesticide

        public bool Insert(PesticidesModel pesticide)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Pesticides_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesName", pesticide.PesticidesName);
                    cmd.Parameters.AddWithValue("@Price", pesticide.Price);
                    cmd.Parameters.AddWithValue("@Description", pesticide.Description);
                    cmd.Parameters.AddWithValue("@Stock", pesticide.Stock);
                    cmd.Parameters.AddWithValue("@ManufacturedDate", (object)pesticide.ManufacturedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExpiryDate", pesticide.ExpiryDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting pesticide: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Update Pesticide

        public bool Update(PesticidesModel pesticide)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Pesticides_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticideID", pesticide.PesticideID);
                    cmd.Parameters.AddWithValue("@PesticidesName", pesticide.PesticidesName);
                    cmd.Parameters.AddWithValue("@Price", pesticide.Price);
                    cmd.Parameters.AddWithValue("@Description", pesticide.Description);
                    cmd.Parameters.AddWithValue("@Stock", pesticide.Stock);
                    cmd.Parameters.AddWithValue("@ManufacturedDate", (object)pesticide.ManufacturedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExpiryDate", pesticide.ExpiryDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating pesticide: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Delete Pesticide

        public bool Delete(int pesticideID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Pesticides_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticideID", pesticideID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting pesticide: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
