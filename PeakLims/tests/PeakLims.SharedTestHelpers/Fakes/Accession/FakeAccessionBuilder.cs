namespace PeakLims.SharedTestHelpers.Fakes.Accession;

using Domain.HealthcareOrganizationContacts;
using Domain.PanelOrders;
using Domain.Panels.Services;
using Domain.TestOrders;
using HealthcareOrganizationContact;
using Panel;
using PanelOrder;
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
    private readonly List<PanelOrder> _panelOrders = new List<PanelOrder>();
    private readonly List<TestOrder> _testOrders = new List<TestOrder>();
    private bool _includeAPanelOrder = true;
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
    
    public FakeAccessionBuilder WithPanelOrder(PanelOrder panelOrder)
    {
        _panelOrders.Add(panelOrder);
        return this;
    }
    
    public FakeAccessionBuilder ExcludePanelOrders()
    {
        _includeAPanelOrder = false;
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
    
    public Accession Build(IPanelRepository panelRepository)
    {
        var accession = Accession.Create(_accessionData);
        
        if(_contacts.Count <= 0 && _includeAContact)
            _contacts.Add(FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
                .RuleFor(x => x.HealthcareOrganizationId, _accessionData.HealthcareOrganizationId)
                .Generate()));
        
        if (_panelOrders.Count <= 0 && _includeAPanelOrder)
        {
            var fakeTest = FakeTest.GenerateActivated();
            var fakePanel = new FakePanelBuilder()
                .WithRepository(panelRepository)
                .Build();
            fakePanel.AddTest(fakeTest);
            var fakePanelOrder = FakePanelOrder.Generate();
            fakePanelOrder.SetPanel(fakePanel);
            _panelOrders.Add(fakePanelOrder);
        }
        
        if (_testOrders.Count <= 0 && _includeATestOrder)
        {
            var fakeTest = FakeTest.GenerateActivated();
            var fakeTestOrder = FakeTestOrder.Generate();
            fakeTestOrder.SetTest(fakeTest);
            _testOrders.Add(fakeTestOrder);
        }
        
        foreach (var contact in _contacts)
        {
            accession.AddContact(contact);
        }
        foreach (var panelOrder in _panelOrders)
        {
            accession.AddPanelOrder(panelOrder);
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