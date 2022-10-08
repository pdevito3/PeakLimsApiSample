namespace PeakLims.Domain.TestOrders.Dtos;

public sealed class TestOrderDto 
{
        public Guid Id { get; set; }
        public string State { get; set; }
        public Guid? TestId { get; set; }
}
