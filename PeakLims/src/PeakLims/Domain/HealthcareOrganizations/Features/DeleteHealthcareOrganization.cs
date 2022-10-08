namespace PeakLims.Domain.HealthcareOrganizations.Features;

using PeakLims.Domain.HealthcareOrganizations.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteHealthcareOrganization
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid healthcareOrganization)
        {
            Id = healthcareOrganization;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IHealthcareOrganizationRepository _healthcareOrganizationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationRepository healthcareOrganizationRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _healthcareOrganizationRepository = healthcareOrganizationRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteHealthcareOrganizations);

            var recordToDelete = await _healthcareOrganizationRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _healthcareOrganizationRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}