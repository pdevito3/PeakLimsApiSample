namespace PeakLims.Domain.Accessions.Features;

using PeakLims.Domain.Accessions.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteAccession
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid accession)
        {
            Id = accession;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IAccessionRepository _accessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionRepository accessionRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _accessionRepository = accessionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteAccessions);

            var recordToDelete = await _accessionRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _accessionRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}