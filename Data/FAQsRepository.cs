using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class FAQRepository
    {
        private readonly IConfiguration _configuration;

        #region Constructor
        public FAQRepository(IConfiguration configuration)
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

        #region GetAllFAQs
        public List<FAQModel> GetAllFAQs()
        {
            string connectionString = GetConnectionString();
            List<FAQModel> faqs = new List<FAQModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_FAQs_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            faqs.Add(new FAQModel
                            {
                                FAQID = reader.GetInt32(reader.GetOrdinal("FAQID")),
                                Question = reader.GetString(reader.GetOrdinal("Question")),
                                Answer = reader.GetString(reader.GetOrdinal("Answer"))
                            });
                        }
                    }
                }
            }
            return faqs;
        }
        #endregion

        #region GetFAQByID
        public FAQModel GetFAQByID(int faqID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_FAQs_SelectByID";
                    command.Parameters.AddWithValue("@FAQID", faqID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FAQModel
                            {
                                FAQID = reader.GetInt32(reader.GetOrdinal("FAQID")),
                                Question = reader.GetString(reader.GetOrdinal("Question")),
                                Answer = reader.GetString(reader.GetOrdinal("Answer"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region InsertFAQ
        public bool InsertFAQ(FAQModel faq)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_FAQs_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Question", faq.Question);
                    cmd.Parameters.AddWithValue("@Answer", faq.Answer);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting FAQ: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region UpdateFAQ
        public bool UpdateFAQ(FAQModel faq)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_FAQs_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@FAQID", faq.FAQID);
                    cmd.Parameters.AddWithValue("@Question", faq.Question);
                    cmd.Parameters.AddWithValue("@Answer", faq.Answer);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating FAQ: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region DeleteFAQ
        public bool DeleteFAQ(int faqID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_FAQs_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@FAQID", faqID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting FAQ: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
