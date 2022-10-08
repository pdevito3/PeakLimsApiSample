namespace PeakLims.Domain.PanelOrders.Dtos;

public sealed class PanelOrderDto 
{
        public Guid Id { get; set; }
        public string Status { get; set; }
        public Guid? PanelId { get; set; }
}
