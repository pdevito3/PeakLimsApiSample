namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using Domain.Tests.Services;
using Domain.TestStatuses;
using Domain.Tests;
using Moq;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

public class FakeTestBuilder
{
    private TestForCreationDto _testData = new FakeTestForCreationDto().Generate();
    private ITestRepository _testRepository = null;
    private TestStatus _status;

    public FakeTestBuilder WithDto(TestForCreationDto testDto)
    {
        _testData = testDto;
        return this;
    }
    
    public FakeTestBuilder WithRepository(ITestRepository testRepository)
    {
        _testRepository = testRepository;
        return this;
    }
    
    public FakeTestBuilder WithMockRepository(bool testExists = false)
    {
        var mockTestRepository = new Mock<ITestRepository>();
        mockTestRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(testExists);
        
        _testRepository = mockTestRepository.Object;
        return this;
    }

    public FakeTestBuilder Activate()
    {
        _status = TestStatus.Active();
        return this;
    }

    public FakeTestBuilder Deactivate()
    {
        _status = TestStatus.Inactive();
        return this;
    }
    
    public Test Build()
    {
        if (_testRepository == null)
            throw new Exception("A test repository must be provided");

        var test = Test.Create(_testData, _testRepository);
        if (_status == TestStatus.Inactive())
            test.Deactivate();
        if (_status == TestStatus.Active())
            test.Activate();

        return test;
    }
}