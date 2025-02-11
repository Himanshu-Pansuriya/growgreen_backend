using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PesticideController : ControllerBase
    {
        private readonly PesticideRepository _pesticideRepository;

        private readonly CloudinaryService _cloudinaryService;
        

        #region PesticideConstructor
        public PesticideController(PesticideRepository pesticideRepository, CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
            _pesticideRepository = pesticideRepository;
        }
        #endregion

        #region GetAllPesticides
        [HttpGet]
        public IActionResult GetAllPesticides()
        {
            try
            {
                List<PesticidesModel> pesticides = _pesticideRepository.GetAllPesticides();
                return Ok(pesticides);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetPesticideByID
        [HttpGet("{id}")]
        public IActionResult GetPesticideByID(int id)
        {
            try
            {
                var pesticide = _pesticideRepository.GetPesticideByID(id);

                if (pesticide == null)
                    return NotFound($"Pesticide with ID {id} not found.");

                return Ok(pesticide);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region CreatePesticide
        [HttpPost]
        public async Task<IActionResult> CreatePesticide([FromForm] PesticidesUploadModel pesticide)
        {
            if (pesticide == null)
            {
                return BadRequest("Pesticide object cannot be null.");
            }

            if (pesticide.File == null || pesticide.File.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                // Upload image to Cloudinary
                using var stream = pesticide.File.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, pesticide.File.FileName);

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return StatusCode(500, "Image upload failed. Please try again.");
                }

                // Create new pesticide record
                var newPesticide = new PesticidesModel
                {
                    PesticidesName = pesticide.PesticidesName?.Trim(),
                    Price = pesticide.Price,
                    Description = pesticide.Description?.Trim(),
                    Stock = pesticide.Stock,
                    ManufacturedDate = pesticide.ManufacturedDate,
                    ExpiryDate = pesticide.ExpiryDate,
                    ImageUrl = imageUrl
                };

                bool isInserted = _pesticideRepository.Insert(newPesticide);

                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetPesticideByID), new { id = pesticide.PesticideID }, pesticide);

                }

                return StatusCode(500, "An error occurred while creating the pesticide.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        #endregion


        #region UpdatePesticide
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePesticideAsync(int id, [FromForm] PesticidesUploadModel pesticide)
        {
            if (pesticide == null || pesticide.PesticideID != id)
                return BadRequest("Invalid pesticide data.");

            try
            {
                string imageUrl;

                // Check if a new file is uploaded
                if (pesticide.File != null && pesticide.File.Length > 0)
                {
                    // Upload the new image to Cloudinary
                    using var stream = pesticide.File.OpenReadStream();
                    imageUrl = await _cloudinaryService.UploadImageAsync(stream, pesticide.File.FileName);
                }
                else
                {
                    // Retrieve the existing image URL from the repository
                    var existingPesticide = _pesticideRepository.GetPesticideByID(id);
                    if (existingPesticide == null)
                        return NotFound("Pesticide not found.");

                    imageUrl = existingPesticide.ImageUrl; // Retain the existing URL
                }

                // Update the pesticide with the new data
                var updatedPesticide = new PesticidesModel
                {
                    PesticideID = pesticide.PesticideID,
                    PesticidesName = pesticide.PesticidesName,
                    Price = pesticide.Price,
                    Description = pesticide.Description,
                    Stock = pesticide.Stock,
                    ManufacturedDate = pesticide.ManufacturedDate,
                    ExpiryDate = pesticide.ExpiryDate,
                    ImageUrl = imageUrl  // Use the updated or existing image URL
                };

                bool isUpdated = _pesticideRepository.Update(updatedPesticide);

                if (isUpdated)
                    return Ok(new { Message = "Pesticide updated successfully!" });

                return StatusCode(500, "Failed to update pesticide.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion


        #region DeletePesticide
        [HttpDelete("{id}")]
        public IActionResult DeletePesticide(int id)
        {
            try
            {
                bool isDeleted = _pesticideRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Pesticide deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the pesticide.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
