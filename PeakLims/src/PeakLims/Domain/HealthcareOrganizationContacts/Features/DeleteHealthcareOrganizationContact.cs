namespace PeakLims.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteHealthcareOrganizationContact
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid healthcareOrganizationContact)
        {
            Id = healthcareOrganizationContact;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteHealthcareOrganizationContacts);

            var recordToDelete = await _healthcareOrganizationContactRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _healthcareOrganizationContactRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}