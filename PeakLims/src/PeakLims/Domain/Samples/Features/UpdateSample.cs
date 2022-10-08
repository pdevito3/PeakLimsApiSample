namespace PeakLims.Domain.Samples.Features;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ISampleRepository sampleRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _sampleRepository = sampleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateSamples);

            var sampleToUpdate = await _sampleRepository.GetById(request.Id, cancellationToken: cancellationToken);

            sampleToUpdate.Update(request.SampleToUpdate);
            _sampleRepository.Update(sampleToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}