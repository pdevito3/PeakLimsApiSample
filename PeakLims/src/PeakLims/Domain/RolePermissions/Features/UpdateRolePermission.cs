namespace PeakLims.Domain.RolePermissions.Features;

using PeakLims.Domain.RolePermissions;
using PeakLims.Domain.RolePermissions.Dtos;
using PeakLims.Domain.RolePermissions.Validators;
using PeakLims.Domain.RolePermissions.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateRolePermission
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly RolePermissionForUpdateDto RolePermissionToUpdate;

        public Command(Guid rolePermission, RolePermissionForUpdateDto newRolePermissionData)
        {
            Id = rolePermission;
            RolePermissionToUpdate = newRolePermissionData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateRolePermissions);

            var rolePermissionToUpdate = await _rolePermissionRepository.GetById(request.Id, cancellationToken: cancellationToken);

            rolePermissionToUpdate.Update(request.RolePermissionToUpdate);
            _rolePermissionRepository.Update(rolePermissionToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}