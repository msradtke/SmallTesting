using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmallTesting.RegionEvents
{
    public class ChildRegionViewModel : ViewModelBase
    {
        public ChildRegionViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            Child = new GrandChildRegionViewModel(EventAggregator);
            Wire.RegisterChild(Child);
            GetRegionEvent<FakeEvent>().Subscribe(Fake);
            TestCommand = new ActionCommand(Test);
        }

        private void Fake(string obj)
        {
            Console.WriteLine("Should listen " + obj);
        }

        private void Test()
        {
            
            GetRegionEvent<DocumentOpenedEvent>().Publish("Document Opened");
            Console.WriteLine("test");
        }
        public ViewModelBase Child { get; set; }
        public ICommand TestCommand { get; private set; }
    }

}
