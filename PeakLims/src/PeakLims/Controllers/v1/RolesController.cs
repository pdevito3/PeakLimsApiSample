namespace PeakLims.Controllers.v1;

using Domain;
using Domain.Roles;
using HeimGuard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Exceptions;

[ApiController]
[Route("api/roles")]
[ApiVersion("1.0")]
public sealed class RolesController: ControllerBase
{
    private readonly IHeimGuardClient _heimGuard;

    public RolesController(IHeimGuardClient heimGuard)
    {
        _heimGuard = heimGuard;
    }

    /// <summary>
    /// Gets a list of all available roles.
    /// </summary>
    /// <response code="200">List retrieved.</response>
    /// <response code="500">There was an error getting the list of roles.</response>
    [HttpGet(Name = "GetRoles")]
    [Authorize]
    public List<string> GetRoles()
    {
        _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanGetRoles);
        return Role.ListNames();
    }
}