namespace PeakLims.Domain.Samples.Dtos;

public abstract class SampleForManipulationDto : ContainerlessSampleForManipulationDto
{
        public Guid? ContainerId { get; set; }
}
