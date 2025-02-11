using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
using growgreen_backend.Models;

namespace growgreen_backend.Data
{
    public class PesticidesTransactionRepository
    {
        private readonly IConfiguration _configuration;

        #region Constructor
        public PesticidesTransactionRepository(IConfiguration configuration)
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

        #region GetAllPesticidesTransactions
        public List<PesticidesTransactionModel> GetAllPesticidesTransactions()
        {
            string connectionString = GetConnectionString();
            List<PesticidesTransactionModel> transactions = new List<PesticidesTransactionModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_PesticidesTransaction_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new PesticidesTransactionModel
                            {
                                PesticidesTransactionID = reader.GetInt32(reader.GetOrdinal("PesticidesTransactionID")),
                                UserName = reader.GetString(reader.GetOrdinal("Username")),
                                BuyerID = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                PesticideID = reader.GetInt32(reader.GetOrdinal("PesticideID")),
                                PesticidesName = reader.GetString(reader.GetOrdinal("PesticidesName")),
                                QuantityPurchased = reader.GetInt32(reader.GetOrdinal("QuantityPurchased")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                PaymentDetail = reader.GetString(reader.GetOrdinal("PaymentDetail"))
                            });
                        }
                    }
                }
            }
            return transactions;
        }
        #endregion

        #region GetPesticidesTransactionByID
        public PesticidesTransactionModel GetPesticidesTransactionByID(int transactionID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_PesticidesTransaction_SelectByID";
                    command.Parameters.AddWithValue("@PesticidesTransactionID", transactionID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PesticidesTransactionModel
                            {
                                PesticidesTransactionID = reader.GetInt32(reader.GetOrdinal("PesticidesTransactionID")),
                                BuyerID = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                                PesticideID = reader.GetInt32(reader.GetOrdinal("PesticideID")),
                                QuantityPurchased = reader.GetInt32(reader.GetOrdinal("QuantityPurchased")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                PaymentDetail = reader.GetString(reader.GetOrdinal("PaymentDetail"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Insert
        public bool Insert(PesticidesTransactionModel transaction)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesTransaction_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
                    cmd.Parameters.AddWithValue("@PesticideID", transaction.PesticideID);
                    cmd.Parameters.AddWithValue("@QuantityPurchased", transaction.QuantityPurchased);
                    cmd.Parameters.AddWithValue("@TotalPrice", transaction.TotalPrice);
                    cmd.Parameters.AddWithValue("@PurchaseDate", transaction.PurchaseDate);
                    cmd.Parameters.AddWithValue("@PaymentMethod",transaction.PaymentMethod);
                    cmd.Parameters.AddWithValue("@PaymentDetail", transaction.PaymentDetail);

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

        #region Update
        public bool Update(PesticidesTransactionModel transaction)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesTransaction_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesTransactionID", transaction.PesticidesTransactionID);
                    cmd.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
                    cmd.Parameters.AddWithValue("@PesticideID", transaction.PesticideID);
                    cmd.Parameters.AddWithValue("@QuantityPurchased", transaction.QuantityPurchased);
                    cmd.Parameters.AddWithValue("@TotalPrice", transaction.TotalPrice);
                    cmd.Parameters.AddWithValue("@PaymentMethod", transaction.PaymentMethod);
                    cmd.Parameters.AddWithValue("@PurchaseDate", transaction.PurchaseDate);
                    cmd.Parameters.AddWithValue("@PaymentDetail", transaction.PaymentDetail);

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

        #region Delete
        public bool Delete(int transactionID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_PesticidesTransaction_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@PesticidesTransactionID", transactionID);

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
