using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmallTesting.RegionEvents
{
    public class GrandChildRegionViewModel : ViewModelBase
    {
        public GrandChildRegionViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            GetRegionEvent<DocumentOpenedEvent>().Subscribe(DoNotListen);

            TestCommand = new ActionCommand(Fake);
        }

        private void Fake()
        {
            GetRegionEvent<FakeEvent>().Publish("Fake");
        }

        public ICommand TestCommand { get; set; }
        private void DoNotListen(string obj)
        {
            Console.WriteLine("Should NOT listen " + obj);
        }
    }
}
