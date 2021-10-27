namespace FireSaverApi.DataContext
{
    abstract public class Point
    {
        public int Id { get; set; }
        public Position MapPosition { get; set; }
        public int? MapPositionId { get; set; }
    }
}