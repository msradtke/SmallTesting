using SmallTesting.EqualityTesting;
using SmallTesting.ListTesting;
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
            CurrentViewModel = new EqualityTestingVm();
        }
        public object CurrentViewModel { get; set; }
    }
}
