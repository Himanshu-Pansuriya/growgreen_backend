using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CropController : ControllerBase
    {
        private readonly CropRepository _cropRepository;

        #region CropConstructor
        public CropController(CropRepository cropRepository)
        {
            _cropRepository = cropRepository;
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
        public IActionResult CreateCrop([FromBody] CropsModel crop)
        {
            if (crop == null)
                return BadRequest("Crop object cannot be null.");

            try
            {
                bool isInserted = _cropRepository.Insert(crop);

                if (isInserted)
                    return CreatedAtAction(nameof(GetCropByID), new { id = crop.CropID }, crop);

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
        public IActionResult UpdateCrop(int id, [FromBody] CropsModel crop)
        {
            if (crop == null || crop.CropID != id)
                return BadRequest("Invalid crop data.");

            try
            {
                bool isUpdated = _cropRepository.Update(crop);

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
