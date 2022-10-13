namespace PeakLims.Domain.Panels;

using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels.Validators;
using PeakLims.Domain.Panels.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Tests;
using Services;
using ValidationException = SharedKernel.Exceptions.ValidationException;

public class Panel : BaseEntity
{

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string PanelCode { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string PanelName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int TurnAroundTime { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Type { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int Version { get; private set; }

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
        newPanel.TurnAroundTime = panelForCreationDto.TurnAroundTime;
        newPanel.Type = panelForCreationDto.Type;
        newPanel.Version = panelForCreationDto.Version;

        newPanel.QueueDomainEvent(new PanelCreated(){ Panel = newPanel });
        
        return newPanel;
    }

    public void Update(PanelForUpdateDto panelForUpdateDto)
    {
        new PanelForUpdateDtoValidator().ValidateAndThrow(panelForUpdateDto);

        PanelName = panelForUpdateDto.PanelName;
        TurnAroundTime = panelForUpdateDto.TurnAroundTime;
        Type = panelForUpdateDto.Type;
        Version = panelForUpdateDto.Version;

        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }

    public static void GuardWhenExists(string panelCode, int version, IPanelRepository panelRepository)
    {
        if (Exists(panelCode, version, panelRepository))
            throw new ValidationException(nameof(Test),
                $"A panel with the given panel code ('{panelCode}') and version ('{version}') already exists.");
    }

    public static bool Exists(string panelCode, int version, IPanelRepository panelRepository) => panelRepository.Exists(panelCode, version);

    public void AddTest(Test test)
    {        
        Tests.Add(test);
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }

    public void RemoveTest(Test test)
    {        
        Tests.Remove(test);
        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }
    
    protected Panel() { } // For EF + Mocking
}