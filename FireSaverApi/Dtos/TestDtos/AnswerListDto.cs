using System.Collections.Generic;

namespace FireSaverApi.Dtos.TestDtos
{
    public class AnswerListItem
    {
        public int QuestionId { get; set; }
        public string Answear { get; set; }
    }

    public class AnswerListDto
    {
        public int TestId { get; set; }
        public List<AnswerListItem> Answears { get; set; } 
    }
}