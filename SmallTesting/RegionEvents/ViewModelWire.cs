using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.RegionEvents
{
    public class ViewModelWire
    {
        public ViewModelWire(ViewModelBase parent)
        {            
            ChildViewModelRegion = new ViewModelRegion();
        }
        public void RegisterChild(ViewModelBase child)
        {            
            child.ListenRegions.Add(ChildViewModelRegion);
            child.PublishRegions.Add(ChildViewModelRegion);
            Cleanup += ()=> child.Cleanup();
        }

        public Action Cleanup { get; set; }
        public List<Action> CleanupActions { get; private set; }
        public ViewModelRegion ChildViewModelRegion { get; private set; }
    }

    public class ViewModelRegion
    {
        static ViewModelRegion()
        {            
            GlobalRegion = new ViewModelRegion();
        }
        public ViewModelRegion()
        {
            Id = Guid.NewGuid().ToString();
        }
        public static readonly ViewModelRegion GlobalRegion;
        public string Id { get; set; }
    }
}
