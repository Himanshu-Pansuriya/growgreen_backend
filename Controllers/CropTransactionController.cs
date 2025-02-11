using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CropsTransactionController : ControllerBase
    {
        private readonly CropsTransactionRepository _cropsTransactionRepository;

        #region CropsTransactionConstructor
        public CropsTransactionController(CropsTransactionRepository cropsTransactionRepository)
        {
            _cropsTransactionRepository = cropsTransactionRepository;
        }
        #endregion

        #region GetAllTransactions
        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            try
            {
                List<CropsTransactionModel> transactions = _cropsTransactionRepository.GetAllTransactions();
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
                var transaction = _cropsTransactionRepository.GetTransactionByID(id);

                if (transaction == null)
                    return NotFound($"Transaction with ID {id} not found.");

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region InsertTransaction
        [HttpPost]
        public IActionResult InsertTransaction([FromBody] CropsTransactionModel transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction object cannot be null.");

            try
            {
                bool isInserted = _cropsTransactionRepository.Insert(transaction);

                if (isInserted)
                    return CreatedAtAction(nameof(GetTransactionByID), new { id = transaction.CropsTransactionID }, transaction);

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
        public IActionResult UpdateTransaction(int id, [FromBody] CropsTransactionModel transaction)
        {
            if (transaction == null || transaction.CropsTransactionID != id)
                return BadRequest("Invalid transaction data.");

            try
            {
                bool isUpdated = _cropsTransactionRepository.Update(transaction);

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
                bool isDeleted = _cropsTransactionRepository.Delete(id);

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
