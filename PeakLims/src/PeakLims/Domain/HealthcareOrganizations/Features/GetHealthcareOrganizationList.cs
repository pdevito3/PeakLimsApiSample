namespace PeakLims.Domain.HealthcareOrganizations.Features;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Services;
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

public static class GetHealthcareOrganizationList
{
    public sealed class Query : IRequest<PagedList<HealthcareOrganizationDto>>
    {
        public readonly HealthcareOrganizationParametersDto QueryParameters;

        public Query(HealthcareOrganizationParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<HealthcareOrganizationDto>>
    {
        private readonly IHealthcareOrganizationRepository _healthcareOrganizationRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IHealthcareOrganizationRepository healthcareOrganizationRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _healthcareOrganizationRepository = healthcareOrganizationRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<HealthcareOrganizationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadHealthcareOrganizations);

            var collection = _healthcareOrganizationRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<HealthcareOrganizationDto>();

            return await PagedList<HealthcareOrganizationDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}