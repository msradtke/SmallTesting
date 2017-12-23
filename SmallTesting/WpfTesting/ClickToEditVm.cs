using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
namespace SmallTesting.WpfTesting
{
    public class ClickToEditVm : INotifyPropertyChanged
    {
        public ClickToEditVm()
        {
            DisplayText = "Display";
            EditText = "Edit";
            CanEditAttribute = false;
            EditAttributeCommand = new ActionCommand(EditAttribute);
            RemoveEditCommand = new ActionCommand(RemoveEdit);
        }
        public ICommand EditAttributeCommand { get; set; }
        public ICommand RemoveEditCommand { get; set; }
        public bool CanEditAttribute { get; set; }
        public string DisplayText { get; set; }
        public string EditText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void EditAttribute()
        {
            CanEditAttribute = !CanEditAttribute;
        }
        void RemoveEdit()
        {
            CanEditAttribute = false;
        }
    }
}
