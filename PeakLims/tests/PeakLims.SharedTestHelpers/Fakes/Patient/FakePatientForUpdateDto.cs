namespace PeakLims.SharedTestHelpers.Fakes.Patient;

using AutoBogus;
using Domain.Ethnicities;
using Domain.Races;
using Domain.Sexes;
using Lifespan;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakePatientForUpdateDto : AutoFaker<PatientForUpdateDto>
{
    public FakePatientForUpdateDto()
    {
        RuleFor(x => x.Lifespan, () => new FakeLifespanForUpdateDto().Generate());
        RuleFor(x => x.Sex, f => f.PickRandom(Sex.ListNames()));
        RuleFor(x => x.Race, f => f.PickRandom(Race.ListNames()));
        RuleFor(x => x.Ethnicity, f => f.PickRandom(Ethnicity.ListNames()));
    }
}