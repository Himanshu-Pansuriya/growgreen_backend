using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class CropsTransactionRepository
    {
        private readonly IConfiguration _configuration;

        #region Constructor
        public CropsTransactionRepository(IConfiguration configuration)
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

        #region GetAllTransactions
        public List<CropsTransactionModel> GetAllTransactions()
        {
            string connectionString = GetConnectionString();
            List<CropsTransactionModel> transactions = new List<CropsTransactionModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_CropsTransaction_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new CropsTransactionModel
                            {
                                CropsTransactionID = reader.GetInt32(reader.GetOrdinal("CropsTransactionID")),
                                BuyerID = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                CropID = reader.GetInt32(reader.GetOrdinal("CropID")),
                                QuantityPurchased = reader.GetDecimal(reader.GetOrdinal("QuantityPurchased")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate"))
                            });
                        }
                    }
                }
            }
            return transactions;
        }
        #endregion

        #region GetTransactionByID
        public CropsTransactionModel GetTransactionByID(int transactionID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_CropsTransaction_SelectByID";
                    command.Parameters.AddWithValue("@CropsTransactionID", transactionID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CropsTransactionModel
                            {
                                CropsTransactionID = reader.GetInt32(reader.GetOrdinal("CropsTransactionID")),
                                BuyerID = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                CropID = reader.GetInt32(reader.GetOrdinal("CropID")),
                                QuantityPurchased = reader.GetDecimal(reader.GetOrdinal("QuantityPurchased")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region InsertTransaction
        public bool Insert(CropsTransactionModel transaction)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_CropsTransaction_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
                    cmd.Parameters.AddWithValue("@CropID", transaction.CropID);
                    cmd.Parameters.AddWithValue("@QuantityPurchased", transaction.QuantityPurchased);
                    cmd.Parameters.AddWithValue("@TotalPrice", transaction.TotalPrice);
                    cmd.Parameters.AddWithValue("@PurchaseDate", transaction.PurchaseDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting transaction: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region UpdateTransaction
        public bool Update(CropsTransactionModel transaction)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_CropsTransaction_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CropsTransactionID", transaction.CropsTransactionID);
                    cmd.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
                    cmd.Parameters.AddWithValue("@CropID", transaction.CropID);
                    cmd.Parameters.AddWithValue("@QuantityPurchased", transaction.QuantityPurchased);
                    cmd.Parameters.AddWithValue("@TotalPrice", transaction.TotalPrice);
                    cmd.Parameters.AddWithValue("@PurchaseDate", transaction.PurchaseDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating transaction: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region DeleteTransaction
        public bool Delete(int transactionID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_CropsTransaction_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@CropsTransactionID", transactionID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
