using Prism.Events;
using SmallTesting.EqualityTesting;
using SmallTesting.Expressions;
using SmallTesting.ListTesting;
using SmallTesting.RegionEvents;
using SmallTesting.WpfTesting;
using SmallTesting.XmlSerialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting
{
    public class MainWindowVm
    {
        public MainWindowVm()
        {
            IEventAggregator eg = new EventAggregator();
            CurrentViewModel = new RegionEventViewModel(eg);
        }
        public object CurrentViewModel { get; set; }
    }
}
