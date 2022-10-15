namespace PeakLims.SharedTestHelpers.Fakes.Patient;

using AutoBogus;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;
using Services;

public class FakePatient
{
    public static Patient Generate(PatientForCreationDto patientForCreationDto, IDateTimeProvider dateTimeProvider)
    {
        return Patient.Create(patientForCreationDto, dateTimeProvider);
    }

    public static Patient Generate(IDateTimeProvider dateTimeProvider)
    {
        return Patient.Create(new FakePatientForCreationDto().Generate(), dateTimeProvider);
    }
}