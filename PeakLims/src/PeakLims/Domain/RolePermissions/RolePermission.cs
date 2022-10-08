namespace PeakLims.Domain.RolePermissions;

using Dtos;
using Validators;
using DomainEvents;
using FluentValidation;
using Roles;

public class RolePermission : BaseEntity
{
    public virtual Role Role { get; private set; }
    public virtual string Permission { get; private set; }


    public static RolePermission Create(RolePermissionForCreationDto rolePermissionForCreationDto)
    {
        new RolePermissionForCreationDtoValidator().ValidateAndThrow(rolePermissionForCreationDto);

        var newRolePermission = new RolePermission();

        newRolePermission.Role = new Role(rolePermissionForCreationDto.Role);
        newRolePermission.Permission = rolePermissionForCreationDto.Permission;

        newRolePermission.QueueDomainEvent(new RolePermissionCreated(){ RolePermission = newRolePermission });
        
        return newRolePermission;
    }

    public void Update(RolePermissionForUpdateDto rolePermissionForUpdateDto)
    {
        new RolePermissionForUpdateDtoValidator().ValidateAndThrow(rolePermissionForUpdateDto);

        Role = new Role(rolePermissionForUpdateDto.Role);
        Permission = rolePermissionForUpdateDto.Permission;

        QueueDomainEvent(new RolePermissionUpdated(){ Id = Id });
    }
    
    protected RolePermission() { } // For EF + Mocking
}