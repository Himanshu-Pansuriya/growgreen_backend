using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class ContactRepository
    {
        private readonly IConfiguration _configuration;

        #region Constructor
        public ContactRepository(IConfiguration configuration)
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

        #region GetAllContacts
        public List<ContactModel> GetAllContacts()
        {
            string connectionString = GetConnectionString();
            List<ContactModel> contacts = new List<ContactModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Contact_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new ContactModel
                            {
                                ContactID = reader.GetInt32(reader.GetOrdinal("ContactID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MobileNo = reader.GetString(reader.GetOrdinal("MobileNo")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                            });
                        }
                    }
                }
            }
            return contacts;
        }
        #endregion

        #region GetContactByID
        public ContactModel GetContactByID(int contactID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Contact_SelectByID";
                    command.Parameters.AddWithValue("@ContactID", contactID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ContactModel
                            {
                                ContactID = reader.GetInt32(reader.GetOrdinal("ContactID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MobileNo = reader.GetString(reader.GetOrdinal("MobileNo")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Insert
        public bool Insert(ContactModel contact)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@UserID", contact.UserID);
                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@MobileNo", contact.MobileNo);
                    cmd.Parameters.AddWithValue("@Email", contact.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", contact.Description ?? (object)DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting contact: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Update
        public bool Update(ContactModel contact)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ContactID", contact.ContactID);
                    cmd.Parameters.AddWithValue("@UserID", contact.UserID);
                    cmd.Parameters.AddWithValue("@Name", contact.Name);
                    cmd.Parameters.AddWithValue("@MobileNo", contact.MobileNo);
                    cmd.Parameters.AddWithValue("@Email", contact.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", contact.Description ?? (object)DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating contact: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int contactID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Contact_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@ContactID", contactID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting contact: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
