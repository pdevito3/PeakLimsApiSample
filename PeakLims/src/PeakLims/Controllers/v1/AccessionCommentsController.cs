namespace PeakLims.Controllers.v1;

using PeakLims.Domain.AccessionComments.Features;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Wrappers;
using PeakLims.Domain;
using SharedKernel.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

[ApiController]
[Route("api/accessioncomments")]
[ApiVersion("1.0")]
public sealed class AccessionCommentsController: ControllerBase
{
    private readonly IMediator _mediator;

    public AccessionCommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a single AccessionComment by ID.
    /// </summary>
    /// <response code="200">AccessionComment record returned successfully.</response>
    /// <response code="400">AccessionComment has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the AccessionComment.</response>
    [ProducesResponseType(typeof(AccessionCommentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetAccessionComment")]
    public async Task<ActionResult<AccessionCommentDto>> GetAccessionComment(Guid id)
    {
        var query = new GetAccessionComment.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new AccessionComment record.
    /// </summary>
    /// <response code="201">AccessionComment created.</response>
    /// <response code="400">AccessionComment has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the AccessionComment.</response>
    [ProducesResponseType(typeof(AccessionCommentDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost(Name = "AddAccessionComment")]
    public async Task<ActionResult<AccessionCommentDto>> AddAccessionComment([FromBody]AccessionCommentForCreationDto accessionCommentForCreation)
    {
        var command = new AddAccessionComment.Command(accessionCommentForCreation.AccessionId, accessionCommentForCreation.Comment);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetAccessionComment",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing AccessionComment.
    /// </summary>
    /// <response code="204">AccessionComment updated.</response>
    /// <response code="400">AccessionComment has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the AccessionComment.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}", Name = "UpdateAccessionComment")]
    public async Task<IActionResult> UpdateAccessionComment(Guid id, AccessionCommentForUpdateDto accessionComment)
    {
        var command = new UpdateAccessionComment.Command(id, accessionComment.Comment);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
