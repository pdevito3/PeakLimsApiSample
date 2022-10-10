namespace PeakLims.Domain.Panels.Features;

using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels.Validators;
using PeakLims.Domain.Panels.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdatePanel
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly PanelForUpdateDto UpdatedPanelData;

        public Command(Guid panel, PanelForUpdateDto newPanelData)
        {
            Id = panel;
            UpdatedPanelData = newPanelData;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdatePanels);

            var panelToUpdate = await _panelRepository.GetById(request.Id, cancellationToken: cancellationToken);
            Panel.GuardWhenExists(panelToUpdate.PanelCode, request.UpdatedPanelData.Version, _panelRepository);

            panelToUpdate.Update(request.UpdatedPanelData);
            _panelRepository.Update(panelToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}