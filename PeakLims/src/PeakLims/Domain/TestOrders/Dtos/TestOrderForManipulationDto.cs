namespace PeakLims.Domain.TestOrders.Dtos;

public abstract class TestOrderForManipulationDto 
{
        public string Status { get; set; }
        public Guid? TestId { get; set; }
}
