namespace PeakLims.Domain.Panels.Features;

using PeakLims.Domain.Panels.Services;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddPanel
{
    public sealed class Command : IRequest<PanelDto>
    {
        public readonly PanelForCreationDto PanelToAdd;

        public Command(PanelForCreationDto panelToAdd)
        {
            PanelToAdd = panelToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, PanelDto>
    {
        private readonly IPanelRepository _panelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPanelRepository panelRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _panelRepository = panelRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<PanelDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddPanels);

            var panel = Panel.Create(request.PanelToAdd);
            await _panelRepository.Add(panel, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var panelAdded = await _panelRepository.GetById(panel.Id, cancellationToken: cancellationToken);
            return _mapper.Map<PanelDto>(panelAdded);
        }
    }
}