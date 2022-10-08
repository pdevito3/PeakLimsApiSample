namespace PeakLims.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using PeakLims.Wrappers;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetHealthcareOrganizationContactList
{
    public sealed class Query : IRequest<PagedList<HealthcareOrganizationContactDto>>
    {
        public readonly HealthcareOrganizationContactParametersDto QueryParameters;

        public Query(HealthcareOrganizationContactParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<HealthcareOrganizationContactDto>>
    {
        private readonly IHealthcareOrganizationContactRepository _healthcareOrganizationContactRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationContactRepository healthcareOrganizationContactRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationContactRepository = healthcareOrganizationContactRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<HealthcareOrganizationContactDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadHealthcareOrganizationContacts);

            var collection = _healthcareOrganizationContactRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<HealthcareOrganizationContactDto>();

            return await PagedList<HealthcareOrganizationContactDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}