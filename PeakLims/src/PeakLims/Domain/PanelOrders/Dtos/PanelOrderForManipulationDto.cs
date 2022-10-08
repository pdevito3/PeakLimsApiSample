namespace PeakLims.Domain.PanelOrders.Dtos;

public abstract class PanelOrderForManipulationDto 
{
        public string Status { get; set; }
        public Guid? PanelId { get; set; }
}
