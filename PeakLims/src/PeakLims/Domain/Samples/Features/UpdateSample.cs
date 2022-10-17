namespace PeakLims.Domain.Samples.Features;

using Containers.Services;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;
using PeakLims.Domain.Samples.Validators;
using PeakLims.Domain.Samples.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateSample
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly SampleForUpdateDto SampleToUpdate;

        public Command(Guid sample, SampleForUpdateDto newSampleData)
        {
            Id = sample;
            SampleToUpdate = newSampleData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;
        private readonly IMapper _mapper;

        public Handler(ISampleRepository sampleRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard, IMapper mapper, IContainerRepository containerRepository)
        {
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
            _mapper = mapper;
            _containerRepository = containerRepository;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateSamples);

            var sampleToUpdate = await _sampleRepository.GetById(request.Id, cancellationToken: cancellationToken);
            var containerlessUpdatedSample = _mapper.Map<ContainerlessSampleForUpdateDto>(request.SampleToUpdate);
            sampleToUpdate.Update(containerlessUpdatedSample);
            await sampleToUpdate.SetSampleContainer(request.SampleToUpdate.ContainerId, _containerRepository, cancellationToken);
            _sampleRepository.Update(sampleToUpdate);
            
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}