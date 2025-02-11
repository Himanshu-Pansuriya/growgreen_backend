using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CropController : ControllerBase
    {
        private readonly CropRepository _cropRepository;
        private readonly CloudinaryService _cloudinaryService;

        #region CropConstructor
        public CropController(CropRepository cropRepository,CloudinaryService cloudinaryService)
        {
            _cropRepository = cropRepository;
            _cloudinaryService = cloudinaryService;
        }
        #endregion

        #region GetAllCrops
        [HttpGet]
        public IActionResult GetAllCrops()
        {
            try
            {
                List<CropsModel> crops = _cropRepository.GetAllCrops();
                return Ok(crops);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetCropByID
        [HttpGet("{id}")]
        public IActionResult GetCropByID(int id)
        {
            try
            {
                var crop = _cropRepository.GetCropByID(id);

                if (crop == null)
                    return NotFound($"Crop with ID {id} not found.");

                return Ok(crop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region CreateCrop
        [HttpPost]
        public async Task<IActionResult> CreateCrop([FromForm] CropsUploadModel crop)
        {
            if (crop == null)
                return BadRequest("Crop object cannot be null.");

            if (crop.File == null || crop.File.Length == 0)
                return BadRequest("Image file is required.");

            try
            {
                using var stream = crop.File.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, crop.File.FileName);

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return StatusCode(500, "Image upload failed. Please try again.");
                }

                var newCrop = new CropsModel
                {
                    FarmerID = crop.FarmerID,
                    CropName = crop.CropName?.Trim(),
                    CropType = crop.CropType?.Trim(),
                    Quantity = crop.Quantity,
                    PricePer20KG = crop.PricePer20KG,
                    Description = crop.Description?.Trim(),
                    status = crop.status,
                    ContactNo = crop.ContactNo,
                    Address = crop.Address,
                    ImageUrl = imageUrl // Store the uploaded image URL
                };

                bool isInserted = _cropRepository.Insert(newCrop);

                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetCropByID), new { id = newCrop.CropID }, newCrop);
                }

                return StatusCode(500, "An error occurred while creating the crop.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateCrop
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCrop(int id, [FromForm] CropsUploadModel crop)
        {
            if (crop == null || crop.CropID != id)
                return BadRequest("Invalid crop data.");

            try
            {
                string imageUrl ;
                if (crop.File != null && crop.File.Length > 0)
                {
                    using var stream = crop.File.OpenReadStream();
                    imageUrl = await _cloudinaryService.UploadImageAsync(stream, crop.File.FileName);
                }
                else 
                {
                    var existingcrop = _cropRepository.GetCropByID(id);
                    if (existingcrop == null)
                        return NotFound("Crop not found.");
                    imageUrl = existingcrop.ImageUrl;
                }

                var updatedCrop = new CropsModel
                {
                    CropID = crop.CropID,
                    FarmerID = crop.FarmerID,
                    CropName = crop.CropName?.Trim(),
                    CropType = crop.CropType?.Trim(),
                    Quantity = crop.Quantity,
                    PricePer20KG = crop.PricePer20KG,
                    Description = crop.Description?.Trim(),
                    status = crop.status,
                    ContactNo = crop.ContactNo,
                    Address = crop.Address,
                    ImageFile = null,
                    ImageUrl = imageUrl
                };

                bool isUpdated = _cropRepository.Update(updatedCrop);

                if (isUpdated)
                    return Ok(new { Message = "Crop updated successfully!" });

                return StatusCode(500, "An error occurred while updating the crop.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteCrop
        [HttpDelete("{id}")]
        public IActionResult DeleteCrop(int id)
        {
            try
            {
                bool isDeleted = _cropRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Crop deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the crop.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
