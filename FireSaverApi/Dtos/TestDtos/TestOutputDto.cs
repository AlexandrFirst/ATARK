using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class TestOutputDto
    {
        public int Id { get; set; }
        public IList<QuestionOutputDto> Questions { get; set; }
    }
}