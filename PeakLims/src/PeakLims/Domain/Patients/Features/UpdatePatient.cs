namespace PeakLims.Domain.Patients.Features;

using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;
using PeakLims.Domain.Patients.Validators;
using PeakLims.Domain.Patients.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdatePatient
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly PatientForUpdateDto PatientToUpdate;

        public Command(Guid patient, PatientForUpdateDto newPatientData)
        {
            Id = patient;
            PatientToUpdate = newPatientData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;
        private readonly IDateTimeProvider _dateTimeProvider;

        public Handler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard, IDateTimeProvider dateTimeProvider)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdatePatients);

            var patientToUpdate = await _patientRepository.GetById(request.Id, cancellationToken: cancellationToken);

            patientToUpdate.Update(request.PatientToUpdate, _dateTimeProvider);
            _patientRepository.Update(patientToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}