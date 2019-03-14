using Prism.Events;
using System;
using System.Collections.Generic;
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
            TestCommand = new ActionCommand(Test);
        }
        private void Test()
        {
            GetRegionEvent<DocumentOpenedEvent>().Publish("Document Opened");
            Console.WriteLine("test");
        }

        public ICommand TestCommand { get; set; }
    }

}
