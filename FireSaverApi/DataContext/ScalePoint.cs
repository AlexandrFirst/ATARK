namespace FireSaverApi.DataContext
{
    public class ScalePoint : Point
    {
        public string WorldPosition { get; set; }
        public ScaleModel ScaleModel { get; set; }
    }
}