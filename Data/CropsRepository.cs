using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class CropRepository
    {
        #region Fields and Constructor

        private readonly IConfiguration _configuration;

        public CropRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        #endregion

        #region GetAllOperations

        public List<CropsModel> GetAllCrops()
        {
            string connectionString = GetConnectionString();
            List<CropsModel> crops = new List<CropsModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Crops_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            crops.Add(new CropsModel
                            {
                                CropID = reader.GetInt32(reader.GetOrdinal("CropID")),
                                FarmerID = reader.GetInt32(reader.GetOrdinal("FarmerID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                CropName = reader.GetString(reader.GetOrdinal("CropName")),
                                CropType = reader.IsDBNull(reader.GetOrdinal("CropType")) ? null : reader.GetString(reader.GetOrdinal("CropType")),
                                Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                PricePer20KG = reader.GetDecimal(reader.GetOrdinal("PricePer20KG")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                status = reader.GetString(reader.GetOrdinal("Status")),
                                ContactNo = reader.GetString(reader.GetOrdinal("ContactNo")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            });

                        }
                    }
                }
            }
            return crops;
        }
        #endregion

        #region GetByID
        public CropsModel GetCropByID(int CropID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Crops_SelectByID";
                    command.Parameters.AddWithValue("@CropID", CropID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CropsModel
                            {
                                CropID = reader.GetInt32(reader.GetOrdinal("CropID")),
                                FarmerID = reader.GetInt32(reader.GetOrdinal("FarmerID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                CropName = reader.GetString(reader.GetOrdinal("CropName")),
                                CropType = reader.IsDBNull(reader.GetOrdinal("CropType")) ? null : reader.GetString(reader.GetOrdinal("CropType")),
                                Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                PricePer20KG = reader.GetDecimal(reader.GetOrdinal("PricePer20KG")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                status = reader.GetString(reader.GetOrdinal("Status")),
                                ContactNo = reader.GetString(reader.GetOrdinal("ContactNo")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Insert Operation

        public bool Insert(CropsModel crop)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Crops_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@FarmerID", crop.FarmerID);
                    cmd.Parameters.AddWithValue("@CropName", crop.CropName);
                    cmd.Parameters.AddWithValue("@CropType", crop.CropType);
                    cmd.Parameters.AddWithValue("@Quantity", crop.Quantity.HasValue ? (object)crop.Quantity.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PricePer20KG", crop.PricePer20KG);
                    cmd.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(crop.Description) ? crop.Description : DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", crop.status);
                    cmd.Parameters.AddWithValue("@ContactNo", crop.ContactNo);
                    cmd.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(crop.Address) ? crop.Address : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageUrl", !string.IsNullOrEmpty(crop.ImageUrl) ? crop.ImageUrl : DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting crop: {ex.Message}");
                return false;
            }
        }


        #endregion

        #region Update Operation
        public bool Update(CropsModel crop)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Crops_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CropID", crop.CropID);
                    cmd.Parameters.AddWithValue("@FarmerID", crop.FarmerID);
                    cmd.Parameters.AddWithValue("@CropName", crop.CropName);
                    cmd.Parameters.AddWithValue("@CropType", crop.CropType);
                    cmd.Parameters.AddWithValue("@Quantity", crop.Quantity.HasValue ? (object)crop.Quantity.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PricePer20KG", crop.PricePer20KG);
                    cmd.Parameters.AddWithValue("@Description", !string.IsNullOrEmpty(crop.Description) ? crop.Description : DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", crop.status);
                    cmd.Parameters.AddWithValue("@ContactNo", crop.ContactNo);
                    cmd.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(crop.Address) ? crop.Address : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageUrl", !string.IsNullOrEmpty(crop.ImageUrl) ? crop.ImageUrl : DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating crop: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Delete Operation

        public bool Delete(int cropID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Crops_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CropID", cropID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting crop: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
