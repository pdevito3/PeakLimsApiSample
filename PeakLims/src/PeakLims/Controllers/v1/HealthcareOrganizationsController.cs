namespace PeakLims.Controllers.v1;

using PeakLims.Domain.HealthcareOrganizations.Features;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
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
[Route("api/healthcareorganizations")]
[ApiVersion("1.0")]
public sealed class HealthcareOrganizationsController: ControllerBase
{
    private readonly IMediator _mediator;

    public HealthcareOrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all HealthcareOrganizations.
    /// </summary>
    /// <response code="200">HealthcareOrganization list returned successfully.</response>
    /// <response code="400">HealthcareOrganization has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the HealthcareOrganization.</response>
    /// <remarks>
    /// Requests can be narrowed down with a variety of query string values:
    /// ## Query String Parameters
    /// - **PageNumber**: An integer value that designates the page of records that should be returned.
    /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
    /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
    /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
    ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
    ///     - {Operator} is one of the Operators below
    ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
    ///
    ///    | Operator | Meaning                       | Operator  | Meaning                                      |
    ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
    ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
    ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
    ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
    ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
    ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
    ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
    ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
    ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
    /// </remarks>
    [ProducesResponseType(typeof(IEnumerable<HealthcareOrganizationDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet(Name = "GetHealthcareOrganizations")]
    public async Task<IActionResult> GetHealthcareOrganizations([FromQuery] HealthcareOrganizationParametersDto healthcareOrganizationParametersDto)
    {
        var query = new GetHealthcareOrganizationList.Query(healthcareOrganizationParametersDto);
        var queryResponse = await _mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a single HealthcareOrganization by ID.
    /// </summary>
    /// <response code="200">HealthcareOrganization record returned successfully.</response>
    /// <response code="400">HealthcareOrganization has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the HealthcareOrganization.</response>
    [ProducesResponseType(typeof(HealthcareOrganizationDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetHealthcareOrganization")]
    public async Task<ActionResult<HealthcareOrganizationDto>> GetHealthcareOrganization(Guid id)
    {
        var query = new GetHealthcareOrganization.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new HealthcareOrganization record.
    /// </summary>
    /// <response code="201">HealthcareOrganization created.</response>
    /// <response code="400">HealthcareOrganization has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the HealthcareOrganization.</response>
    [ProducesResponseType(typeof(HealthcareOrganizationDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost(Name = "AddHealthcareOrganization")]
    public async Task<ActionResult<HealthcareOrganizationDto>> AddHealthcareOrganization([FromBody]HealthcareOrganizationForCreationDto healthcareOrganizationForCreation)
    {
        var command = new AddHealthcareOrganization.Command(healthcareOrganizationForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetHealthcareOrganization",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing HealthcareOrganization.
    /// </summary>
    /// <response code="204">HealthcareOrganization updated.</response>
    /// <response code="400">HealthcareOrganization has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the HealthcareOrganization.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}", Name = "UpdateHealthcareOrganization")]
    public async Task<IActionResult> UpdateHealthcareOrganization(Guid id, HealthcareOrganizationForUpdateDto healthcareOrganization)
    {
        var command = new UpdateHealthcareOrganization.Command(id, healthcareOrganization);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing HealthcareOrganization record.
    /// </summary>
    /// <response code="204">HealthcareOrganization deleted.</response>
    /// <response code="400">HealthcareOrganization has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the HealthcareOrganization.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpDelete("{id:guid}", Name = "DeleteHealthcareOrganization")]
    public async Task<ActionResult> DeleteHealthcareOrganization(Guid id)
    {
        var command = new DeleteHealthcareOrganization.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Sets a Healthcare Organization status to `Active`
    /// </summary>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}/activate", Name = "SetHealthcareOrganizationStatusToActive")]
    public async Task<IActionResult> SetHealthcareOrganizationStatusToActive(Guid id)
    {
        var command = new ActivateHealthcareOrganization.Command(id);
        await _mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Sets a Healthcare Organization status to `Inactive`
    /// </summary>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}/deactivate", Name = "SetHealthcareOrganizationStatusToInactive")]
    public async Task<IActionResult> SetHealthcareOrganizationStatusToInactive(Guid id)
    {
        var command = new DeactivateHealthcareOrganization.Command(id);
        await _mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
