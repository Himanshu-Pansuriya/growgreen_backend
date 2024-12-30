using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
        private readonly FAQRepository _faqRepository;

        #region Constructor
        public FAQController(FAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }
        #endregion

        #region GetAllFAQs
        [HttpGet]
        public IActionResult GetAllFAQs()
        {
            try
            {
                List<FAQModel> faqs = _faqRepository.GetAllFAQs();
                return Ok(faqs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetFAQByID
        [HttpGet("{id}")]
        public IActionResult GetFAQByID(int id)
        {
            try
            {
                var faq = _faqRepository.GetFAQByID(id);

                if (faq == null)
                    return NotFound($"FAQ with ID {id} not found.");

                return Ok(faq);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region CreateFAQ
        [HttpPost]
        public IActionResult CreateFAQ([FromBody] FAQModel faq)
        {
            if (faq == null)
                return BadRequest("FAQ object cannot be null.");

            try
            {
                bool isInserted = _faqRepository.InsertFAQ(faq);

                if (isInserted)
                    return CreatedAtAction(nameof(GetFAQByID), new { id = faq.FAQID }, faq);

                return StatusCode(500, "An error occurred while creating the FAQ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateFAQ
        [HttpPut("{id}")]
        public IActionResult UpdateFAQ(int id, [FromBody] FAQModel faq)
        {
            if (faq == null || faq.FAQID != id)
                return BadRequest("Invalid FAQ data.");

            try
            {
                bool isUpdated = _faqRepository.UpdateFAQ(faq);

                if (isUpdated)
                    return Ok(new { Message = "FAQ updated successfully!" });

                return StatusCode(500, "An error occurred while updating the FAQ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteFAQ
        [HttpDelete("{id}")]
        public IActionResult DeleteFAQ(int id)
        {
            try
            {
                bool isDeleted = _faqRepository.DeleteFAQ(id);

                if (isDeleted)
                    return Ok(new { Message = "FAQ deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the FAQ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
