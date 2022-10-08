namespace PeakLims.Domain.HealthcareOrganizations.Mappings;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations;
using Mapster;

public sealed class HealthcareOrganizationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<HealthcareOrganization, HealthcareOrganizationDto>();
        config.NewConfig<HealthcareOrganizationForCreationDto, HealthcareOrganization>()
            .TwoWays();
        config.NewConfig<HealthcareOrganizationForUpdateDto, HealthcareOrganization>()
            .TwoWays();
    }
}