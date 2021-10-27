namespace FireSaverApi.DataContext
{
    public class ScalePoint : Point
    {
        public Position WorldPosition { get; set; }
        public int? WorldPositionId { get; set; }
        public ScaleModel ScaleModel { get; set; }
    }
}