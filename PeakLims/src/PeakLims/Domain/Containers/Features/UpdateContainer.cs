namespace PeakLims.Domain.Containers.Features;

using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.Dtos;
using PeakLims.Domain.Containers.Validators;
using PeakLims.Domain.Containers.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateContainer
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly ContainerForUpdateDto ContainerToUpdate;

        public Command(Guid container, ContainerForUpdateDto newContainerData)
        {
            Id = container;
            ContainerToUpdate = newContainerData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IContainerRepository containerRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _containerRepository = containerRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateContainers);

            var containerToUpdate = await _containerRepository.GetById(request.Id, cancellationToken: cancellationToken);

            containerToUpdate.Update(request.ContainerToUpdate);
            _containerRepository.Update(containerToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}