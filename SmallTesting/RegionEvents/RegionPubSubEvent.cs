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
        PubSubEvent<RegionEventPayload<TPayload>> publishEvent;
        PubSubEvent<TPayload> subEvent;
        public RegionPubSubEvent()
        {
            publishEvent = new PubSubEvent<RegionEventPayload<TPayload>>();
        }
        public void Publish(TPayload payload)
        {
            var regionPayload = GetRegionEventPayload(payload);
            publishEvent.Publish(regionPayload);
        }
        public SubscriptionToken Subscribe(Action<TPayload> action)
        {
            return subEvent.Subscribe(action);
        }
        public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive);
        public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive);
        public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter);

        RegionEventPayload<TPayload> GetRegionEventPayload(TPayload payload)
        {
            var regionPayload = new RegionEventPayload<TPayload>();
            regionPayload.ViewModelRegions = ViewModelRegions;
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
        public List<ViewModelRegion> ViewModelRegions { get; set; }
        public ViewModelBase Sender { get; set; }
    }
}
