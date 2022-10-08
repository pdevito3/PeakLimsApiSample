namespace PeakLims.Domain.Patients.Features;

using PeakLims.Domain.Patients.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeletePatient
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid patient)
        {
            Id = patient;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeletePatients);

            var recordToDelete = await _patientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _patientRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}