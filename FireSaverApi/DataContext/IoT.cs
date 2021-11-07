namespace FireSaverApi.DataContext
{
    public class IoT //gas detector is MQ-135
    {
        public int Id { get; set; }
        public string MapPosition { get; set; }
        public string IotIdentifier { get; set; }
        public bool IsAuthNeeded { get; set; }
        public double LastRecordedTemperature { get; set; }
        public double LastRecordedCO2Level { get; set; }
        public double LastRecordedAmmoniaLevel { get; set; }
        public double LastRecordedNitrogenOxidesLevel { get; set; }
        public double LastRecordedSmokeLevel { get; set; }
        public double LastRecordedPetrolLevel { get; set; }
        public Compartment Compartment { get; set; }
    }
}