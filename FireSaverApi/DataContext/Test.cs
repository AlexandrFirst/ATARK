using System.Collections.Generic;

namespace FireSaverApi.DataContext
{
    public class Test
    {
        public Test()
        {
            Questions = new List<Question>();
        }

        public int Id { get; set; }
        public int TryCount { get; set; }
        public IList<Question> Questions { get; set; }
        public Compartment Compartment { get; set; }
    }
}