namespace PeakLims.Domain.TestOrders.Features;

using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.Services;
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

public static class GetTestOrderList
{
    public sealed class Query : IRequest<PagedList<TestOrderDto>>
    {
        public readonly TestOrderParametersDto QueryParameters;

        public Query(TestOrderParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<TestOrderDto>>
    {
        private readonly ITestOrderRepository _testOrderRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ITestOrderRepository testOrderRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _testOrderRepository = testOrderRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<TestOrderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadTestOrders);

            var collection = _testOrderRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<TestOrderDto>();

            return await PagedList<TestOrderDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}