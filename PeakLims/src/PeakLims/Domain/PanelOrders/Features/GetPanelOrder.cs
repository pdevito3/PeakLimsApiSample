namespace PeakLims.Domain.PanelOrders.Features;

using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetPanelOrder
{
    public sealed class Query : IRequest<PanelOrderDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PanelOrderDto>
    {
        private readonly IPanelOrderRepository _panelOrderRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelOrderRepository panelOrderRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _panelOrderRepository = panelOrderRepository;
            _heimGuard = heimGuard;
        }

        public async Task<PanelOrderDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadPanelOrders);

            var result = await _panelOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<PanelOrderDto>(result);
        }
    }
}