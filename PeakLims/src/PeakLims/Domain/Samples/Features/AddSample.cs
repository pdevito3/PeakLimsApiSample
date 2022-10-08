namespace PeakLims.Domain.Samples.Features;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ISampleRepository sampleRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<SampleDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddSamples);

            var sample = Sample.Create(request.SampleToAdd);
            await _sampleRepository.Add(sample, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var sampleAdded = await _sampleRepository.GetById(sample.Id, cancellationToken: cancellationToken);
            return _mapper.Map<SampleDto>(sampleAdded);
        }
    }
}