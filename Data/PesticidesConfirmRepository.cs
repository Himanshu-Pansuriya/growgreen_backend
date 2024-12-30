using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class PesticidesConfirmRepository
    {
        private readonly IConfiguration _configuration;

        #region Constructor
        public PesticidesConfirmRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region GetConnectionString
        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }
        #endregion

        #region GetAllConfirmations
        public List<PesticidesConfirmModel> GetAllConfirmations()
        {
            string connectionString = GetConnectionString();
            List<PesticidesConfirmModel> confirmations = new List<PesticidesConfirmModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_PesticidesConfirm_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            confirmations.Add(new PesticidesConfirmModel
                            {
                                PesticidesConfirmID = reader.GetInt32(reader.GetOrdinal("PesticidesConfirmID")),
                                PesticidesTransactionID = reader.GetInt32(reader.GetOrdinal("PesticidesTransactionID")),
                                SalesmanID = reader.GetInt32(reader.GetOrdinal("SalesmanID")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            });
                        }
                    }
                }
            }
            return confirmations;
        }
        #endregion

        #region GetConfirmationByID
        public PesticidesConfirmModel GetConfirmationByID(int confirmationID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_PesticidesConfirm_SelectByID";
                    command.Parameters.AddWithValue("@PesticidesConfirmID", confirmationID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PesticidesConfirmModel
                            {
                                PesticidesConfirmID = reader.GetInt32(reader.GetOrdinal("PesticidesConfirmID")),
                                PesticidesTransactionID = reader.GetInt32(reader.GetOrdinal("PesticidesTransactionID")),
                                SalesmanID = reader.GetInt32(reader.GetOrdinal("SalesmanID")),
                                Status = reader.GetString(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region InsertConfirmation
        public bool Insert(PesticidesConfirmModel confirmation)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesConfirm_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesTransactionID", confirmation.PesticidesTransactionID);
                    cmd.Parameters.AddWithValue("@SalesmanID", confirmation.SalesmanID);
                    cmd.Parameters.AddWithValue("@Status", confirmation.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting confirmation: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region UpdateConfirmation
        public bool Update(PesticidesConfirmModel confirmation)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesConfirm_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesConfirmID", confirmation.PesticidesConfirmID);
                    cmd.Parameters.AddWithValue("@PesticidesTransactionID", confirmation.PesticidesTransactionID);
                    cmd.Parameters.AddWithValue("@SalesmanID", confirmation.SalesmanID);
                    cmd.Parameters.AddWithValue("@Status", confirmation.Status);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating confirmation: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region DeleteConfirmation
        public bool Delete(int confirmationID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesConfirm_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesConfirmID", confirmationID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting confirmation: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
