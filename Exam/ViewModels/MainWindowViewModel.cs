﻿using Exam.Commands;
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

            CurrentTimeSet  = 10.ToString();
            NextCommand     = new Command(_NextQuestion);
            TimeSetCommand  = new Command(_TimeSetCommand);
            Message.MessageVisibility = Visibility.Hidden;
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

        public Command TimeSetCommand { get; set; }

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

        private string _CurrentTimeSet;
        public string CurrentTimeSet
        {
            get => _CurrentTimeSet;
            set => this.RaiseAndSetIfChanged(ref _CurrentTimeSet, value);
        }


        #endregion

        #region Helper


        private void toOptions(string Source)
        {
            Source = 
                Source
                .Replace("A、", "A.")
                .Replace("B、", "B.")
                .Replace("C、", "C.")
                .Replace("D、", "D.")
                .Replace("A．", "A.")
                .Replace("B．", "B.")
                .Replace("C．", "C.")
                .Replace("D．", "D.")
                .Trim();

            int a = Source.IndexOf("A.");
            if (a < 0)
            {
                Option1.OptionString = "❌";
                Option1.OptionColor = Brushes.Red;
                Option2.OptionString = "✔";
                Option2.OptionColor = Brushes.Green;
            }
            else 
            {
                int b = Source.IndexOf("B.");
                Option1.OptionString = Source.Substring(a, b - a - 1);
                Option1.OptionColor = Brushes.LightBlue;

                int c = Source.IndexOf("C.");
                if (c < 0)
                {
                    Option2.OptionString = Source.Substring(b);
                    Option2.OptionColor = Brushes.LightBlue;
                }
                else
                {
                    Option2.OptionString = Source.Substring(b, c - b - 1);
                    Option2.OptionColor = Brushes.LightBlue;

                    int d = Source.IndexOf("D.");
                    if (d < 0)
                    {
                        Option3.OptionString = Source.Substring(c);
                        Option3.OptionVisiable = Visibility.Visible;
                    }
                    else
                    {
                        Option3.OptionString = Source.Substring(c, d - c - 1);
                        Option3.OptionVisiable = Visibility.Visible;

                        Option4.OptionString = Source.Substring(d);
                        Option4.OptionVisiable = Visibility.Visible;
                    }
                }
            }
        }

        private Random RD = new Random();
        private Thread TD;

        private void _NextQuestion()
        {
            TimeStop();
            Message.MessageVisibility = Visibility.Hidden;
            Option3.OptionVisiable = Visibility.Hidden;
            Option4.OptionVisiable = Visibility.Hidden;

            int currentNo = RD.Next(GetQuestions.Result.Count);
            CurrentQuestion = GetQuestions.Result[currentNo];

            TD = new Thread(new ThreadStart(() => TimeUp(int.Parse(CurrentTimeSet)*1000)));
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
            TimeStop();
            if (selectNo == CurrentQuestion.Answer)
            {
                Count.CountPass += 1;
                this.Message.MessageString = "回答正确!";
                this.Message.MessageColor = Brushes.Green;
            }
            else
            {
                Count.CountFail += 1;
                
                if(CurrentQuestion.Class == "判断题")
                {
                    this.Message.MessageString = "回答错误" + string.Format("，正确答案是 {0} !", CurrentQuestion.Answer == "A" ? "❌" : "✔");
                }
                else
                {
                    this.Message.MessageString = "回答错误" + string.Format("，正确答案是 {0} !", CurrentQuestion.Answer);
                }
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
            Count.CountAll += 1;
            Count.CountFail += 1;
        }
        private void TimeStop()
        {
            try
            {
                TD.Suspend();
            }
            catch (Exception)
            {
            }

        }
        private void _TimeSetCommand()
        {
            Views.TimeSetView TSV = new Views.TimeSetView(int.Parse(CurrentTimeSet));
            TimeStop();
            TSV.ShowDialog();
            CurrentTimeSet = TSV.TimeSet.ToString();
        }

        #endregion
    }
}
