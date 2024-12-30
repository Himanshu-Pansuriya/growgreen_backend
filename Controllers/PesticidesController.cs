using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PesticideController : ControllerBase
    {
        private readonly PesticideRepository _pesticideRepository;

        #region PesticideConstructor
        public PesticideController(PesticideRepository pesticideRepository)
        {
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
        public IActionResult CreatePesticide([FromBody] PesticidesModel pesticide)
        {
            if (pesticide == null)
                return BadRequest("Pesticide object cannot be null.");

            try
            {
                bool isInserted = _pesticideRepository.Insert(pesticide);

                if (isInserted)
                    return CreatedAtAction(nameof(GetPesticideByID), new { id = pesticide.PesticideID }, pesticide);

                return StatusCode(500, "An error occurred while creating the pesticide.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdatePesticide
        [HttpPut("{id}")]
        public IActionResult UpdatePesticide(int id, [FromBody] PesticidesModel pesticide)
        {
            if (pesticide == null || pesticide.PesticideID != id)
                return BadRequest("Invalid pesticide data.");

            try
            {
                bool isUpdated = _pesticideRepository.Update(pesticide);

                if (isUpdated)
                    return Ok(new { Message = "Pesticide updated successfully!" });

                return StatusCode(500, "An error occurred while updating the pesticide.");
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
