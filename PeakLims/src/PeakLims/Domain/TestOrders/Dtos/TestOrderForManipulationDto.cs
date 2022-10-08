namespace PeakLims.Domain.TestOrders.Dtos;

public abstract class TestOrderForManipulationDto 
{
        public string State { get; set; }
        public Guid? TestId { get; set; }
}
