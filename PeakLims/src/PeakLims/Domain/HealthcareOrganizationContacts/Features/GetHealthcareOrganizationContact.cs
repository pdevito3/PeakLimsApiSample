namespace PeakLims.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetHealthcareOrganizationContact
{
    public sealed class Query : IRequest<HealthcareOrganizationContactDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, HealthcareOrganizationContactDto>
    {
        private readonly IHealthcareOrganizationContactRepository _healthcareOrganizationContactRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationContactRepository healthcareOrganizationContactRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationContactRepository = healthcareOrganizationContactRepository;
            _heimGuard = heimGuard;
        }

        public async Task<HealthcareOrganizationContactDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadHealthcareOrganizationContacts);

            var result = await _healthcareOrganizationContactRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<HealthcareOrganizationContactDto>(result);
        }
    }
}