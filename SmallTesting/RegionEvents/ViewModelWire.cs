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
            
            DefaultViewModelRegion = new ViewModelRegion();

        }

        public void RegisterChild(ViewModelBase child, ViewModelRegion viewModelRegion = null)
        {
            if (viewModelRegion == null)
                viewModelRegion = DefaultViewModelRegion;
            child.Region = viewModelRegion;

        }

        public ViewModelRegion DefaultViewModelRegion { get; private set; }
    }

    public class ViewModelRegion
    {
        static ViewModelRegion()
        {
            GlobalRegion = new ViewModelRegion();
        }
        public static readonly ViewModelRegion GlobalRegion;

    }
}
