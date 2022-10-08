namespace PeakLims.Domain.PanelOrders;

using SharedKernel.Exceptions;
using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders.Validators;
using PeakLims.Domain.PanelOrders.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Panels;


public class PanelOrder : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string State { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Panel")]
    public virtual Guid? PanelId { get; private set; }
    public virtual Panel Panel { get; private set; }


    public static PanelOrder Create(PanelOrderForCreationDto panelOrderForCreationDto)
    {
        new PanelOrderForCreationDtoValidator().ValidateAndThrow(panelOrderForCreationDto);

        var newPanelOrder = new PanelOrder();

        newPanelOrder.State = panelOrderForCreationDto.State;
        newPanelOrder.PanelId = panelOrderForCreationDto.PanelId;

        newPanelOrder.QueueDomainEvent(new PanelOrderCreated(){ PanelOrder = newPanelOrder });
        
        return newPanelOrder;
    }

    public void Update(PanelOrderForUpdateDto panelOrderForUpdateDto)
    {
        new PanelOrderForUpdateDtoValidator().ValidateAndThrow(panelOrderForUpdateDto);

        State = panelOrderForUpdateDto.State;
        PanelId = panelOrderForUpdateDto.PanelId;

        QueueDomainEvent(new PanelOrderUpdated(){ Id = Id });
    }
    
    protected PanelOrder() { } // For EF + Mocking
}