using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogRepository _blogRepository;

        #region BlogConstructor
        public BlogController(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        #endregion

        #region GetAllBlogs
        [HttpGet]
        public IActionResult GetAllBlogs()
        {
            try
            {
                List<BlogModel> blogs = _blogRepository.GetAllBlogs();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetBlogByID
        [HttpGet("{id}")]
        public IActionResult GetBlogByID(int id)
        {
            try
            {
                var blogs = _blogRepository.GetAllBlogs();
                var blog = blogs.FirstOrDefault(b => b.BlogID == id);

                if (blog == null)
                    return NotFound($"Blog with ID {id} not found.");

                return Ok(blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region InsertBlog
        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogModel blog)
        {
            if (blog == null)
                return BadRequest("Blog object cannot be null.");

            try
            {
                bool isInserted = _blogRepository.Insert(blog);

                if (isInserted)
                    return CreatedAtAction(nameof(GetBlogByID), new { id = blog.BlogID }, blog);

                return StatusCode(500, "An error occurred while creating the blog.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateBlog
        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, [FromBody] BlogModel blog)
        {
            if (blog == null || blog.BlogID != id)
                return BadRequest("Invalid blog data.");

            try
            {
                bool isUpdated = _blogRepository.Update(blog);

                if (isUpdated)
                    return Ok(new { Message = "Blog updated successfully!" });

                return StatusCode(500, "An error occurred while updating the blog.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteBlog
        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            try
            {
                bool isDeleted = _blogRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Blog deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the blog.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
