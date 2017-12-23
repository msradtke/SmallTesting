using SmallTesting.WpfTesting;
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
            CurrentViewModel = new ClickToEditVm();
        }
        public object CurrentViewModel { get; set; }
    }
}
