using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models.QRModel;
using FireSaverMobile.Models.TestCompartmentModels;
using FireSaverMobile.Resx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace FireSaverMobile.ViewModels
{
    public class CompartmentTestPageViewModel : BaseViewModel
    {
        private QrModel qrModel;
        private TestCompModel test;
        private AnswerListModel testAnswers = new AnswerListModel();

        private int currentQuestionIndex = 0;

        private QuestionOutputDto currentQuestion = null;
        public QuestionOutputDto CurrentQuestion
        {
            get { return currentQuestion; }
            set
            {
                SetValue(ref currentQuestion, value);
                OnPropertyChanged("IsFinalQuestion");
                OnPropertyChanged("TitleCaption");
            }
        }

        private bool isBusy = false;
        public bool IsBusy 
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }

        public bool IsFinalQuestion
        {
            get
            {
                if (test == null)
                    return false;
                return currentQuestionIndex == test.Questions.Count - 1;
            }
        }

        public LocalizedString TitleCaption
        {
            get
            {
                if (test == null)
                    return new LocalizedString(() => string.Format($"0/0"));

                return new LocalizedString(() => string.Format(AppResources.TestCaption, currentQuestionIndex + 1, test.Questions.Count));
            }
        }

        private IUserService userService;
        private ICompartmentEnterService compartmentService;
        private ILoginService loginService;

        public ICommand SendAnswers { get; set; }

        public ICommand AddAnswerToQuestion { get; set; }
        public ICommand RemoveAnswerFromQuestion { get; set; }

        public ICommand NextQuestion { get; set; }
        public ICommand PrevQuestion { get; set; }

        public EventHandler OnCompartmentSuccessEntering;
        public EventHandler<TestCompModel> OnTestRecieved;
        public EventHandler OnTestFailed;


        public CompartmentTestPageViewModel()
        {
            userService = TinyIOC.Container.Resolve<IUserService>();
            compartmentService = TinyIOC.Container.Resolve<ICompartmentEnterService>();
            loginService = TinyIOC.Container.Resolve<ILoginService>();

            SendAnswers = new Command(async () =>
            {
                IsBusy = true;
                var checkingResult = await compartmentService.SendTestAnswers(testAnswers);
                IsBusy = false;
                if (checkingResult != null)
                {
                    OnCompartmentSuccessEntering?.Invoke(null, null);
                }
                else
                {
                    OnTestFailed?.Invoke(null, null);
                }
            });

            AddAnswerToQuestion = new Command<string>((selectedVariant) =>
            {
                var questionAnswer = testAnswers.Answears[currentQuestionIndex];

                List<string> answers = questionAnswer.Answear.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();


                answers.Add(selectedVariant);
                var newAnswer = string.Join(",", answers);
                questionAnswer.Answear = newAnswer;
                questionAnswer.QuestionId = currentQuestion.Id;
            });

            RemoveAnswerFromQuestion = new Command<string>((selectedVariant) =>
            {
                var questionAnswer = testAnswers.Answears[currentQuestionIndex];
                List<string> answers = questionAnswer.Answear.Split(',').ToList();

                while (answers.Remove(selectedVariant)) ;

                var newAnswer = string.Join(",", answers);
                questionAnswer.Answear = newAnswer;
                questionAnswer.QuestionId = currentQuestion.Id;
            });

            NextQuestion = new Command(() =>
            {
                if (currentQuestionIndex >= testAnswers.Answears.Count - 1)
                    return;

                currentQuestionIndex++;
                InitCurrentQuestion();
            });

            PrevQuestion = new Command(() =>
            {
                if (currentQuestionIndex <= 0)
                    return;
                currentQuestionIndex--;
                InitCurrentQuestion();
            });
        }

        public async Task InitTest(QrModel qrModel)
        {
            this.qrModel = qrModel;

            IsBusy = true;
            test = await compartmentService.SendCompartmentData(new UserEnterCompartmentDto()
            {
                CompartmentId = qrModel.CompatrmentId.Value,
                IotId = qrModel.IOTId
            });
            isBusy = false;

            if (test == null)
            {
     
                OnCompartmentSuccessEntering?.Invoke(null, null);
            }
            else
            {
                testAnswers.TestId = test.Id;
                testAnswers.Answears = new List<AnswerListItem>();

                for (int i = 0; i < test.Questions.Count; i++) 
                {
                    testAnswers.Answears.Add(new AnswerListItem());
                }

                InitCurrentQuestion();
            }
        }

        private void InitCurrentQuestion()
        {
            CurrentQuestion = test.Questions[currentQuestionIndex];

            var checkedAnswers = test.Questions[currentQuestionIndex].CheckedAnswer;

            if (checkedAnswers.Count == 0)
            {
                var observableListItems = test.Questions[currentQuestionIndex]
                    .PossibleAnswears.Select(a => new ChekListItem() { Content = a, IsChecked = false }).ToList(); ;

                foreach (var item in observableListItems)
                {
                    CurrentQuestion.CheckedAnswer.Add(item);
                }
            }
        }
    }
}
