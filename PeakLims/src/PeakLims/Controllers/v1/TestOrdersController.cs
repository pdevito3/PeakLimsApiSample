namespace PeakLims.Controllers.v1;

using PeakLims.Domain.TestOrders.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Domain.TestOrders.Features;
using MediatR;

[ApiController]
[Route("api/testorders")]
[ApiVersion("1.0")]
public sealed class TestOrdersController: ControllerBase
{
    private readonly IMediator _mediator;

    public TestOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a single Test Order by ID.
    /// </summary>
    /// <response code="200">Test Order record returned successfully.</response>
    /// <response code="400">Test Order has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while getting the Test Order.</response>
    [ProducesResponseType(typeof(TestOrderDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetTestOrder")]
    public async Task<ActionResult<TestOrderDto>> GetTestOrder(Guid id)
    {
        var query = new GetTestOrder.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Cancels a test order
    /// </summary>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{testOrderId:guid}/cancel", Name = "CancelTestOrder")]
    public async Task<IActionResult> CancelTestOrder(Guid testOrderId, [FromBody] CancelTestOrderDto cancelTestOrderDto)
    {
        var command = new CancelTestOrder.Command(testOrderId, cancelTestOrderDto.Reason, cancelTestOrderDto.Comments);
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Sets a given sample on a test order
    /// </summary>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{testOrderId:guid}/SetSample/{sampleId:guid}", Name = "SetSampleOnTestOrder")]
    public async Task<IActionResult> SetSampleOnTestOrder(Guid testOrderId, Guid sampleId)
    {
        var command = new SetSampleOnTestOrder.Command(testOrderId, sampleId);
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Removes the sample a test order when present
    /// </summary>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{testOrderId:guid}/RemoveSample", Name = "RemoveSampleOnTestOrder")]
    public async Task<IActionResult> RemoveSampleOnTestOrder(Guid testOrderId)
    {
        var command = new RemoveSampleOnTestOrder.Command(testOrderId);
        await _mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
