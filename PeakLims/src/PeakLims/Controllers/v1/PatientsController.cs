namespace PeakLims.Controllers.v1;

using PeakLims.Domain.Patients.Features;
using PeakLims.Domain.Patients.Dtos;
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
[Route("api/patients")]
[ApiVersion("1.0")]
public sealed class PatientsController: ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all Patients.
    /// </summary>
    /// <response code="200">Patient list returned successfully.</response>
    /// <response code="400">Patient has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the Patient.</response>
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
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet(Name = "GetPatients")]
    public async Task<IActionResult> GetPatients([FromQuery] PatientParametersDto patientParametersDto)
    {
        var query = new GetPatientList.Query(patientParametersDto);
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
    /// Gets a single Patient by ID.
    /// </summary>
    /// <response code="200">Patient record returned successfully.</response>
    /// <response code="400">Patient has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the Patient.</response>
    [ProducesResponseType(typeof(PatientDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetPatient")]
    public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
    {
        var query = new GetPatient.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new Patient record.
    /// </summary>
    /// <response code="201">Patient created.</response>
    /// <response code="400">Patient has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the Patient.</response>
    [ProducesResponseType(typeof(PatientDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost(Name = "AddPatient")]
    public async Task<ActionResult<PatientDto>> AddPatient([FromBody]PatientForCreationDto patientForCreation)
    {
        var command = new AddPatient.Command(patientForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetPatient",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing Patient.
    /// </summary>
    /// <response code="204">Patient updated.</response>
    /// <response code="400">Patient has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the Patient.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}", Name = "UpdatePatient")]
    public async Task<IActionResult> UpdatePatient(Guid id, PatientForUpdateDto patient)
    {
        var command = new UpdatePatient.Command(id, patient);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing Patient record.
    /// </summary>
    /// <response code="204">Patient deleted.</response>
    /// <response code="400">Patient has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the Patient.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpDelete("{id:guid}", Name = "DeletePatient")]
    public async Task<ActionResult> DeletePatient(Guid id)
    {
        var command = new DeletePatient.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
