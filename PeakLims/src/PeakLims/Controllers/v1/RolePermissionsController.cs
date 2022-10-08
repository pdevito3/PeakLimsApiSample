namespace PeakLims.Controllers.v1;

using PeakLims.Domain.RolePermissions.Features;
using PeakLims.Domain.RolePermissions.Dtos;
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
[Route("api/rolepermissions")]
[ApiVersion("1.0")]
public sealed class RolePermissionsController: ControllerBase
{
    private readonly IMediator _mediator;

    public RolePermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    /// <summary>
    /// Gets a list of all RolePermissions.
    /// </summary>
    /// <response code="200">RolePermission list returned successfully.</response>
    /// <response code="400">RolePermission has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the RolePermission.</response>
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
    [ProducesResponseType(typeof(IEnumerable<RolePermissionDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet(Name = "GetRolePermissions")]
    public async Task<IActionResult> GetRolePermissions([FromQuery] RolePermissionParametersDto rolePermissionParametersDto)
    {
        var query = new GetRolePermissionList.Query(rolePermissionParametersDto);
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
    /// Gets a single RolePermission by ID.
    /// </summary>
    /// <response code="200">RolePermission record returned successfully.</response>
    /// <response code="400">RolePermission has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the RolePermission.</response>
    [ProducesResponseType(typeof(RolePermissionDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpGet("{id:guid}", Name = "GetRolePermission")]
    public async Task<ActionResult<RolePermissionDto>> GetRolePermission(Guid id)
    {
        var query = new GetRolePermission.Query(id);
        var queryResponse = await _mediator.Send(query);

        return Ok(queryResponse);
    }


    /// <summary>
    /// Creates a new RolePermission record.
    /// </summary>
    /// <response code="201">RolePermission created.</response>
    /// <response code="400">RolePermission has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the RolePermission.</response>
    [ProducesResponseType(typeof(RolePermissionDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost(Name = "AddRolePermission")]
    public async Task<ActionResult<RolePermissionDto>> AddRolePermission([FromBody]RolePermissionForCreationDto rolePermissionForCreation)
    {
        var command = new AddRolePermission.Command(rolePermissionForCreation);
        var commandResponse = await _mediator.Send(command);

        return CreatedAtRoute("GetRolePermission",
            new { commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Updates an entire existing RolePermission.
    /// </summary>
    /// <response code="204">RolePermission updated.</response>
    /// <response code="400">RolePermission has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the RolePermission.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpPut("{id:guid}", Name = "UpdateRolePermission")]
    public async Task<IActionResult> UpdateRolePermission(Guid id, RolePermissionForUpdateDto rolePermission)
    {
        var command = new UpdateRolePermission.Command(id, rolePermission);
        await _mediator.Send(command);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing RolePermission record.
    /// </summary>
    /// <response code="204">RolePermission deleted.</response>
    /// <response code="400">RolePermission has missing/invalid values.</response>
    /// <response code="401">This request was not able to be authenticated.</response>
    /// <response code="403">The required permissions to access this resource were not present in the given request.</response>
    /// <response code="500">There was an error on the server while creating the RolePermission.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize]
    [Produces("application/json")]
    [HttpDelete("{id:guid}", Name = "DeleteRolePermission")]
    public async Task<ActionResult> DeleteRolePermission(Guid id)
    {
        var command = new DeleteRolePermission.Command(id);
        await _mediator.Send(command);

        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
