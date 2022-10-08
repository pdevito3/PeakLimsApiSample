namespace PeakLims.Domain.PanelOrders.Features;

using PeakLims.Domain.PanelOrders.Services;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddPanelOrder
{
    public sealed class Command : IRequest<PanelOrderDto>
    {
        public readonly PanelOrderForCreationDto PanelOrderToAdd;

        public Command(PanelOrderForCreationDto panelOrderToAdd)
        {
            PanelOrderToAdd = panelOrderToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, PanelOrderDto>
    {
        private readonly IPanelOrderRepository _panelOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelOrderRepository panelOrderRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _panelOrderRepository = panelOrderRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<PanelOrderDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddPanelOrders);

            var panelOrder = PanelOrder.Create(request.PanelOrderToAdd);
            await _panelOrderRepository.Add(panelOrder, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var panelOrderAdded = await _panelOrderRepository.GetById(panelOrder.Id, cancellationToken: cancellationToken);
            return _mapper.Map<PanelOrderDto>(panelOrderAdded);
        }
    }
}