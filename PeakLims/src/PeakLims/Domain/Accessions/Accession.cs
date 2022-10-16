namespace PeakLims.Domain.Accessions;

using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions.DomainEvents;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AccessionStatuses;
using Panels;
using Sieve.Attributes;
using PeakLims.Domain.Patients;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.AccessionComments;
using PeakLims.Services;
using SharedKernel.Exceptions;
using TestOrders.Services;
using Tests;

public class Accession : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string AccessionNumber { get; }

    public virtual AccessionStatus Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Patient")]
    public virtual Guid? PatientId { get; private set; }
    public virtual Patient Patient { get; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("HealthcareOrganization")]
    public virtual Guid? HealthcareOrganizationId { get; private set; }
    public virtual HealthcareOrganization HealthcareOrganization { get; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<HealthcareOrganizationContact> Contacts { get; } = new List<HealthcareOrganizationContact>();

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<TestOrder> TestOrders { get; private set; } = new List<TestOrder>();

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<AccessionComment> Comments { get; private set; } = new List<AccessionComment>();

    public static Accession Create(AccessionForCreationDto accessionForCreationDto)
    {
        var newAccession = new Accession();

        newAccession.Status = AccessionStatus.Draft();
        newAccession.PatientId = accessionForCreationDto.PatientId;
        newAccession.HealthcareOrganizationId = accessionForCreationDto.HealthcareOrganizationId;

        newAccession.QueueDomainEvent(new AccessionCreated(){ Accession = newAccession });
        
        return newAccession;
    }

    public Accession Update(AccessionForUpdateDto accessionForUpdateDto)
    {
        if (Status != AccessionStatus.Draft()) return this;
        
        PatientId = accessionForUpdateDto.PatientId;
        HealthcareOrganizationId = accessionForUpdateDto.HealthcareOrganizationId;
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession SetStatusToReadyForTesting(IDateTimeProvider dateTimeProvider)
    {
        new ValidationException(nameof(Accession),
                $"A patient is required in order to set an accession to {AccessionStatus.ReadyForTesting().Value}")
            .ThrowWhenNullOrEmpty(PatientId);
        new ValidationException(nameof(Accession),
                $"An organization is required in order to set an accession to {AccessionStatus.ReadyForTesting().Value}")
            .ThrowWhenNullOrEmpty(HealthcareOrganizationId);
        if (TestOrders.Count <= 0)
            throw new ValidationException(nameof(Accession),
                $"At least 1 panel or test is required in order to set an accession to {AccessionStatus.ReadyForTesting().Value}");
        if (Contacts.Count <= 0)
            throw new ValidationException(nameof(Accession),
                $"At least 1 organization contact is required in order to set an accession to {AccessionStatus.ReadyForTesting().Value}");
        
        // TODO unit test
        if (Status != AccessionStatus.Draft())
            throw new ValidationException(nameof(Accession),
                $"Test orders in a '{Status?.Value}' state can not be set to '{AccessionStatus.ReadyForTesting().Value}'");

        Status = AccessionStatus.ReadyForTesting();
        
        foreach (var testOrder in TestOrders)
        {
            testOrder.SetStatusToReadyForTesting(dateTimeProvider);
        }

        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession AddTest(Test test)
    {
        // TODO unit test
        GuardIfInFinalState("Tests");
        
        var hasNonActiveTests = !test.Status.IsActive();
        if(hasNonActiveTests)
            throw new ValidationException(nameof(Accession),
                $"This test is not active. Only active tests can be added to an accession.");

        var testOrder = TestOrder.Create(test);
        TestOrders.Add(testOrder);
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession RemoveTestOrder(TestOrder testOrder)
    {
        var alreadyExists = TestOrders.Any(x => testOrder.Id == x.Id);
        if (!alreadyExists)
            return this;
        
        if(testOrder.IsPartOfPanel())
            throw new ValidationException(nameof(Accession),
                $"Test orders that are part of a panel can not be selectively removed.");
        
        RemoveTestOrderForTestOrPanel(testOrder);
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    private void RemoveTestOrderForTestOrPanel(TestOrder testOrder)
    {
        // TODO unit test
        GuardIfInFinalState("Test orders");

        // TODO if test order status is not in one of the pending states, guard

        TestOrders.Remove(testOrder);
    }

    public Accession AddPanel(Panel panel)
    {
        // TODO unit test
        GuardIfInFinalState("Panels");
        
        var hasInactivePanel = !panel.Status.IsActive();
        if(hasInactivePanel)
            throw new ValidationException(nameof(Accession),
                $"This panel is not active. Only active panels can be added to an accession.");
        
        var hasNonActiveTests = panel.Tests.Any(x => !x.Status.IsActive());
        if(hasNonActiveTests)
            throw new ValidationException(nameof(Accession),
                $"This panel has one or more tests that are not active. Only active tests can be added to an accession.");

        // TODO unit test
        var hasNoTests = panel.Tests.Count == 0;
        if(hasNoTests)
            throw new ValidationException(nameof(Accession),
                $"This panel has no tests to assign.");
        
        foreach (var test in panel.Tests)
        {
            var testOrder = TestOrder.Create(test, panel);
            TestOrders.Add(testOrder);
        }
        
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession RemovePanel(Panel panel)
    {
        // TODO unit test
        GuardIfInFinalState("Panels");

        var alreadyExists = TestOrders.Any(x => panel.Id == x.AssociatedPanelId);
        if (!alreadyExists)
            return this;

        var testsToRemove = TestOrders.Where(x => x.AssociatedPanelId == panel.Id).ToList();
        foreach (var testOrder in testsToRemove)
        {
            RemoveTestOrderForTestOrPanel(testOrder);
        }
        
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession AddContact(HealthcareOrganizationContact contact)
    {
        var alreadyExists = HealthcareOrganizationContactAlreadyExists(contact);
        if (alreadyExists)
            return this;
        
        Contacts.Add(contact);
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    public Accession RemoveContact(HealthcareOrganizationContact contact)
    {
        var alreadyExists = HealthcareOrganizationContactAlreadyExists(contact);
        if (!alreadyExists)
            return this;
        
        Contacts.Remove(contact);
        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
        return this;
    }

    private bool HealthcareOrganizationContactAlreadyExists(HealthcareOrganizationContact contact) 
        => Contacts.Any(x => contact.Id == x.Id);

    private void GuardIfInFinalState(string subject)
    {
        if (Status.IsFinalState())
            throw new ValidationException(nameof(Accession),
                $"This accession is in a final state. {subject} can not be modified.");
    }
    
    protected Accession() { } // For EF + Mocking
}