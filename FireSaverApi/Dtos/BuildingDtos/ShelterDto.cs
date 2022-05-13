namespace FireSaverApi.Dtos.BuildingDtos
{
    public class ShelterDto
    {
        public int Id { get; set; } = 0;
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string Info { get; set; }
        public PositionDto ShelterPosition { get; set; }
    }
}