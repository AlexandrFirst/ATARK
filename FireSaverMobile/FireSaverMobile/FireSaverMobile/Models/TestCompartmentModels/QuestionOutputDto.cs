using FireSaverMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace FireSaverMobile.Models.TestCompartmentModels
{

    public class ChekListItem : BaseViewModel
    {
        public string Content { get; set; }

        private bool isChecked = false;
        public bool IsChecked { get { return isChecked; } set { SetValue(ref isChecked, value); } }
    }

    public class QuestionOutputDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<string> PossibleAnswears { get; set; }

        [JsonIgnore]
        public ObservableCollection<ChekListItem> CheckedAnswer { get; set; } = new ObservableCollection<ChekListItem>();
    }
}
