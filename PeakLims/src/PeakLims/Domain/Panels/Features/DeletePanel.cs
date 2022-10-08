namespace PeakLims.Domain.Panels.Features;

using PeakLims.Domain.Panels.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeletePanel
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid panel)
        {
            Id = panel;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IPanelRepository _panelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelRepository panelRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _panelRepository = panelRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeletePanels);

            var recordToDelete = await _panelRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _panelRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}