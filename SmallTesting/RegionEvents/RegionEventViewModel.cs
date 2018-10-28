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
            //GetRegionEvent<DocumentOpenedEvent>().Publish(message);
        }
    }

    public class RegionEventPayload<T>
    {
        public RegionEventPayload()
        {
            PublishIds = new List<Guid>();
        }
        public List<Guid> PublishIds { get; set; }
        public T PayLoad { get; set; }
    }

    public class DocumentOpenedEvent : PubSubEvent<RegionEventPayload<string>> { }
}
