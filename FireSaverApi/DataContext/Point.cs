namespace FireSaverApi.DataContext
{
    public class Point
    {
        public int Id { get; set; }
        public Position MapPosition { get; set; }
        public Position WorldPosition { get; set; }
    }
}