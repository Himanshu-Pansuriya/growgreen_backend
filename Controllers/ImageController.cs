using growgreen_backend.Models;
using growgreen_backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace growgreen_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly GrowGreenDbContext _dbContext;

        public ImageController(CloudinaryService cloudinaryService, GrowGreenDbContext dbContext)
        {
            _cloudinaryService = cloudinaryService;
            _dbContext = dbContext;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using var stream = model.File.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, model.File.FileName);

                // Save uploaded image URL to the database
                var newImageRecord = new ImageEntity
                {
                    Url = imageUrl,
                    UploadedAt = DateTime.UtcNow
                };

                _dbContext.Images.Add(newImageRecord);
                await _dbContext.SaveChangesAsync();

                return Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine("Error uploading image: " + ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
