using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ReactiveUI;

namespace Exam.ViewModels
{
    class MessageViewModel : ReactiveObject
    {
        private string _MessageString;
        public string MessageString
        {
            get => _MessageString;
            set => this.RaiseAndSetIfChanged(ref _MessageString, value);
        }

        private Visibility _MessageVisibility;
        public Visibility MessageVisibility
        {
            get => _MessageVisibility;
            set => this.RaiseAndSetIfChanged(ref _MessageVisibility, value);
        }

        private Brush _MessageColor;
        public Brush MessageColor
        {
            get => _MessageColor;
            set => this.RaiseAndSetIfChanged(ref _MessageColor, value);
        }
    }
}
