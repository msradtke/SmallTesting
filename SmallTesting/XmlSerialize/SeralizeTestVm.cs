using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace SmallTesting.XmlSerialize
{
    public class SeralizeTestVm
    {
        public SeralizeTestVm()
        {
            GroupCommand = new ActionCommand(GroupParts);
            CreateData();
            SetGroupObjects();
            CreatePartData();
            GroupParts();

            CreatePartGroupVm = new CreatePartGroupVm(GetPartGroupObjects(), GetPartFilterObjects());
        }
        public ICommand GroupCommand { get; set; }
        public object CreatePartGroupVm { get; set; }
        List<GroupObject<Part>> GetPartGroupObjects()
        {
            return new List<GroupObject<Part>>
            {
                new PartNameGroupObject(),
                new PartTypeGroupObject(),
                new PartFilterGroupObject(),
            };
        }

        List<FilterObject<Part>> GetPartFilterObjects()
        {
            return new List<FilterObject<Part>>
            {
                new PartAttributeFilterObject(),
                new PartPropertyFilterObject(),
            };
        }

        void CreateData()
        {
            var stringList1 = new List<int> { 1, 2, 3 };
            var stringList2 = new List<int> { 1, 2, 3 };
            SameString = stringList1.SequenceEqual(stringList2);


            GroupRootObject = new PartGroupObject();
            var partTypeGroup = new PartTypeGroupObject();
            var partNameGroup = new PartGroupObject();
            var partFilter = new PartAttributeFilterObject();

            GroupRootObject.GroupObjects = new List<GroupObject<Part>>();

            var partNameFilter = new PartPropertyFilterObject();
            partNameFilter.PropertyName = "Part Name";
            partNameFilter.Value = "Grooved Board";
            partNameGroup.FilterObjects.Add(partNameFilter);

            // partGroup.FilterObjects = new List<FilterObject<Part>>();
            // partGroup.GroupObjects = new List<GroupObject<Part>>();
            //partGroup.FilterObjects.Add(partFilter);


            GroupRootObject.GroupObjects.Add(partTypeGroup);
            GroupRootObject.GroupObjects.Add(partNameGroup);

            XmlSerializer xsSubmit = new XmlSerializer(GroupRootObject.GetType(), new Type[] { typeof(PartTypeGroupObject), typeof(PartGroupObject), typeof(PartPropertyFilterObject), typeof(PartAttributeFilterObject) });

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, GroupRootObject);
                    Xml = System.Xml.Linq.XDocument.Parse(sww.ToString()).ToString(); // Your XML
                }
            }

            //deserialize
            using (var reader = new StringReader(Xml))
            {
                var car = (GroupObject<Part>)xsSubmit.Deserialize(reader);
            }

        }
        void SetGroupObjects()
        {
            GroupObjects = new List<GroupObject<Part>>();
            //GroupObjects.Add(new AttributeGroupObject());
            GroupObjects.Add(new PartTypeGroupObject());
            //GroupObjects.Add(new PartGroupObject());
        }

        void CreatePartData()
        {
            Parts = new List<Part>
            {
                new Part { Name = "Grooved Board",Length = 27,Type = "2x6"},
                new Part { Name = "Grooved Board",Length = 55,Type = "2x6"},
                new Part { Name = "Grooved Board",Length = 96,Type = "2x6"},
                new Part { Name = "Grooved Board",Length = 27,Type = "2x4"},
                new Part { Name = "Grooved Board",Length = 78,Type = "2x8"},
                new Part { Name = "",Length = 27,Type = "2x6"},
                new Part { Name = "",Length = 85,Type = "2x6"},
                new Part { Name = "",Length = 27,Type = "2x8"},
                new Part { Name = "",Length = 96,Type = "6x8"}
            };

        }

        void GroupParts()
        {
            var itemGroup = new ItemGroup<Part>();
            itemGroup.Items = Parts;

            GroupedParts = GroupObject<Part>.CreateGroupItems(Parts);
            GroupRootObject.Group(GroupedParts);
        }
        void GetGroupObjectsFromXml()
        {

        }
        public string Xml { get; set; }
        public GroupObject<Part> GroupRootObject { get; set; }
        public List<GroupObject<Part>> GroupObjects { get; set; }
        public bool SameString { get; set; }
        public List<Part> Parts { get; set; }
        public List<GroupItem<Part>> GroupedParts { get; set; }
    }
    public abstract class GroupObject<T>
    {
        public GroupObject()
        {
            GroupObjects = new List<GroupObject<T>>();
            FilterObjects = new List<FilterObject<T>>();
        }
        //public List<GroupItem<T>> GroupItems { get; set; }

        public abstract object GetViewModel();
        public abstract GroupObject<T> Create();

        public int Id { get; set; }
        public List<FilterObject<T>> FilterObjects { get; set; }
        public List<GroupObject<T>> GroupObjects { get; set; }
        public string Name { get; set; }
        public virtual void Group(List<GroupItem<T>> groupItems)
        {
            var newGroupObjects = new List<ItemGroup<T>>();
            foreach (var group in GroupObjects)
                group.Group(groupItems);
        }

        public virtual List<GroupItem<T>> Filter(List<GroupItem<T>> groupItems)
        {
            return null;
        }

        public static List<GroupItem<T>> CreateGroupItems(List<T> items)
        {
            var groupItems = new List<GroupItem<T>>();
            foreach (var item in items)
            {
                var groupItem = new GroupItem<T>();
                groupItem.Item = item;
                groupItems.Add(groupItem);
            }

            return groupItems;
        }


        //public Func<List<object>, List<object>> Group { get; set; }
    }
    public class ItemGroup<T>
    {
        public ItemGroup()
        {
            Items = new List<T>();
        }
        public string Name { get; set; }
        public List<T> Items { get; set; }
    }
    public class GroupItem<T>
    {
        public GroupItem()
        {
            GroupNames = new List<string>();
        }
        public string Name { get; set; }
        public T Item { get; set; }
        public List<string> GroupNames { get; set; }
    }
    public abstract class FilterObject<T>
    {
        public abstract object GetViewModel();
        public abstract FilterObject<T> Create();
        public string Name { get; set; }
        public virtual List<GroupItem<T>> Filter(List<GroupItem<T>> groupItems)
        {
            return null;
        }
        //public Func<List<object>, List<object>> Filter { get; set; }
    }
    public class PartAttributeFilterObject : FilterObject<Part>
    {
        public PartAttributeFilterObject()
        {
            Name = "Part Attribute";
        }
        public string Attribute { get; set; }
        public string Value { get; set; }

        public override FilterObject<Part> Create()
        {
            return new PartAttributeFilterObject();
        }

        public override object GetViewModel()
        {
            return new PartAttributeFilterVm(this);
        }
    }
    public class PartPropertyFilterObject : FilterObject<Part>
    {
        public PartPropertyFilterObject()
        {
            Name = "Part Property";
            PropertyNames = new List<string> { "Part Type", "Part Name" };
        }

        public List<string> PropertyNames { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }

        public override List<GroupItem<Part>> Filter(List<GroupItem<Part>> groupItems)
        {
            Name = Value;
            switch (PropertyName)
            {
                case "Part Type":
                    return FilterByPartType(groupItems);
                case "Part Name":
                    return FilterByPartName(groupItems);
                default:
                    return new List<GroupItem<Part>>();
            }
        }

        List<GroupItem<Part>> FilterByPartType(List<GroupItem<Part>> items)
        {
            var newGroupItems = new List<GroupItem<Part>>();

            foreach (var item in items)
            {
                if (item.Item.Type == Value)
                    newGroupItems.Add(item);

            }
            return newGroupItems;
        }

        List<GroupItem<Part>> FilterByPartName(List<GroupItem<Part>> items)
        {
            var newGroupItems = new List<GroupItem<Part>>();

            foreach (var item in items)
            {
                if (item.Item.Name == Value)
                    newGroupItems.Add(item);
            }
            return newGroupItems;
        }

        public override object GetViewModel()
        {
            return new PartPropertyFilterVm(this);
        }

        public override FilterObject<Part> Create()
        {
            return new PartPropertyFilterObject();
        }
    }

    public class PartGroupObject : GroupObject<Part>
    {
        public PartGroupObject()
        {
            Name = "Part";

        }

        public override void Group(List<GroupItem<Part>> groupItems)
        {
            string Name = "";
            string space = "";

            foreach (var filter in FilterObjects)
            {
                Name += space;
                groupItems = filter.Filter(groupItems);
                Name += filter.Name;
                space = " ";
            }

            if (!string.IsNullOrWhiteSpace(Name))
                foreach (var item in groupItems)
                {
                    item.GroupNames.Add(Name);
                }

            base.Group(groupItems);
        }
        public override List<GroupItem<Part>> Filter(List<GroupItem<Part>> groupItems)
        {
            return base.Filter(groupItems);
        }

        public override object GetViewModel()
        {
            throw new NotImplementedException();
        }

        public override GroupObject<Part> Create()
        {
            return new PartGroupObject();
        }
    }

    public class PartTypeGroupObject : GroupObject<Part>
    {
        public PartTypeGroupObject()
        {
            Name = "Part Type";
        }

        public override GroupObject<Part> Create()
        {
            return new PartTypeGroupObject();
        }

        public override object GetViewModel()
        {
            return new GroupObjectViewModel { Description = "Group by part type." };
        }
        public override void Group(List<GroupItem<Part>> groupItems)
        {
            foreach (var item in groupItems)
            {
                item.GroupNames.Add(item.Item.Type);//just add the part type name as the groupname
            }
        }
    }

    public class AttributeGroupObject : GroupObject<Part>
    {
        public AttributeGroupObject()
        {
            FilterObjects = new List<FilterObject<Part>>();
            Name = "Part Attribute";
            FilterObjects.Add(new PartAttributeFilterObject());
        }

        public override GroupObject<Part> Create()
        {
            return new AttributeGroupObject();
        }

        public override object GetViewModel()
        {
            throw new NotImplementedException();
        }
    }

    public class PartNameGroupObject : GroupObject<Part>
    {
        public PartNameGroupObject()
        {
            Name = "Part Name";
        }

        public override GroupObject<Part> Create()
        {
            return new PartNameGroupObject();
        }

        public override object GetViewModel()
        {
            return new GroupObjectViewModel { Description = "Group by part name." };
        }

        public override void Group(List<GroupItem<Part>> groupItems)
        {
            groupItems.ForEach(x => x.GroupNames.Add(x.Item.Name));
        }
    }


    public class GroupObjectViewModel
    {
        public string Description { get; set; }
    }

    public class PartFilterGroupObject : GroupObject<Part>
    {
        public PartFilterGroupObject()
        {
            Name = "Part Filter";
        }
        public override GroupObject<Part> Create()
        {
            return new PartTypeGroupObject();
        }

        public override object GetViewModel()
        {
            return new GroupObjectViewModel { Description = "Group by a part filter." };
        }
    }

    public class PartPropertyFilterVm : BaseViewModel
    {
        private string _selectedPropertyName;

        public PartPropertyFilterVm(PartPropertyFilterObject filterObject)
        {
            FilterObject = filterObject;
            PropertyNames = FilterObject.PropertyNames;

        }
        public List<string> PropertyNames { get; set; }
        public string SelectedPropertyName { get => _selectedPropertyName; set { PropertySelected(); _selectedPropertyName = value; } }
        public string Value { get; set; }

        public PartPropertyFilterObject FilterObject { get; set; }

        void PropertySelected()
        {
            FilterObject.PropertyName = SelectedPropertyName;
        }
    }

    public class PartAttributeFilterVm : BaseViewModel
    {
        private string _value;
        private string _attribute;

        public PartAttributeFilterVm(PartAttributeFilterObject filterObject)
        {
            FilterObject = filterObject;
        }

        public List<string> PropertyNames { get; set; }
        public string Value { get => _value; set { _value = value; FilterObject.Value = value; } }
        public string Attribute { get => _attribute; set { _attribute = value; FilterObject.Attribute = value; } }

        public PartAttributeFilterObject FilterObject { get; set; }


    }
}



