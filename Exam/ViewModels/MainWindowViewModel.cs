using Exam.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Exam.Models;
using Exam.Services;
using System.Windows.Media;
using System.Threading;

namespace Exam.ViewModels
{
    class MainWindowViewModel : ReactiveObject
    {
        #region Constructor
        public MainWindowViewModel()
        {
            Message = new MessageViewModel();
            Count = new CountViewModel();

            Option1 = new OptionViewModel(() => SelectAction("A"));
            Option2 = new OptionViewModel(() => SelectAction("B"));
            Option3 = new OptionViewModel(() => SelectAction("C"));
            Option4 = new OptionViewModel(() => SelectAction("D"));

            NextCommand = new Command(nextQuestion);
            nextQuestion();
        }
        #endregion

        #region Parameter
        public CountViewModel Count { get; set; }

        private MessageViewModel _Message;
        public MessageViewModel Message
        {
            get => _Message;
            set => this.RaiseAndSetIfChanged(ref _Message, value);
        }

        public Command NextCommand { set; get; }

        public OptionViewModel Option1 { set; get; }
        public OptionViewModel Option2 { set; get; }
        public OptionViewModel Option3 { set; get; }
        public OptionViewModel Option4 { set; get; }

        private Question _CurrentQuestion;
        public Question CurrentQuestion
        {
            get { return _CurrentQuestion; }
            set
            {
                _CurrentQuestion = value;
                this.StringContent = _CurrentQuestion.Content;
                this.Options = _CurrentQuestion.Options;
                switch(_CurrentQuestion.Answer)
                {
                    case "正确":
                        _CurrentQuestion.Answer = "A";
                        break;
                    case "错误":
                        _CurrentQuestion.Answer = "B";
                        break;
                    default:
                        break;
                }
            }
        }

        private string _StringContent;
        public string StringContent
        {
            get => _StringContent;
            set =>  this.RaiseAndSetIfChanged(ref _StringContent, value);
        }

        private string _Options;
        public string Options
        {
            get { return _Options; }
            set
            {
                _Options = value;
                toOptions(_Options);
            }
        }

        private int _TimeTickValue;
        public int TimeTickValue
        {
            get => _TimeTickValue;
            set => this.RaiseAndSetIfChanged(ref _TimeTickValue, value);
        }

        private int myVar;

        public int MyProperty
        {
            get => myVar;
            set => this.RaiseAndSetIfChanged(ref myVar, value);
        }


        #endregion

        #region Helper
        private void toOptions(string Source)
        {
            int a = Source.IndexOf("A.");
            int b = Source.IndexOf("B.");
            int c = Source.IndexOf("C.");
            int d = Source.IndexOf("D.");
            try
            {
                Option1.OptionString = Source.Substring(a, b - a - 1);
                Option1.OptionColor = Brushes.LightBlue;

                Option2.OptionString = Source.Substring(b, c - b - 1);
                Option2.OptionColor = Brushes.LightBlue;

                Option3.OptionString = Source.Substring(c, d - c - 1);
                Option3.OptionVisiable = Visibility.Visible;

                Option4.OptionString = Source.Substring(d);
                Option4.OptionVisiable = Visibility.Visible;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (a < 0)
                {
                    Option1.OptionString = "错误";
                    Option1.OptionColor = Brushes.Red;
                    Option2.OptionString = "正确";
                    Option2.OptionColor = Brushes.Green;
                }
            }
        }

        private Random rd = new Random();
        private Thread TD;

        private void nextQuestion()
        {
            Message.MessageVisibility = Visibility.Hidden;
            Option3.OptionVisiable = Visibility.Hidden;
            Option4.OptionVisiable = Visibility.Hidden;

            int currentNo = rd.Next(GetQuestions.Result.Count);
            CurrentQuestion = GetQuestions.Result[currentNo];

            TD = new Thread(new ThreadStart(() => TimeUp(10000)));
            TD.Start();

        }

        private void SelectAction(string selectNo)
        {
            Message.MessageVisibility = Visibility.Visible;
            Count.CountAll += 1;
            AnswerJudgement(selectNo);
        }
        private void AnswerJudgement(string selectNo)
        {
            try
            {
                TD.Suspend();
            }
            catch (Exception)
            {
            }
            if (selectNo == CurrentQuestion.Answer)
            {
                Count.CountPass += 1;
                this.Message.MessageString = "正确!";
                this.Message.MessageColor = Brushes.Green;
            }
            else
            {
                Count.CountFail += 1;
                this.Message.MessageString = string.Format("错误，正确答案是 {0} !",CurrentQuestion.Answer);
                this.Message.MessageColor = Brushes.Red;
            }
        }
        private void TimeUp(int time)
        {
            TimeTickValue = 100;
            int Step = time / 20;
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(Step);
                TimeTickValue -= 5;
            }
            Message.MessageVisibility = Visibility.Visible;
            this.Message.MessageString = "时间到!";
            this.Message.MessageColor = Brushes.Red;
            Count.CountFail += 1;
        }

        private void RenderProgress()
        {

        }
        #endregion
    }
}
