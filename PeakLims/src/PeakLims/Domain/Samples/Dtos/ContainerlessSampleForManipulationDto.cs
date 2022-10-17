namespace PeakLims.Domain.Samples.Dtos;

public abstract class ContainerlessSampleForManipulationDto 
{
        public string Type { get; set; }
        public decimal? Quantity { get; set; }
        public DateOnly? CollectionDate { get; set; }
        public DateOnly? ReceivedDate { get; set; }
        public string CollectionSite { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? ParentSampleId { get; set; }
}
