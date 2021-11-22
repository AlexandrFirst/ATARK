namespace FireSaverApi.Dtos
{
    public class ScalePointDto
    {
        public int Id { get; set; }
        public PositionDto MapPosition { get; set; }
        public PositionDto WorldPosition { get; set; }
    }
}