namespace PeakLims.Domain.Panels.Dtos;

public abstract class PanelForManipulationDto 
{
        public string PanelNumber { get; set; }
        public string PanelCode { get; set; }
        public string PanelName { get; set; }
        public int TurnAroundTime { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }

}
