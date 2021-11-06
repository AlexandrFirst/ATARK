using System;
using FireSaverApi.Dtos.ScaleModel;

namespace FireSaverApi.Dtos.EvacuationPlanDtos
{
    public class EvacuationPlanDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime UploadTime { get; set; }
        public ScaleModelDto ScaleModel { get; set; }
    }
}