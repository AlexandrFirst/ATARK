using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class QuestionOutputDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<string> PossibleAnswears { get; set; }
    }
}