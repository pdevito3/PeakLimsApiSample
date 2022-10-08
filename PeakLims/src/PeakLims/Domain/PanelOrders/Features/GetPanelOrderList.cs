namespace PeakLims.Domain.PanelOrders.Features;

using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders.Services;
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

public static class GetPanelOrderList
{
    public sealed class Query : IRequest<PagedList<PanelOrderDto>>
    {
        public readonly PanelOrderParametersDto QueryParameters;

        public Query(PanelOrderParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<PanelOrderDto>>
    {
        private readonly IPanelOrderRepository _panelOrderRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelOrderRepository panelOrderRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _panelOrderRepository = panelOrderRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<PanelOrderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadPanelOrders);

            var collection = _panelOrderRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<PanelOrderDto>();

            return await PagedList<PanelOrderDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}