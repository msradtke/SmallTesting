using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.RegionEvents
{
    public class RegionPubSubEvent<TPayload> : RegionPubSubEvent
    {
        PubSubEvent<RegionEventPayload<TPayload>> pubSubEvent;
        public RegionPubSubEvent()
        {
            pubSubEvent = new PubSubEvent<RegionEventPayload<TPayload>>();
        }
        public void Publish(TPayload payload)
        {
            var regionPayload = new RegionEventPayload<TPayload>();
            regionPayload.PublishIds = PublishIds;
            regionPayload.Sender = Sender;
            pubSubEvent.Publish(regionPayload);
        }

    }
    public interface IRegionPubSubEvent
    {
        List<Guid> PublishIds { get; set; }
    }
    public class RegionPubSubEvent : EventBase
    {
        public List<Guid> PublishIds { get; set; }
        public ViewModelBase Sender { get; set; }
    }
}
