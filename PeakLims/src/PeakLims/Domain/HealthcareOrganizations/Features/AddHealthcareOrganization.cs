namespace PeakLims.Domain.HealthcareOrganizations.Features;

using PeakLims.Domain.HealthcareOrganizations.Services;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddHealthcareOrganization
{
    public sealed class Command : IRequest<HealthcareOrganizationDto>
    {
        public readonly HealthcareOrganizationForCreationDto HealthcareOrganizationToAdd;

        public Command(HealthcareOrganizationForCreationDto healthcareOrganizationToAdd)
        {
            HealthcareOrganizationToAdd = healthcareOrganizationToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, HealthcareOrganizationDto>
    {
        private readonly IHealthcareOrganizationRepository _healthcareOrganizationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationRepository healthcareOrganizationRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationRepository = healthcareOrganizationRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<HealthcareOrganizationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddHealthcareOrganizations);

            var healthcareOrganization = HealthcareOrganization.Create(request.HealthcareOrganizationToAdd);
            await _healthcareOrganizationRepository.Add(healthcareOrganization, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var healthcareOrganizationAdded = await _healthcareOrganizationRepository.GetById(healthcareOrganization.Id, cancellationToken: cancellationToken);
            return _mapper.Map<HealthcareOrganizationDto>(healthcareOrganizationAdded);
        }
    }
}