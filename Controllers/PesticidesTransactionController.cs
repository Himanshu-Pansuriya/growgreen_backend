using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PesticidesTransactionController : ControllerBase
    {
        private readonly PesticidesTransactionRepository _pesticidesTransactionRepository;

        #region Constructor
        public PesticidesTransactionController(PesticidesTransactionRepository pesticidesTransactionRepository)
        {
            _pesticidesTransactionRepository = pesticidesTransactionRepository;
        }
        #endregion

        #region GetAllTransactions
        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            try
            {
                List<PesticidesTransactionModel> transactions = _pesticidesTransactionRepository.GetAllPesticidesTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetTransactionByID
        [HttpGet("{id}")]
        public IActionResult GetTransactionByID(int id)
        {
            try
            {
                var transaction = _pesticidesTransactionRepository.GetPesticidesTransactionByID(id);

                if (transaction == null)
                    return NotFound($"Pesticides Transaction with ID {id} not found.");

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region CreateTransaction
        [HttpPost]
        public IActionResult CreateTransaction([FromBody] PesticidesTransactionModel transaction)
        { 
            if (transaction == null)
                return BadRequest("Transaction object cannot be null.");

            try
            {
                bool isInserted = _pesticidesTransactionRepository.Insert(transaction);

                if (isInserted)
                    return CreatedAtAction(nameof(GetTransactionByID), new { id = transaction.PesticidesTransactionID }, transaction);

                return StatusCode(500, "An error occurred while creating the transaction.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateTransaction
        [HttpPut("{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] PesticidesTransactionModel transaction)
        {
            if (transaction == null || transaction.PesticidesTransactionID != id)
                return BadRequest("Invalid transaction data.");

            try
            {
                bool isUpdated = _pesticidesTransactionRepository.Update(transaction);

                if (isUpdated)
                    return Ok(new { Message = "Transaction updated successfully!" });

                return StatusCode(500, "An error occurred while updating the transaction.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteTransaction
        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            try
            {
                bool isDeleted = _pesticidesTransactionRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Transaction deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the transaction.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
