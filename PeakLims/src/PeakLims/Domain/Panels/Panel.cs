namespace PeakLims.Domain.Panels;

using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels.Validators;
using PeakLims.Domain.Panels.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using PanelStatuses;
using Sieve.Attributes;
using PeakLims.Domain.Tests;
using Services;
using TestOrders.Services;
using ValidationException = SharedKernel.Exceptions.ValidationException;

public class Panel : BaseEntity
{

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string PanelCode { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string PanelName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Type { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int Version { get; private set; }
    
    public virtual PanelStatus Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<Test> Tests { get; private set; } = new List<Test>();


    public static Panel Create(PanelForCreationDto panelForCreationDto, IPanelRepository panelRepository)
    {
        new PanelForCreationDtoValidator().ValidateAndThrow(panelForCreationDto);
        GuardWhenExists(panelForCreationDto.PanelCode, panelForCreationDto.Version, panelRepository);

        var newPanel = new Panel();

        newPanel.PanelCode = panelForCreationDto.PanelCode;
        newPanel.PanelName = panelForCreationDto.PanelName;
        newPanel.Type = panelForCreationDto.Type;
        newPanel.Version = panelForCreationDto.Version;
        newPanel.Status = PanelStatus.Draft();

        newPanel.QueueDomainEvent(new PanelCreated(){ Panel = newPanel });
        
        return newPanel;
    }

    public void Update(PanelForUpdateDto panelForUpdateDto, IPanelRepository panelRepository)
    {
        new PanelForUpdateDtoValidator().ValidateAndThrow(panelForUpdateDto);
        GuardWhenExists(PanelCode, panelForUpdateDto.Version, panelRepository);

        PanelName = panelForUpdateDto.PanelName;
        Type = panelForUpdateDto.Type;
        Version = panelForUpdateDto.Version;

        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }

    public Panel Activate()
    {
        if (Status == PanelStatus.Active())
            return this;
        
        Status = PanelStatus.Active();
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
        return this;
    }

    public Panel Deactivate()
    {
        if (Status == PanelStatus.Inactive())
            return this;
        
        Status = PanelStatus.Inactive();
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
        return this;
    }

    public static void GuardWhenExists(string panelCode, int version, IPanelRepository panelRepository)
    {
        if (Exists(panelCode, version, panelRepository))
            throw new ValidationException(nameof(Panel),
                $"A panel with the given panel code ('{panelCode}') and version ('{version}') already exists.");
    }

    public static bool Exists(string panelCode, int version, IPanelRepository panelRepository) => panelRepository.Exists(panelCode, version);

    public void AddTest(Test test, ITestOrderRepository testOrderRepository)
    {
        GuardWhenPanelIsAssignedToAnAccession(testOrderRepository);
        Tests.Add(test);
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }

    public void RemoveTest(Test test, ITestOrderRepository testOrderRepository)
    {
        GuardWhenPanelIsAssignedToAnAccession(testOrderRepository);
        Tests.Remove(test);
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }

    private void GuardWhenPanelIsAssignedToAnAccession(ITestOrderRepository testOrderRepository)
    {
        if (testOrderRepository.HasPanelAssignedToAccession(this))
            throw new ValidationException(nameof(Panel),
                $"This panel has been assigned to one or more accessions. Tests can not be updated on a panel when the associated panel is in use.");
    }

    protected Panel() { } // For EF + Mocking
}