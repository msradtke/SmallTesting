using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.RegionEvents
{
    public class ViewModelBase : IViewModelBase, INotifyPropertyChanged
    {
        List<object> _handlers; //keep reference to handlers alive
        public ViewModelBase()
        {
            ListenRegionIds = new List<Guid>();
            PublishRegionIds = new List<Guid>();
            _handlers = new List<object>();
            ChildViewModels = new List<IViewModelBase>();
        }
        /// <summary>
        /// Alerts a listener to change view based on event of other viewmodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="FactoryParameter">Parameter to pass to the viewmodel factory</param>
        public delegate void ChangeViewEventHandler(object sender, object FactoryParameter = null);

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public List<Guid> ListenRegionIds { get; set; }
        public List<Guid> PublishRegionIds { get; set; }

        public string Description { get; private set; }
        private bool _descriptionIsSet { get; set; }
        public bool SetDescription(string name)
        {
            if (!_descriptionIsSet)
            {
                Description = name;
                _descriptionIsSet = true;
            }
            return _descriptionIsSet;
        }
        protected T GetRegionEvent<T>() where T : EventBase, new()
        {
            return EventAggregator.GetEvent<T>();
        }

        public class RegionPubSubEvent<T, TPayload> : PubSubEvent<T> where T: RegionEventPayload<TPayload>
        {
            RegionEventPayload<TPayload> EventCreatePayload(TPayload payload)
            {
                return new RegionEventPayload<TPayload>() { PayLoad = payload };
            }
        }


        protected void PublishEventToRegion<TEventType, TPayload>(TPayload payload) 
            where TEventType : PubSubEvent<RegionEventPayload<TPayload>>, new()
        {
            var regionPayload = new RegionEventPayload<TPayload>
            {
                PublishIds = PublishRegionIds,
                PayLoad = payload
            };
            var test = EventAggregator.GetEvent<TEventType>();
            EventAggregator.GetEvent<TEventType>().Publish(regionPayload);
        }
        protected void SubscribeRegion<TEventType, TPayload>(Action<TPayload> handler) 
            where TEventType : PubSubEvent<RegionEventPayload<TPayload>>, new()
        {
            Action<RegionEventPayload<TPayload>> regionHandler;
            regionHandler = x => handler(x.PayLoad);
            _handlers.Add(regionHandler);
            TEventType test = new TEventType();
            EventAggregator.GetEvent<TEventType>().Subscribe(regionHandler, ThreadOption.PublisherThread, false, ShouldListen);
        }

        protected bool ShouldListen<TPayload>(RegionEventPayload<TPayload> payload)
        {

            if (ListenRegionIds.Count == 0)
            {
                return true;
            }
            if (payload.PublishIds.Intersect(ListenRegionIds).Any())
            {
                return true;
            }
            return false;
        }

        protected void SubscribeRegion<T, TPayload>(Action<RegionEventPayload<TPayload>> handler, ThreadOption threadOption, bool keepSubscriberReferenceAlive) where T : PubSubEvent<RegionEventPayload<TPayload>>, new()
        {
            EventAggregator.GetEvent<T>().Subscribe(handler, threadOption, keepSubscriberReferenceAlive, x => x.PublishIds.Intersect(ListenRegionIds).Any());
        }
        protected void test<T>(RegionEventPayload<T> payload)
        {

        }
        public IEventAggregator EventAggregator { get; set; }

        protected List<IViewModelBase> ChildViewModels { get; set; }
        public void AddListenRegionId(Guid id)
        {
            ListenRegionIds.Add(id);
            foreach (var child in ChildViewModels)
                child.AddListenRegionId(id);
        }
        public void AddPublishRegionId(Guid id)
        {
            PublishRegionIds.Add(id);
            foreach (var child in ChildViewModels)
                child.AddPublishRegionId(id);
        }
        public void AddListenRegionIds(List<Guid> ids)
        {
            ListenRegionIds.AddRange(ids);
            foreach (var child in ChildViewModels)
                child.AddListenRegionIds(ids);
        }
        public void AddPublishRegionIds(List<Guid> ids)
        {
            PublishRegionIds.AddRange(ids);
            foreach (var child in ChildViewModels)
                child.AddPublishRegionIds(ids);
        }

        public virtual void Initialize() { }

    }

    public interface IViewModelBase
    {
        void AddListenRegionId(Guid id);
        void AddPublishRegionId(Guid id);
        void AddListenRegionIds(List<Guid> ids);
        void AddPublishRegionIds(List<Guid> ids);
        bool SetDescription(string description);
    }
}
