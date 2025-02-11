using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PesticidesConfirmController : ControllerBase
    {
        private readonly PesticidesConfirmRepository _pesticidesConfirmRepository;

        #region Constructor
        public PesticidesConfirmController(PesticidesConfirmRepository pesticidesConfirmRepository)
        {
            _pesticidesConfirmRepository = pesticidesConfirmRepository;
        }
        #endregion

        #region GetAllConfirmations
        [HttpGet]
        public IActionResult GetAllConfirmations()
        {
            try
            {
                List<PesticidesConfirmModel> confirmations = _pesticidesConfirmRepository.GetAllConfirmations();
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetConfirmationByID
        [HttpGet("{id}")]
        public IActionResult GetConfirmationByID(int id)
        {
            try
            {
                var confirmation = _pesticidesConfirmRepository.GetConfirmationByID(id);

                if (confirmation == null)
                    return NotFound($"Confirmation with ID {id} not found.");

                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region InsertConfirmation
        [HttpPost]
        public IActionResult InsertConfirmation([FromBody] PesticidesConfirmModel confirmation)
        {
            if (confirmation == null)
                return BadRequest("Confirmation object cannot be null.");

            try
            {
                bool isInserted = _pesticidesConfirmRepository.Insert(confirmation);

                if (isInserted)
                    return CreatedAtAction(nameof(GetConfirmationByID), new { id = confirmation.PesticidesConfirmID }, confirmation);

                return StatusCode(500, "An error occurred while creating the confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateConfirmation
        [HttpPut("{id}")]
        public IActionResult UpdateConfirmation(int id, [FromBody] PesticidesConfirmModel confirmation)
        {
            if (confirmation == null || confirmation.PesticidesConfirmID != id)
                return BadRequest("Invalid confirmation data.");

            try
            {
                bool isUpdated = _pesticidesConfirmRepository.Update(confirmation);

                if (isUpdated)
                    return Ok(new { Message = "Confirmation updated successfully!" });

                return StatusCode(500, "An error occurred while updating the confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteConfirmation
        [HttpDelete("{id}")]
        public IActionResult DeleteConfirmation(int id)
        {
            try
            {
                bool isDeleted = _pesticidesConfirmRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Confirmation deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
