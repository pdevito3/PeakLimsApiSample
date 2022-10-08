namespace PeakLims.Domain.TestOrders.Dtos;

public sealed class TestOrderDto 
{
        public Guid Id { get; set; }
        public string Status { get; set; }
        public Guid? TestId { get; set; }
}
