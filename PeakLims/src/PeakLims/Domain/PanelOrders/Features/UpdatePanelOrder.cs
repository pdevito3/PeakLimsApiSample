namespace PeakLims.Domain.PanelOrders.Features;

using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders.Validators;
using PeakLims.Domain.PanelOrders.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdatePanelOrder
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly PanelOrderForUpdateDto PanelOrderToUpdate;

        public Command(Guid panelOrder, PanelOrderForUpdateDto newPanelOrderData)
        {
            Id = panelOrder;
            PanelOrderToUpdate = newPanelOrderData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IPanelOrderRepository _panelOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelOrderRepository panelOrderRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _panelOrderRepository = panelOrderRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdatePanelOrders);

            var panelOrderToUpdate = await _panelOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);

            panelOrderToUpdate.Update(request.PanelOrderToUpdate);
            _panelOrderRepository.Update(panelOrderToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}