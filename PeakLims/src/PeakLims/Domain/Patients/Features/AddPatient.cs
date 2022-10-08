namespace PeakLims.Domain.Patients.Features;

using PeakLims.Domain.Patients.Services;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddPatient
{
    public sealed class Command : IRequest<PatientDto>
    {
        public readonly PatientForCreationDto PatientToAdd;

        public Command(PatientForCreationDto patientToAdd)
        {
            PatientToAdd = patientToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, PatientDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<PatientDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddPatients);

            var patient = Patient.Create(request.PatientToAdd);
            await _patientRepository.Add(patient, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var patientAdded = await _patientRepository.GetById(patient.Id, cancellationToken: cancellationToken);
            return _mapper.Map<PatientDto>(patientAdded);
        }
    }
}