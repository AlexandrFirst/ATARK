using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string AnswearsList { get; set; }
        public string PossibleAnswears { get; set; }
        public Test Test { get; set; }
    }
}