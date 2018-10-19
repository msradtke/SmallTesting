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
        }
        public ICommand GroupCommand { get; set; }
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
    public class GroupObject<T>
    {
        public GroupObject()
        {
            GroupObjects = new List<GroupObject<T>>();
            FilterObjects = new List<FilterObject<T>>();
        }
        //public List<GroupItem<T>> GroupItems { get; set; }

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
    public class FilterObject<T>
    {
        public string Name { get; set; }
        public virtual List<GroupItem<Part>> Filter(List<GroupItem<Part>> groupItems)
        {
            return null;
        }
        //public Func<List<object>, List<object>> Filter { get; set; }
    }
    public class PartAttributeFilterObject : FilterObject<Part>
    {
        public PartAttributeFilterObject()
        {
            //Filter = FilterAttributes;
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
    }

    public class PartTypeGroupObject : GroupObject<Part>
    {
        public PartTypeGroupObject()
        {
            Name = "Part Type";
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
    }

  

}
