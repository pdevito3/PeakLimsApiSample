namespace PeakLims.SharedTestHelpers.Fakes.Address;

using AutoBogus;
using Bogus;
using Domain.Addresses.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public sealed class FakeAddressForCreationDto : AutoFaker<AddressForCreationDto>
{
    public FakeAddressForCreationDto()
    {
        RuleFor(u => u.Line1, f => f.Address.StreetAddress());
        RuleFor(u => u.Line2, f => f.Address.SecondaryAddress());
        RuleFor(u => u.City, f => f.Address.City());
        RuleFor(u => u.State, f => f.Address.State());
        RuleFor(u => u.PostalCode, f => f.Address.ZipCode());
        RuleFor(u => u.Country, f => f.Address.Country());
    }
}