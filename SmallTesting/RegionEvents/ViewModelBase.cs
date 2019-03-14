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
            Wire = new ViewModelWire(this);
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
        protected TEventType GetRegionEvent<TEventType>(ViewModelRegion viewModelRegion = null) where TEventType : RegionPubSubEvent, new()
        {
            if (viewModelRegion != null)
                return GetRegionEvent<TEventType>(new List<ViewModelRegion> { viewModelRegion });

            var regionEvent = EventAggregator.GetEvent<TEventType>();
            regionEvent.ViewModelRegions = Wire.;0
            regionEvent.Sender = this;
            return regionEvent;
            //return EventAggregator.GetEvent<T>();
        }
        protected TEventType GetRegionEvent<TEventType>(List<ViewModelRegion> ViewModelRegions) where TEventType : RegionPubSubEvent, new()
        {
            var regionEvent = EventAggregator.GetEvent<TEventType>();
            regionEvent.ViewModelRegions = ViewModelRegions;
            regionEvent.Sender = this;
            return regionEvent;
            //return EventAggregator.GetEvent<T>();
        }
        /*
        public class RegionPubSubEvent<T, TPayload> : PubSubEvent<T> where T: RegionEventPayload<TPayload>
        {
            RegionEventPayload<TPayload> EventCreatePayload(TPayload payload)
            {
                return new RegionEventPayload<TPayload>() { PayLoad = payload };
            }
        }*/

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



        public ViewModelWire Wire { get; set; }
        public ViewModelRegion Region { get; set; }
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
