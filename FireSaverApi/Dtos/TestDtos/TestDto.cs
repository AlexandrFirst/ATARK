using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class TestDto
    {
        public int TryCount { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}