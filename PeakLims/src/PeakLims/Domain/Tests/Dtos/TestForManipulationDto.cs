namespace PeakLims.Domain.Tests.Dtos;

public abstract class TestForManipulationDto 
{
        public int TurnAroundTime { get; set; }
        public string TestName { get; set; }
        public string Methodology { get; set; }
        public string Platform { get; set; }
        public int Version { get; set; }

}
