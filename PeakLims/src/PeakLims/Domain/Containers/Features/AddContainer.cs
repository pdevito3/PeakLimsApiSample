namespace PeakLims.Domain.Containers.Features;

using PeakLims.Domain.Containers.Services;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddContainer
{
    public sealed class Command : IRequest<ContainerDto>
    {
        public readonly ContainerForCreationDto ContainerToAdd;

        public Command(ContainerForCreationDto containerToAdd)
        {
            ContainerToAdd = containerToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, ContainerDto>
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IContainerRepository containerRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _containerRepository = containerRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<ContainerDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddContainers);

            var container = Container.Create(request.ContainerToAdd);
            await _containerRepository.Add(container, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var containerAdded = await _containerRepository.GetById(container.Id, cancellationToken: cancellationToken);
            return _mapper.Map<ContainerDto>(containerAdded);
        }
    }
}