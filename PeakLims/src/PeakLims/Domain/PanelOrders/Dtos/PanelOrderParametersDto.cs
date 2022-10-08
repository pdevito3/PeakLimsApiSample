namespace PeakLims.Domain.PanelOrders.Dtos;

using SharedKernel.Dtos;

public sealed class PanelOrderParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
