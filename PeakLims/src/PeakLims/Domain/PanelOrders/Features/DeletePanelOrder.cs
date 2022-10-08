namespace PeakLims.Domain.PanelOrders.Features;

using PeakLims.Domain.PanelOrders.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeletePanelOrder
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid panelOrder)
        {
            Id = panelOrder;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeletePanelOrders);

            var recordToDelete = await _panelOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _panelOrderRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}