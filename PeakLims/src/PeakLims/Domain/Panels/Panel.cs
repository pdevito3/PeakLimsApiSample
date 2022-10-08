namespace PeakLims.Domain.Panels;

using SharedKernel.Exceptions;
using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels.Validators;
using PeakLims.Domain.Panels.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Tests;


public class Panel : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string PanelNumber { get; private set; }

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
    public virtual ICollection<Test> Tests { get; private set; }


    public static Panel Create(PanelForCreationDto panelForCreationDto)
    {
        new PanelForCreationDtoValidator().ValidateAndThrow(panelForCreationDto);

        var newPanel = new Panel();

        newPanel.PanelNumber = panelForCreationDto.PanelNumber;
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

        PanelNumber = panelForUpdateDto.PanelNumber;
        PanelCode = panelForUpdateDto.PanelCode;
        PanelName = panelForUpdateDto.PanelName;
        TurnAroundTime = panelForUpdateDto.TurnAroundTime;
        Type = panelForUpdateDto.Type;
        Version = panelForUpdateDto.Version;

        QueueDomainEvent(new PanelUpdated(){ Id = Id });
    }
    
    protected Panel() { } // For EF + Mocking
}