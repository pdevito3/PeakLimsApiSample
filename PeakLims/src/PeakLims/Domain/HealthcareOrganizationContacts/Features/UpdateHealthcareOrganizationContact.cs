namespace PeakLims.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts.Validators;
using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateHealthcareOrganizationContact
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly HealthcareOrganizationContactForUpdateDto HealthcareOrganizationContactToUpdate;

        public Command(Guid healthcareOrganizationContact, HealthcareOrganizationContactForUpdateDto newHealthcareOrganizationContactData)
        {
            Id = healthcareOrganizationContact;
            HealthcareOrganizationContactToUpdate = newHealthcareOrganizationContactData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IHealthcareOrganizationContactRepository _healthcareOrganizationContactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationContactRepository healthcareOrganizationContactRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _healthcareOrganizationContactRepository = healthcareOrganizationContactRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateHealthcareOrganizationContacts);

            var healthcareOrganizationContactToUpdate = await _healthcareOrganizationContactRepository.GetById(request.Id, cancellationToken: cancellationToken);

            healthcareOrganizationContactToUpdate.Update(request.HealthcareOrganizationContactToUpdate);
            _healthcareOrganizationContactRepository.Update(healthcareOrganizationContactToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}