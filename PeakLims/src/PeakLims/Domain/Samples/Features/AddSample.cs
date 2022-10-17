namespace PeakLims.Domain.Samples.Features;

using Containers.Services;
using PeakLims.Domain.Samples.Services;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddSample
{
    public sealed class Command : IRequest<SampleDto>
    {
        public readonly SampleForCreationDto SampleToAdd;

        public Command(SampleForCreationDto sampleToAdd)
        {
            SampleToAdd = sampleToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, SampleDto>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ISampleRepository sampleRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard, IContainerRepository containerRepository)
        {
            _mapper = mapper;
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
            _containerRepository = containerRepository;
        }

        public async Task<SampleDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddSamples);

            var containerlessSampleToAdd = _mapper.Map<ContainerlessSampleForCreationDto>(request.SampleToAdd);
            var container = await _containerRepository.GetById(request.SampleToAdd.ContainerId, cancellationToken: cancellationToken);
            var sample = Sample.Create(containerlessSampleToAdd, container);
            await _sampleRepository.Add(sample, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);
            
            return _mapper.Map<SampleDto>(sample);
        }
    }
}