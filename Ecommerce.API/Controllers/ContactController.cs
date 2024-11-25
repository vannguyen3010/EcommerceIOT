using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Contact;
using Shared.DTO.Response;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ContactController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateContact")]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactDto contactDto)
        {
            try
            {
                if (contactDto == null)
                {
                    _logger.LogError("Contact object sent from client is null.");
                    return BadRequest("Contact object is null");
                }

                if (!ModelState.IsValid)
                {

                    _logger.LogError("Invalid brand object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var contactEntity = _mapper.Map<Contact>(contactDto);

                await _repository.Contact.CreateContactAsync(contactEntity);

                return Ok(new ApiResponse<ContactDto>
                {
                    Success = true,
                    Message = "Banner created successfully.",
                    Data = _mapper.Map<ContactDto>(contactEntity)
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong inside CreateBrand action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetAllContact")]
        public async Task<IActionResult> GetAllContact(int type = 0, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (contacts, totalCount) = await _repository.Contact.GetAllContactAsync(trackChanges: false, type: type, pageNumber, pageSize);
                _logger.LogInfo("Returned all brands from database.");

                var contactsResult = _mapper.Map<IEnumerable<ContactDto>>(contacts);
                return Ok(new
                {
                    Success = true,
                    Message = "Banner created successfully.",
                    data = new
                    {
                        totalCount,
                        contacts = contactsResult
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllBrands action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete]
        [Route("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                var contact = await _repository.Contact.GetContactByIdAsync(id, trackChanges: false);
                if (contact == null)
                {
                    _logger.LogError($"Brand with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Contact.DeleteContact(contact);
                _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteBrand action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
