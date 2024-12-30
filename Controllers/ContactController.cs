using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ContactRepository _contactRepository;

        #region ContactConstructor
        public ContactController(ContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        #endregion

        #region GetAllContact
        [HttpGet]
        public IActionResult GetAllContacts()
        {
            try
            {
                List<ContactModel> contacts = _contactRepository.GetAllContacts();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region GetContactByID
        [HttpGet("{id}")]
        public IActionResult GetContactByID(int id)
        {
            try
            {
                var contacts = _contactRepository.GetAllContacts();
                var contact = contacts.FirstOrDefault(c => c.ContactID == id);

                if (contact == null)
                    return NotFound($"Contact with ID {id} not found.");

                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region InsertContact
        [HttpPost]
        public IActionResult CreateContact([FromBody] ContactModel contact)
        {
            if (contact == null)
                return BadRequest("Contact object cannot be null.");

            try
            {
                bool isInserted = _contactRepository.Insert(contact);

                if (isInserted)
                    return CreatedAtAction(nameof(GetContactByID), new { id = contact.ContactID }, contact);

                return StatusCode(500, "An error occurred while creating the contact.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region UpdateContact
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] ContactModel contact)
        {
            if (contact == null || contact.ContactID != id)
                return BadRequest("Invalid contact data.");

            try
            {
                bool isUpdated = _contactRepository.Update(contact);

                if (isUpdated)
                    return Ok(new { Message = "Contact updated successfully!" });

                return StatusCode(500, "An error occurred while updating the contact.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region DeleteContact
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            try
            {
                bool isDeleted = _contactRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { Message = "Contact deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the contact.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
