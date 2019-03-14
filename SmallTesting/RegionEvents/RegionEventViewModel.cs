using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace SmallTesting.RegionEvents
{
    public class RegionEventViewModel : ViewModelBase
    {
        public RegionEventViewModel()
        {
            //PublishEventToRegion<DocumentOpenedEvent>().(message);
            GetRegionEvent<DocumentOpenedEvent>().Publish("test");
            ChildViewModel = new ChildRegionViewModel();

        }
        public ChildRegionViewModel ChildViewModel { get; set; }

    }
    
    public class RegionEventPayload<T>
    {
        public RegionEventPayload()
        {
            ViewModelRegions = new List<ViewModelRegion>();
        }
        public List<ViewModelRegion> ViewModelRegions { get; set; }
        public T PayLoad { get; set; }
        public ViewModelBase Sender { get; set; }
    }

    public class DocumentOpenedEvent : RegionPubSubEvent<string> { }
}
