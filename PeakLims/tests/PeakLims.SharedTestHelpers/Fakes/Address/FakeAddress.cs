namespace PeakLims.SharedTestHelpers.Fakes.Address;

using Domain.Addresses.Dtos;
using PeakLims.Domain.Addresses;
using PeakLims.Domain.Addresses.Dtos;

public class FakeAddress
{
    public static Address Generate(AddressForCreationDto addressForCreationDto)
    {
        return new Address(addressForCreationDto.Line1,
            addressForCreationDto.Line2,
            addressForCreationDto.City,
            addressForCreationDto.State,
            addressForCreationDto.PostalCode,
            addressForCreationDto.Country);
    }

    public static Address Generate()
    {
        return Generate(new FakeAddressForCreationDto().Generate());
    }
}