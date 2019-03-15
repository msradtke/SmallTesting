using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.RegionEvents
{
    public class SisterRegionViewModel : ViewModelBase
    {
        public SisterRegionViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            GetRegionEvent<DocumentOpenedEvent>().Subscribe(Doc);
        }

        private void Doc(string obj)
        {
            Console.WriteLine("Sister doc openeed");
        }
    }
}
