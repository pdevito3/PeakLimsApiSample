namespace PeakLims.Domain.HealthcareOrganizationContacts.Mappings;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts;
using Mapster;

public sealed class HealthcareOrganizationContactMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<HealthcareOrganizationContact, HealthcareOrganizationContactDto>();
        config.NewConfig<HealthcareOrganizationContactForCreationDto, HealthcareOrganizationContact>()
            .TwoWays();
        config.NewConfig<HealthcareOrganizationContactForUpdateDto, HealthcareOrganizationContact>()
            .TwoWays();
    }
}