using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.TestCompartmentModels
{
    public class TestCompModel
    {
        public int Id { get; set; }
        public IList<QuestionOutputDto> Questions { get; set; }
    }
}
