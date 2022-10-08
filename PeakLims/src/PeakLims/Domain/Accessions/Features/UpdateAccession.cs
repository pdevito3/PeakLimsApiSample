namespace PeakLims.Domain.Accessions.Features;

using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions.Validators;
using PeakLims.Domain.Accessions.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateAccession
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly AccessionForUpdateDto AccessionToUpdate;

        public Command(Guid accession, AccessionForUpdateDto newAccessionData)
        {
            Id = accession;
            AccessionToUpdate = newAccessionData;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateAccessions);

            var accessionToUpdate = await _accessionRepository.GetById(request.Id, cancellationToken: cancellationToken);

            accessionToUpdate.Update(request.AccessionToUpdate);
            _accessionRepository.Update(accessionToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}