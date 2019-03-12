using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace SmallTesting
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            Test = "testing";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Test { get; set; }

    }
}
