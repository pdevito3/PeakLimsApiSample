namespace PeakLims.Domain.RolePermissions.Validators;

using PeakLims.Domain.RolePermissions.Dtos;
using FluentValidation;

public sealed class RolePermissionForCreationDtoValidator: RolePermissionForManipulationDtoValidator<RolePermissionForCreationDto>
{
    public RolePermissionForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}