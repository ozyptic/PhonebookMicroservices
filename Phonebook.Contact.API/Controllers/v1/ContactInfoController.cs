using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Contact.Domain.Entities;
using Phonebook.Contact.Domain.VOs;
using Phonebook.Contact.Infrastracture.Interfaces;
using Phonebook.Shared.Models;
using System.Net;

namespace Phonebook.Contact.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;
        public ContactInfoController(IContactInfoRepository contactInfoRepository, IMapper mapper)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
        }


        [HttpPost("AddContactInfo")]
        [ProducesResponseType(typeof(ResponseDataModel<ContactInfo>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDataModel<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddContactInfo([FromBody] ContactAddInfoVO contactAddInfoVO)
        {
            try
            {
                var newContactInfo = _mapper.Map<ContactInfo>(contactAddInfoVO);
                var contactInfo = await _contactInfoRepository.AddContactInfoAsync(newContactInfo);

                if (contactInfo == null) return BadRequest();

                var result = new ResultId<string> { Id = contactInfo.Id };
                return ResponseDataModel<ResultId<string>>.Success(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllContactInfosAsList")]
        [ProducesResponseType(typeof(ResponseDataModel<List<ContactInfo>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllContactInfosAsList()
        {
            try
            {
                var list = await _contactInfoRepository.GetAllContactInfosAsListAsync();

                IList<ContactInfoVO>? infos = null;
                if (list.Any())
                {
                    infos = _mapper.Map<IList<ContactInfoVO>>(list);
                }

                Debug.Assert(infos != null, nameof(infos) + " != null");
                return ResponseDataModel<IList<ContactInfoVO>>.Success(infos, (int)HttpStatusCode.OK);
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete("DeleteContactInfo/{contactId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteContactInfo(string contactId)
        {
            try
            {
                var result = await _contactInfoRepository.DeleteContactInfoAsync(contactId);
                return result ? Ok() : BadRequest();
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("GetContactInfoById/{contactId}")]
        [ProducesResponseType(typeof(ResponseDataModel<List<ContactInfo>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetContactInfoById(string contactId)
        {
            try
            {
                var result = await _contactInfoRepository.GetContactInfoByContactIdAsync(contactId);
                return ResponseDataModel<ContactInfo>.Success(result, (int)HttpStatusCode.OK);
            }
            catch
            {
                return ResponseDataModel<Contacts>.Fail("Internal server error", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
