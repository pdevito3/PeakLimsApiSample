namespace PeakLims.Domain.Emails.Mappings;

using SharedKernel.Domain;
using Mapster;

public sealed class EmailMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Email>()
            .MapWith(value => new Email(value));
        config.NewConfig<Email, string>()
            .MapWith(email => email.Value);
    }
}