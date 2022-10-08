namespace PeakLims.SharedTestHelpers.Fakes.Patient;

using AutoBogus;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;

public class FakePatient
{
    public static Patient Generate(PatientForCreationDto patientForCreationDto)
    {
        return Patient.Create(patientForCreationDto);
    }

    public static Patient Generate()
    {
        return Patient.Create(new FakePatientForCreationDto().Generate());
    }
}