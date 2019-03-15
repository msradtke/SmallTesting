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
        Action<RegionEventPayload<TPayload>> regionEventPayload;
        public RegionPubSubEvent()
        {
            pubSubEvent = new PubSubEvent<RegionEventPayload<TPayload>>();
        }
        public void Publish(TPayload payload)
        {
            var regionPayload = GetRegionEventPayload(payload);
            pubSubEvent.Publish(regionPayload);
        }
        public SubscriptionToken Subscribe(Action<TPayload> action)
        {
            regionEventPayload = x => action(x.PayLoad);
            var token = pubSubEvent.Subscribe(regionEventPayload, ThreadOption.PublisherThread, true, Sender.ShouldListen);
            Sender.Wire.Cleanup += () => pubSubEvent.Unsubscribe(token);
            return token;
        }

        RegionEventPayload<TPayload> GetRegionEventPayload(TPayload payload)
        {
            var regionPayload = new RegionEventPayload<TPayload>();
            regionPayload.ViewModelRegions = PublishRegions;
            regionPayload.Sender = Sender;
            regionPayload.PayLoad = payload;
            return regionPayload;
        }
    }
    public interface IRegionPubSubEvent
    {
        List<Guid> PublishIds { get; set; }
    }
    public class RegionPubSubEvent : EventBase
    {
        public List<ViewModelRegion> PublishRegions { get; set; }
        public ViewModelBase Sender { get; set; }
    }
}
