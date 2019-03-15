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
        public ViewModelBase()
        {
            ListenRegions = new List<ViewModelRegion>();
            PublishRegions = new List<ViewModelRegion>();

            Wire = new ViewModelWire(this);
            ListenRegions.Add(Wire.ChildViewModelRegion);
            
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
            regionEvent.Sender = this;
            regionEvent.PublishRegions = PublishRegions;
            return regionEvent;
            //return EventAggregator.GetEvent<T>();
        }
        protected TEventType GetRegionEvent<TEventType>(List<ViewModelRegion> ViewModelRegions) where TEventType : RegionPubSubEvent, new()
        {
            var regionEvent = EventAggregator.GetEvent<TEventType>();
            regionEvent.PublishRegions = ViewModelRegions;
            regionEvent.Sender = this;
            return regionEvent;
            //return EventAggregator.GetEvent<T>();
        }

        public bool ShouldListen<TPayload>(RegionEventPayload<TPayload> payload)
        {

            if (ListenRegions.Count == 0)
            {
                return true;
            }
            if (payload.ViewModelRegions.Intersect(ListenRegions).Any())
            {
                return true;
            }
            return false;
        }
        
        public IEventAggregator EventAggregator { get; set; }
        
        public virtual void Initialize() { }
               
        public ViewModelWire Wire { get; set; }
        public List<ViewModelRegion> ListenRegions { get; set; }
        public List<ViewModelRegion> PublishRegions { get; set; }
        public void Cleanup()
        {
            Wire.Cleanup(); //unsubscribe to all
        }
    }

    public interface IViewModelBase
    {
        bool SetDescription(string description);
    }
}
