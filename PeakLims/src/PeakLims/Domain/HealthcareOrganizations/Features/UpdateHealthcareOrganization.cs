namespace PeakLims.Domain.HealthcareOrganizations.Features;

using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Validators;
using PeakLims.Domain.HealthcareOrganizations.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateHealthcareOrganization
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly HealthcareOrganizationForUpdateDto HealthcareOrganizationToUpdate;

        public Command(Guid healthcareOrganization, HealthcareOrganizationForUpdateDto newHealthcareOrganizationData)
        {
            Id = healthcareOrganization;
            HealthcareOrganizationToUpdate = newHealthcareOrganizationData;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateHealthcareOrganizations);

            var healthcareOrganizationToUpdate = await _healthcareOrganizationRepository.GetById(request.Id, cancellationToken: cancellationToken);

            healthcareOrganizationToUpdate.Update(request.HealthcareOrganizationToUpdate);
            _healthcareOrganizationRepository.Update(healthcareOrganizationToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}