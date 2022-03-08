using ExternalContacts.Api.Models;
using ExternalContacts.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExternalContacts.Api.Controllers
{
    [ApiController]
    [Route("/v1/contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly Random _random = new Random();

        private readonly ILogger<ContactsController> _logger;
        private readonly IExternalContactsService _externalContactsService;


        public ContactsController(IExternalContactsService externalContactsService, ILogger<ContactsController> logger)
        {
            _logger = logger;
            _externalContactsService = externalContactsService;
        }

        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            AreyouFeelingLucky();
            return _externalContactsService.GetAll();
        }

        [HttpGet]
        [Route("{email}")]
        public ActionResult<Contact> Get(string email)
        {
            AreyouFeelingLucky();

            var c = _externalContactsService.GetByEmail(email);
            if (c == null)
                return new NotFoundResult();

            return new JsonResult(c);
        }

        [HttpGet]
        [Route("{id:long}")]
        public ActionResult<Contact> Get(long id)
        {
            AreyouFeelingLucky();

            var c = _externalContactsService.GetById(id);
            if (c == null)
                return new NotFoundResult();

            return new JsonResult(c);
        }


        [HttpPut]
        [Route("{id:long}/gdpr")]
        public ActionResult<Contact> Gdpr(long id)
        {
            AreyouFeelingLucky();

            var c = _externalContactsService.GetById(id);
            if (c == null)
                return new NotFoundResult();

            _externalContactsService.Anonymize(c);

            return new JsonResult(c);
        }

        [HttpPut("gdpr-batch")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<List<Contact>> GdprBatch([FromBody] GdprPutCommand command)
        {
            AreyouFeelingLucky();

            var c = _externalContactsService.GetByIds(command.Ids);
            if (c == null || !c.Any())
                return NotFound();

            _externalContactsService.AnonymizeList(c);

            return Ok(c);
        }

        private void AreyouFeelingLucky()
        {
            var dice = _random.Next(1, 100);
            if (dice >= 1 && dice <= 20)
                throw new Exception($"InternalServer Error: {dice}");
        }
    }
}