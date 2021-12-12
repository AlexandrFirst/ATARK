using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.Models.TestCompartmentModels
{
    public class AnswerListModel
    {
        public int TestId { get; set; }
        public List<AnswerListItem> Answears { get; set; }
    }
}
