namespace PeakLims.SharedTestHelpers.Fakes.Lifespan;

using Bogus;
using Domain.Lifespans;
using Services;

public class FakeLifespan
{
    public static Lifespan Generate(DateOnly dob, IDateTimeProvider dateTimeProvider)
    {
        return new Lifespan(dob, dateTimeProvider);
    }
    public static Lifespan Generate(int age)
    {
        return new Lifespan(age);
    }
    public static Lifespan Generate(IDateTimeProvider dateTimeProvider)
    {
        var faker = new Faker();
        return new Lifespan(faker.Date.PastDateOnly(), dateTimeProvider);
    }
}