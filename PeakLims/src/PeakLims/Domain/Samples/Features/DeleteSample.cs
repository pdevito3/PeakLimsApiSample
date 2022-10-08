namespace PeakLims.Domain.Samples.Features;

using PeakLims.Domain.Samples.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteSample
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid sample)
        {
            Id = sample;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteSamples);

            var recordToDelete = await _sampleRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _sampleRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}