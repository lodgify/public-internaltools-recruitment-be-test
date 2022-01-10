using Microsoft.AspNetCore.Mvc;
using SuperPanel.API.Dto;
using SuperPanel.API.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SuperPanel.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeletionRequestController : ControllerBase
    {
        #region .: Properties :.
        private readonly IDeletionRequestService _deletionRequestService;

        #endregion .: Properties :.

        #region .: Constructor :.

        public DeletionRequestController(IDeletionRequestService deletionRequestService)
        {
            _deletionRequestService = deletionRequestService;
        }


        #endregion .: Constructor :.

        #region .: Public Methods :.
        [HttpGet]
        [Route("deletionRequests")]
        [ProducesResponseType(typeof(IEnumerable<DeletionRequestDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DeletionRequestDto>>> GetAllAsync()
        {
            try
            {
                var deletionRequest = await _deletionRequestService.GetAllAsync();


                return Ok(deletionRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("deletionRequests/{id:int}")]
        [ProducesResponseType(typeof(DeletionRequestDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DeletionRequestDto>> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                return Ok(await _deletionRequestService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("deletionRequests")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> InsertAsync([FromBody] DeletionRequestDto deletionRequest)
        {
            try
            {
                deletionRequest = await _deletionRequestService.InsertAsync(deletionRequest);

                return CreatedAtAction(nameof(GetByIdAsync), new { id = deletionRequest.Id }, deletionRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deletionRequests")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateAsync([FromBody] DeletionRequestDto deletionRequest)
        {
            try
            {
                await _deletionRequestService.UpdateAsync(deletionRequest);

                return CreatedAtAction(nameof(GetByIdAsync), new { id = deletionRequest.Id }, deletionRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion .: Public Methods :.
    }
}
