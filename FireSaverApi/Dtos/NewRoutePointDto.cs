namespace FireSaverApi.Dtos
{
    public class NewRoutePointDto
    {
        public int? ParentRoutePointId { get; set; }
        public PositionDto PointPostion { get; set; }
    }
}