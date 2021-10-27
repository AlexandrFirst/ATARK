using System;

namespace FireSaverApi.DataContext
{
    public class EvacuationPlan
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime UploadTime { get; set; }
        public ScaleModel ScaleModel { get; set; }
        public int? ScaleModelId { get; set; }
        public Compartment Compartment { get; set; }
    }
}