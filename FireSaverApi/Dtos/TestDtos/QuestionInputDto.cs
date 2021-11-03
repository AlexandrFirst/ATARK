using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class QuestionInputDto
    {
        public int Content { get; set; }
        public List<string> AnswearsList { get; set; }
        public List<string> PossibleAnswears { get; set; }
    }
}