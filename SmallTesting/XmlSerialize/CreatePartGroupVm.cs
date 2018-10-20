using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmallTesting.XmlSerialize
{
    public class CreatePartGroupVm : BaseViewModel
    {
        private GroupObjectSelectVm<Part> _selectedGroupObjectVm;

        public CreatePartGroupVm(List<GroupObject<Part>> partGroupObjects, List<FilterObject<Part>> filterObjects)
        {
            PartGroupObjects = partGroupObjects ?? new List<GroupObject<Part>>();
            CreatePartFilterViewModels = new ObservableCollection<CreatePartFilterViewModel>();
            FilterObjects = filterObjects;
            Grouping = new ObservableCollection<GroupObject<Part>>();
            CreatePartFilterViewModel = new CreatePartFilterViewModel(filterObjects);

            AddNewFilterCommand = new ActionCommand(AddNewFilter);
            AddNewGroupObjectCommand = new ActionCommand(AddNewGroupObject);

            CreateGroupObjectVms();
        }

        public ICommand AddNewGroupObjectCommand { get; private set; }
        public ICommand AddNewFilterCommand { get; set; }

        public List<FilterObject<Part>> FilterObjects { get; set; }
        public ObservableCollection<GroupObject<Part>> Grouping { get; set; }
        public object CreatePartFilterViewModel { get; set; }
        public List<GroupObject<Part>> PartGroupObjects { get; set; }
        public GroupObject<Part> CurrentGroupObject { get; set; }
        public ObservableCollection<CreatePartFilterViewModel> CreatePartFilterViewModels { get; set; }
        public List<GroupObjectSelectVm<Part>> GroupObjectSelectVms { get; set; }
        public GroupObjectSelectVm<Part> SelectedGroupObjectVm
        {
            get => _selectedGroupObjectVm;
            set
            {
                _selectedGroupObjectVm = value;
                CurrentGroupObject = _selectedGroupObjectVm.GroupObject.Create();
                SelectedGroupObjectVm.ViewModel = SelectedGroupObjectVm.GroupObject.GetViewModel();
            }
        }

        void CreateGroupObjectVms()
        {
            GroupObjectSelectVms = new List<GroupObjectSelectVm<Part>>();
            foreach (var group in PartGroupObjects)
            {
                var vm = new GroupObjectSelectVm<Part> { GroupObject = group };
                GroupObjectSelectVms.Add(vm);
            }
        }
        void AddNewGroupObject()
        {
            Grouping.Add(SelectedGroupObjectVm.GroupObject);
        }
        void AddNewFilter()
        {

            CreatePartFilterViewModels.Add(new CreatePartFilterViewModel(FilterObjects));
        }



    }

    public class GroupObjectSelectVm<T> : BaseViewModel
    {
        public GroupObject<T> GroupObject { get; set; }
        public bool IsSelected { get; set; }
        public object ViewModel { get; set; }
    }
}
