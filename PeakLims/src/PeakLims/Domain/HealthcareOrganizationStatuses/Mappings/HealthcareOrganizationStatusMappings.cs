namespace PeakLims.Domain.HealthcareOrganizationStatuses.Mappings;

using Mapster;

public sealed class HealthcareOrganizationStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, HealthcareOrganizationStatus>()
            .MapWith(value => new HealthcareOrganizationStatus(value));
        config.NewConfig<HealthcareOrganizationStatus, string>()
            .MapWith(role => role.Value);
    }
}