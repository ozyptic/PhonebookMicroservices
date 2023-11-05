using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Infrastracture.Interfaces;
using Phonebook.Shared.Models;
using System.Net;
using Phonebook.Contact.Domain.Dtos;

namespace Phonebook.Contact.API.Controllers.v1
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ContactsController : ControllerBase
    {

        private readonly IContactsRepository _contactsRepository;
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;
        public ContactsController(IContactsRepository contactsRepository, IContactInfoRepository contactInfoRepository, IMapper mapper)
        {
            _mapper = mapper;
            _contactsRepository = contactsRepository;
            _contactInfoRepository = contactInfoRepository;
        }

        [HttpPost("AddContact")]
        [ProducesResponseType(typeof(ResponseDataModel<Contacts>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDataModel<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddContact([FromBody] ContactAddDto model)
        {
            try
            {
                var newContact = _mapper.Map<Contacts>(model);
                var contact = await _contactsRepository.AddContactAsync(newContact);

                if (contact == null) return BadRequest();

                var result = new ResultId<string> { Id = contact.Id };
                return ResponseDataModel<ResultId<string>>.Success(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeleteContact/{contactId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteContact(string contactId)
        {
            try
            {
                var result = await _contactsRepository.DeleteContactAsync(contactId);
                return result ? Ok() : BadRequest();
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("GetContactById/{contactId}")]
        [ProducesResponseType(typeof(ResponseDataModel<ContactDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDataModel<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetContactById(string contactId)
        {
            try
            {
                var contact = await _contactsRepository.GetContactByIdAsync(contactId);
                if (contact == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<ContactDto>(contact);

                var contactInfos = await _contactInfoRepository.GetContactInfoByContactIdAsync(contact.Id);
                if (contactInfos != null)
                {
                    model.ContactInfoVOs = _mapper.Map<IList<ContactInfoDto>>(contactInfos);
                }

                return ResponseDataModel<ContactDto>.Success(model, (int)HttpStatusCode.OK);
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }


        [HttpGet("GetAllContactsAsList")]
        [ProducesResponseType(typeof(ResponseDataModel<List<Contacts>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllContactsAsList()
        {
            try
            {
                var list = await _contactsRepository.GetAllContactsAsListAsync();
                IList<ContactIXDto>? contacts = null;

                if (true)
                {
                    contacts = _mapper.Map<IList<ContactIXDto>>(list);
                }

                return ResponseDataModel<IList<ContactIXDto>>.Success(contacts, (int)HttpStatusCode.OK);
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
