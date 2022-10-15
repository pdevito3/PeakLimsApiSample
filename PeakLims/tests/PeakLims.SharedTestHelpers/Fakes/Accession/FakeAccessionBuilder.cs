namespace PeakLims.SharedTestHelpers.Fakes.Accession;

using Domain.HealthcareOrganizationContacts;
using Domain.Panels;
using Domain.TestOrders;
using Domain.Tests.Services;
using HealthcareOrganizationContact;
using Moq;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;
using Test;
using TestOrder;

public class FakeAccessionBuilder :
    IPatientSelectionStage,
    IOrganizationSelectionStage,
    ITestRepositorySetterStage
{
    private AccessionForCreationDto _accessionData = new FakeAccessionForCreationDto().Generate();
    private readonly List<HealthcareOrganizationContact> _contacts = new List<HealthcareOrganizationContact>();
    private readonly List<Panel> _panels = new List<Panel>();
    private readonly List<TestOrder> _testOrders = new List<TestOrder>();
    private bool _includeATestOrder = true;
    private bool _includeAContact = true;
    private Guid? _patientId;
    private Guid? _orgId;
    private ITestRepository _testRepository = null;

    private FakeAccessionBuilder() { }
    
    public static IPatientSelectionStage Initialize() => new FakeAccessionBuilder();

    public FakeAccessionBuilder WithDto(AccessionForCreationDto data)
    {
        _accessionData = data;
        return this;
    }
    
    public IOrganizationSelectionStage WithPatientId(Guid patientId)
    {
        _patientId = patientId;
        return this;
    }
    
    public ITestRepositorySetterStage WithHealthcareOrganizationId(Guid orgId)
    {
        _orgId = orgId;
        return this;
    }
    
    public FakeAccessionBuilder WithContact(HealthcareOrganizationContact contact)
    {
        _contacts.Add(contact);
        return this;
    }
    
    public FakeAccessionBuilder WithPanel(Panel panel)
    {
        _panels.Add(panel);
        return this;
    }
    
    public FakeAccessionBuilder WithTestOrder(TestOrder testOrder)
    {
        _testOrders.Add(testOrder);
        return this;
    }
    
    public FakeAccessionBuilder ExcludeTestOrders()
    {
        _includeATestOrder = false;
        return this;
    }
    
    public FakeAccessionBuilder ExcludeContacts()
    {
        _includeAContact = false;
        return this;
    }
    
    public FakeAccessionBuilder ExcludePatient()
    {
        _patientId = null;
        return this;
    }
    
    public FakeAccessionBuilder ExcludeOrg()
    {
        _orgId = null;
        ExcludeContacts();
        return this;
    }
    
    public FakeAccessionBuilder WithTestRepository(ITestRepository testRepository)
    {
        _testRepository = testRepository;
        return this;
    }
    
    public FakeAccessionBuilder WithMockTestRepository(bool testExists = false)
    {
        var mockTestRepository = new Mock<ITestRepository>();
        mockTestRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(testExists);
        
        _testRepository = mockTestRepository.Object;
        return this;
    }
    
    public Accession Build()
    {
        _accessionData.PatientId = _patientId;
        _accessionData.HealthcareOrganizationId = _orgId;
        var accession = Accession.Create(_accessionData);
        
        if(_contacts.Count <= 0 && _includeAContact)
            _contacts.Add(FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
                .RuleFor(x => x.HealthcareOrganizationId, _accessionData.HealthcareOrganizationId)
                .Generate()));

        if (_testOrders.Count <= 0 && _includeATestOrder)
        {
            var fakeTest = new FakeTestBuilder()
                .WithRepository(_testRepository)
                .Activate()
                .Build();
            var fakeTestOrder = TestOrder.Create(fakeTest);
            _testOrders.Add(fakeTestOrder);
        }
        
        foreach (var contact in _contacts)
        {
            accession.AddContact(contact);
        }
        foreach (var panel in _panels)
        {
            accession.AddPanel(panel);
        }
        foreach (var testOrder in _testOrders)
        {
            accession.AddTestOrder(testOrder);
        }

        return accession;
    }
}

public interface IPatientSelectionStage
{
    public IOrganizationSelectionStage WithPatientId(Guid patientId);
}

public interface ITestRepositorySetterStage
{
    public FakeAccessionBuilder WithTestRepository(ITestRepository testRepository);
    public FakeAccessionBuilder WithMockTestRepository(bool testExists = false);
}

public interface IOrganizationSelectionStage
{
    public ITestRepositorySetterStage WithHealthcareOrganizationId(Guid orgId);
}