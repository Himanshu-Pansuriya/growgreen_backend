using growgreen_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace growgreen_backend.Data
{
    public class BlogRepository
    {
        #region Fields and Constructor

        private readonly IConfiguration _configuration;

        public BlogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        #endregion

        #region Get All Blogs

        public List<BlogModel> GetAllBlogs()
        {
            string connectionString = GetConnectionString();
            List<BlogModel> blogs = new List<BlogModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Blog_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            blogs.Add(new BlogModel
                            {
                                BlogID = reader.GetInt32(reader.GetOrdinal("BlogID")),
                                AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Detail = reader.GetString(reader.GetOrdinal("Detail")),
                                PublishedDate = reader.GetDateTime(reader.GetOrdinal("PublishedDate"))
                            });
                        }
                    }
                }
            }
            return blogs;
        }

        #endregion

        #region Get Blog By ID

        public BlogModel GetBlogByID(int blogID)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Blog_SelectByID";
                    command.Parameters.AddWithValue("@BlogID", blogID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BlogModel
                            {
                                BlogID = reader.GetInt32(reader.GetOrdinal("BlogID")),
                                AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Detail = reader.GetString(reader.GetOrdinal("Detail")),
                                PublishedDate = reader.GetDateTime(reader.GetOrdinal("PublishedDate"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Insert Blog

        public bool Insert(BlogModel blog)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Blog_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@AdminID", blog.AdminID);
                    cmd.Parameters.AddWithValue("@Title", blog.Title);
                    cmd.Parameters.AddWithValue("@Detail", blog.Detail);
                    cmd.Parameters.AddWithValue("@PublishedDate", blog.PublishedDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting blog: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Update Blog

        public bool Update(BlogModel blog)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Blog_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@BlogID", blog.BlogID);
                    cmd.Parameters.AddWithValue("@AdminID", blog.AdminID);
                    cmd.Parameters.AddWithValue("@Title", blog.Title);
                    cmd.Parameters.AddWithValue("@Detail", blog.Detail);
                    cmd.Parameters.AddWithValue("@PublishedDate", blog.PublishedDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating blog: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Delete Blog

        public bool Delete(int blogID)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_Blog_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@BlogID", blogID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting blog: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
