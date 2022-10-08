namespace PeakLims.UnitTests.UnitTests.Domain.Users.Features;

using PeakLims.SharedTestHelpers.Fakes.User;
using PeakLims.Domain.Users;
using PeakLims.Domain.Users.Dtos;
using PeakLims.Domain.Users.Mappings;
using PeakLims.Domain.Users.Features;
using PeakLims.Domain.Users.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetUserListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetUserListTests()
    {
        _userRepository = new Mock<IUserRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_user()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate();
        var fakeUserTwo = FakeUser.Generate();
        var fakeUserThree = FakeUser.Generate();
        var user = new List<User>();
        user.Add(fakeUserOne);
        user.Add(fakeUserTwo);
        user.Add(fakeUserThree);
        var mockDbData = user.AsQueryable().BuildMock();
        
        var queryParameters = new UserParametersDto() { PageSize = 1, PageNumber = 2 };

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }
}