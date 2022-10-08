namespace PeakLims.Domain.Patients.Mappings;

using PeakLims.Domain.Patients.Dtos;
using PeakLims.Domain.Patients;
using Mapster;

public sealed class PatientMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Patient, PatientDto>();
        config.NewConfig<PatientForCreationDto, Patient>()
            .TwoWays();
        config.NewConfig<PatientForUpdateDto, Patient>()
            .TwoWays();
    }
}