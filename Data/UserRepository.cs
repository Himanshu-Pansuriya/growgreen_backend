using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
        public class UserRepository
        {
            private readonly IConfiguration _configuration;

        #region Constructor
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region ConnectionString
        private string GetConnectionString()
            {
                return _configuration.GetConnectionString("ConnectionString");
            }
        #endregion

        #region GetUser
        public List<UserModel> GetAllUsers()
            {
                string connectionString = GetConnectionString();
                List<UserModel> users = new List<UserModel>();

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "PR_User_SelectAll";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel user = new UserModel
                                {
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Role = reader.GetString(reader.GetOrdinal("Role")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    ModifiedAt = reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                return users;
            }
        #endregion

        #region GetUserByID
        public UserModel GetUserByID(int UserID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_SelectByID";
                    command.Parameters.AddWithValue("@UserID", UserID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                ModifiedAt = reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Insert
        public bool Insert(UserModel user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting user: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Update
        public bool Update(UserModel user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int userID)
            {
                string connectionString = GetConnectionString();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("PR_User_Delete", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting user: {ex.Message}");
                    return false;
                }
            }

        #endregion

        #region UserDropDown
        public List<UserDropDownModel> GetUserForDropDown()
        {
            string connectionString = GetConnectionString();
            List<UserDropDownModel> udm = new List<UserDropDownModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "PR_User_DropDown";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDropDownModel ud = new UserDropDownModel
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            };
                            udm.Add(ud);
                        }
                    }
                }
            }
            return udm;
        }

        #endregion

        #region UserAuth
        public UserModel UserAuth(String email, String password, String role)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_User_Auth"; 
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel 
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
