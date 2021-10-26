namespace FireSaverApi.DataContext
{
    public class Position
    {
        public int Id { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        // public IoT IotPostion { get; set; }
        // public Point PointPostion { get; set; }
        // public ScalePoint ScalePoint { get; set; }
        // public Building Building { get; set; }
        public User User { get; set; }
    }
}