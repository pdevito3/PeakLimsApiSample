namespace PeakLims.Domain.Containers.Dtos;

public sealed class ContainerDto 
{
        public Guid Id { get; set; }
        public string ContainerNumber { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
}
