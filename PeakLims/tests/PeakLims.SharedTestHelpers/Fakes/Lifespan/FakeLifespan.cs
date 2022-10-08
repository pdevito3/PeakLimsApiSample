namespace PeakLims.SharedTestHelpers.Fakes.Lifespan;

using Bogus;
using Domain.Lifespans;

public class FakeLifespan
{
    public static Lifespan Generate(DateOnly dob)
    {
        return new Lifespan(dob);
    }
    public static Lifespan Generate(int age)
    {
        return new Lifespan(age);
    }
    public static Lifespan Generate()
    {
        var faker = new Faker();
        return new Lifespan(faker.Date.PastDateOnly());
    }
}