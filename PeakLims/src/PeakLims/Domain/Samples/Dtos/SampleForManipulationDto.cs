namespace PeakLims.Domain.Samples.Dtos;

public abstract class SampleForManipulationDto 
{
        public string SampleNumber { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public decimal? Quantity { get; set; }
        public DateOnly? CollectionDate { get; set; }
        public DateOnly? ReceivedDate { get; set; }
        public string CollectionSite { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? ParentSampleId { get; set; }
        public Guid? ContainerId { get; set; }
}
