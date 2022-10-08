namespace PeakLims.UnitTests.UnitTests.Domain.Lifespans;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Lifespans;

[Parallelizable]
public class LifespanTests
{
    private readonly Faker _faker;

    public LifespanTests()
    {
        _faker = new Faker();
    }
    
    [TestCase(-18, 0)]
    [TestCase(-2250, 6)]
    [TestCase(-365, 1)]
    [TestCase(-364, 0)]
    [TestCase(0, 0)]
    public void can_get_lifespan_from_dateonly(int daysOld, int age)
    {
        var dob = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(daysOld);
        var lifespan = new Lifespan(dob);

        lifespan.DateOfBirth.Should().Be(dob);
        lifespan.Age.Should().Be(age);
        lifespan.GetAgeInDays().Should().Be(daysOld*-1);
    }
    
    [TestCase(0, 0)]
    [TestCase(6, 6)]
    [TestCase(1, 1)]
    [TestCase(0, 0)]
    public void can_get_lifespan_from_age(int inputAge, int expectedAge)
    {
        var lifespan = new Lifespan(inputAge);

        lifespan.Age.Should().Be(expectedAge);
        lifespan.DateOfBirth.Should().Be(null);
        lifespan.GetAgeInDays().Should().Be(null);
    }
    
    [Test]
    public void dob_trumps_age()
    {
        var validYearsOld = _faker.Random.Int(min: 0, max: 120);
        var invalidYearsOld = _faker.Random.Int(min: 0, max: 120);
        var dob = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-validYearsOld);
        var lifespan = new Lifespan(invalidYearsOld, dob);

        lifespan.DateOfBirth.Should().Be(dob);
        lifespan.Age.Should().Be(validYearsOld);
    }
    
    [Test]
    public void can_not_be_less_than_0_years_using_dateonly()
    {
        var dob = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
        var lifespan = () => new Lifespan(dob);
        lifespan.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
    
    [Test]
    public void can_not_be_more_than_120_years_using_dateonly()
    {
        var dob = DateOnly.FromDateTime(DateTime.UtcNow).AddYears(120);
        var lifespan = () => new Lifespan(dob);
        lifespan.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
    
    [Test]
    public void can_not_be_less_than_0_years_using_age()
    {
        var lifespan = () => new Lifespan(-1);
        lifespan.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
    
    [Test]
    public void can_not_be_more_than_120_years_using_age()
    {
        var lifespan = () => new Lifespan(121);
        lifespan.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}