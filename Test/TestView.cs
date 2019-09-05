using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.Commands;
using ReactiveUI;

namespace Test
{
    class TestView :ReactiveObject
    {
        private string _TB1;

        public string TB1
        {
            get { return _TB1; }
            set
            {
                this.RaiseAndSetIfChanged(ref _TB1, value);
            }
        }

        public Command CMD { set; get; }

        public TestView()
        {
            TB1 = "dsfafafa";
            CMD = new Command(() =>
            {
                TB1 = "adfasdfadf";
                Console.WriteLine("CMD");
            });
        }
    }
}
