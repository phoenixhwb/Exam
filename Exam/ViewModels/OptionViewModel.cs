using Exam.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Exam.ViewModels
{
    class OptionViewModel : ReactiveObject
    {
        public Command OptionCommand { get; set; }

        private string _OptionString;
        public string OptionString
        {
            get => _OptionString;
            set => this.RaiseAndSetIfChanged(ref _OptionString, value);
        }

        private Visibility _OptionVisiable;
        public Visibility OptionVisiable
        {
            get => _OptionVisiable;
            set => this.RaiseAndSetIfChanged(ref _OptionVisiable, value);
        }

        private Brush _OptionColor;
        public Brush OptionColor
        {
            get => _OptionColor;
            set => this.RaiseAndSetIfChanged(ref _OptionColor, value);
        }

        public OptionViewModel(Action A)
        {
            OptionColor = Brushes.LightBlue;
            OptionCommand = new Command(A);
        }
    }
}
