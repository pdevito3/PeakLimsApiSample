namespace PeakLims.Domain.PanelOrders.Dtos;

public abstract class PanelOrderForManipulationDto 
{
        public string State { get; set; }
        public Guid? PanelId { get; set; }
}
