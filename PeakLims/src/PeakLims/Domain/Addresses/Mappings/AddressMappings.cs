namespace PeakLims.Domain.Addresses.Mappings;

using PeakLims.Domain.Addresses.Dtos;
using SharedKernel.Domain;
using Mapster;

public sealed class AddressMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, PostalCode>()
            .MapWith(value => new PostalCode(value));
        config.NewConfig<PostalCode, string>()
            .MapWith(postalCode => postalCode.Value);

        config.NewConfig<AddressDto, Address>()
            .MapWith(address => new Address(address.Line1, address.Line2, address.City, address.State, address.PostalCode, address.Country))
            .TwoWays();
        config.NewConfig<AddressForCreationDto, Address>()
            .MapWith(address => new Address(address.Line1, address.Line2, address.City, address.State, address.PostalCode, address.Country))
            .TwoWays();
        config.NewConfig<AddressForUpdateDto, Address>()
            .MapWith(address => new Address(address.Line1, address.Line2, address.City, address.State, address.PostalCode, address.Country))
            .TwoWays();
    }
}