namespace FireSaverApi.Dtos.IoTDtos
{
    public class IotNewPositionDto
    {
        public int Id { get; set; }
        public string IotIdentifier { get; set; }
        public PositionDto MapPosition { get; set; }

    }
}