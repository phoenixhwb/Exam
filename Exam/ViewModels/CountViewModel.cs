using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.ViewModels
{
    class CountViewModel : ReactiveObject
    {
        private int _CountAll;
        public int CountAll
        {
            get => _CountAll;
            set => this.RaiseAndSetIfChanged(ref _CountAll, value);
        }

        private int _CountPass;
        public int CountPass
        {
            get => _CountPass;
            set => this.RaiseAndSetIfChanged(ref _CountPass, value);
        }

        private int _CountFail;
        public int CountFail
        {
            get => _CountFail;
            set => this.RaiseAndSetIfChanged(ref _CountFail, value);
        }

        public CountViewModel()
        {
            this.CountAll  = 0;
            this.CountFail = 0;
            this.CountPass = 0;
        }
    }
}
