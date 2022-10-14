namespace PeakLims.SharedTestHelpers.Fakes.Accession;

using Domain.HealthcareOrganizationContacts;
using Domain.Panels;
using Domain.TestOrders;
using HealthcareOrganizationContact;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;
using Test;
using TestOrder;

public class FakeAccessionBuilder :
    IPatientSelectionStage,
    IOrganizationSelectionStage
{
    private AccessionForCreationDto _accessionData = new FakeAccessionForCreationDto().Generate();
    private readonly List<HealthcareOrganizationContact> _contacts = new List<HealthcareOrganizationContact>();
    private readonly List<Panel> _panels = new List<Panel>();
    private readonly List<TestOrder> _testOrders = new List<TestOrder>();
    private bool _includeATestOrder = true;
    private bool _includeAContact = true;

    private FakeAccessionBuilder() { }
    
    public static IPatientSelectionStage Initialize() => new FakeAccessionBuilder();

    public IPatientSelectionStage WithDto(AccessionForCreationDto data)
    {
        _accessionData = data;
        return this;
    }
    
    public IOrganizationSelectionStage WithPatientId(Guid patientId)
    {
        _accessionData.PatientId = patientId;
        return this;
    }
    
    public FakeAccessionBuilder WithHealthcareOrganizationId(Guid orgId)
    {
        _accessionData.HealthcareOrganizationId = orgId;
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
        _accessionData.PatientId = null;
        return this;
    }
    
    public FakeAccessionBuilder ExcludeOrg()
    {
        _accessionData.HealthcareOrganizationId = null;
        ExcludeContacts();
        return this;
    }
    
    public Accession Build()
    {
        var accession = Accession.Create(_accessionData);
        
        if(_contacts.Count <= 0 && _includeAContact)
            _contacts.Add(FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
                .RuleFor(x => x.HealthcareOrganizationId, _accessionData.HealthcareOrganizationId)
                .Generate()));

        if (_testOrders.Count <= 0 && _includeATestOrder)
        {
            var fakeTest = FakeTest.GenerateActivated();
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

public interface IOrganizationSelectionStage
{
    public FakeAccessionBuilder WithHealthcareOrganizationId(Guid orgId);
}