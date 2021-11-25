using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class TestInputDto
    {
        public int Id { get; set; }
        public int TryCount { get; set; }
        public List<QuestionInputDto> Questions { get; set; }
    }
}