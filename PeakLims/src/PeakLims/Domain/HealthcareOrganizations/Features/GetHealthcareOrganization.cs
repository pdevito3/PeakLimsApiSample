namespace PeakLims.Domain.HealthcareOrganizations.Features;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetHealthcareOrganization
{
    public sealed class Query : IRequest<HealthcareOrganizationDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, HealthcareOrganizationDto>
    {
        private readonly IHealthcareOrganizationRepository _healthcareOrganizationRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationRepository healthcareOrganizationRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationRepository = healthcareOrganizationRepository;
            _heimGuard = heimGuard;
        }

        public async Task<HealthcareOrganizationDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadHealthcareOrganizations);

            var result = await _healthcareOrganizationRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<HealthcareOrganizationDto>(result);
        }
    }
}