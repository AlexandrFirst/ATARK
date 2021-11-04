namespace FireSaverApi.Dtos.IoTDtos
{
    public class IoTDataInfo
    {
        public double LastRecordedTemperature { get; set; }
        public double LastRecordedCO2Level { get; set; }
        public double LastRecordedAmmoniaLevel { get; set; }
        public double LastRecordedNitrogenOxidesLevel { get; set; }
        public double LastRecordedSmokeLevel { get; set; }
        public double LastRecordedPetrolLevel { get; set; }
    }
}