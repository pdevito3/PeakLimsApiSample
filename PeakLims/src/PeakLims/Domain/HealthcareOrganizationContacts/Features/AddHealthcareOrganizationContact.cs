namespace PeakLims.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddHealthcareOrganizationContact
{
    public sealed class Command : IRequest<HealthcareOrganizationContactDto>
    {
        public readonly HealthcareOrganizationContactForCreationDto HealthcareOrganizationContactToAdd;

        public Command(HealthcareOrganizationContactForCreationDto healthcareOrganizationContactToAdd)
        {
            HealthcareOrganizationContactToAdd = healthcareOrganizationContactToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, HealthcareOrganizationContactDto>
    {
        private readonly IHealthcareOrganizationContactRepository _healthcareOrganizationContactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationContactRepository healthcareOrganizationContactRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationContactRepository = healthcareOrganizationContactRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<HealthcareOrganizationContactDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddHealthcareOrganizationContacts);

            var healthcareOrganizationContact = HealthcareOrganizationContact.Create(request.HealthcareOrganizationContactToAdd);
            await _healthcareOrganizationContactRepository.Add(healthcareOrganizationContact, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var healthcareOrganizationContactAdded = await _healthcareOrganizationContactRepository.GetById(healthcareOrganizationContact.Id, cancellationToken: cancellationToken);
            return _mapper.Map<HealthcareOrganizationContactDto>(healthcareOrganizationContactAdded);
        }
    }
}