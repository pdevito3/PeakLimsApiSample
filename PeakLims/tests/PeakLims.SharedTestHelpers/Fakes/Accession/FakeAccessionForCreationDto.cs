namespace PeakLims.SharedTestHelpers.Fakes.Accession;

using AutoBogus;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeAccessionForCreationDto : AutoFaker<AccessionForCreationDto>
{
    public FakeAccessionForCreationDto()
    {
        RuleFor(a => a.HealthcareOrganizationId, _ => null);
        RuleFor(a => a.PatientId, _ => null);
    }
}