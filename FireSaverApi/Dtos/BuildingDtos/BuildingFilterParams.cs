using FireSaverApi.Helpers.Pagination;

namespace FireSaverApi.Dtos.BuildingDtos
{
    public class BuildingFilterParams : PageParams
    {
        public int BuildingId { get; set; } = 0;
        public string Address { get; set; } = string.Empty;
    }
}