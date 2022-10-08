namespace PeakLims.Domain.Containers.Dtos;

public abstract class ContainerForManipulationDto 
{
        public string ContainerNumber { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
}
