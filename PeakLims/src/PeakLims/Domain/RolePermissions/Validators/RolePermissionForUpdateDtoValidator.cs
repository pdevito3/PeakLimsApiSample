namespace PeakLims.Domain.RolePermissions.Validators;

using PeakLims.Domain.RolePermissions.Dtos;
using FluentValidation;

public sealed class RolePermissionForUpdateDtoValidator: RolePermissionForManipulationDtoValidator<RolePermissionForUpdateDto>
{
    public RolePermissionForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}