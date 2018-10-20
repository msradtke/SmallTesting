using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.XmlSerialize
{
    public class CreatePartFilterViewModel : BaseViewModel
    {
        private FilterObject<Part> _selectedFilterObject;

        public CreatePartFilterViewModel(List<FilterObject<Part>> filterObjects)
        {
            FilterObjects = filterObjects;
            //GroupObject = groupObject;
        }

        public List<FilterObject<Part>> FilterObjects { get; set; }
        public GroupObject<Part> GroupObject { get; set; }
        public FilterObject<Part> SelectedFilterObject { get => _selectedFilterObject; set { _selectedFilterObject = value; FilterObjectedSelected(); } }
        public object SelectedFilterViewModel { get; set; }
        void FilterObjectedSelected()
        {
            SelectedFilterViewModel = SelectedFilterObject.GetViewModel();
        }
    }
}
