using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SmallTesting.XmlSerialize
{
    public class SeralizeTestVm
    {
        public SeralizeTestVm()
        {
            CreateData();
            SetGroupObjects();
        }
        void CreateData()
        {
            var stringList1 = new List<int> { 1, 2, 3 };
            var stringList2 = new List<int> { 1, 2, 3 };
            SameString = stringList1.SequenceEqual(stringList2);


            GroupRootObject = new PartGroupObject();
            var partGroup = new PartGroupObject();
            var partFilter = new PartAttributeFilterObject();

            GroupRootObject.GroupObjects = new List<GroupObject<Part>>();

            partGroup.FilterObjects = new List<FilterObject<Part>>();
            partGroup.GroupObjects = new List<GroupObject<Part>>();
            partGroup.FilterObjects.Add(partFilter);


            GroupRootObject.GroupObjects.Add(partGroup);

            XmlSerializer xsSubmit = new XmlSerializer(GroupRootObject.GetType(), new Type[] { typeof(PartGroupObject), typeof(PartAttributeFilterObject) });

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
            GroupObjects.Add(new AttributeGroupObject());
            GroupObjects.Add(new PartTypeGroupObject());
            GroupObjects.Add(new PartGroupObject());
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


        void GetGroupObjectsFromXml()
        {

        }
        public string Xml { get; set; }
        public GroupObject<Part> GroupRootObject { get; set; }
        public List<GroupObject<Part>> GroupObjects { get; set; }
        public bool SameString { get; set; }
        public List<Part> Parts { get; set; }
    }
    public class GroupObject<T>
    {
        public GroupObject()
        {
        }

        public List<ItemGroup<T>> ItemGroups { get; set; }

        public int Id { get; set; }
        public List<FilterObject<T>> FilterObjects { get; set; }
        public List<GroupObject<T>> GroupObjects { get; set; }
        public string Name { get; set; }
        public virtual List<ItemGroup<T>> Group(List<ItemGroup<T>> groupObjects, int index = 0)
        {
            Id = ++index;
            var newGroupObjects = new List<ItemGroup<T>>();
            foreach (var group in GroupObjects)
                group.Group(ItemGroups);
            return newGroupObjects;
        }

        ItemGroup<T> GetItemGroup(List<int> ancestry)
        {
            foreach(var itemGroup in ItemGroups)
            {
                if (itemGroup.Ancestry.SequenceEqual(ancestry))
                    return itemGroup;
            }
            return null;
        }
        public virtual List<ItemGroup<T>> Filter(List<ItemGroup<T>> itemGroups)
        {
            return null;
        }


        //public Func<List<object>, List<object>> Group { get; set; }
    }
    public class ItemGroup<T>
    {
        public List<int> Ancestry { get; set; }
        public string Name { get; set; }
        public List<T> Items { get; set; }
    }
    public class FilterObject<T>
    {
        public string Name { get; set; }
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
        }
    }

    public class PartGroupObject : GroupObject<Part>
    {
        public PartGroupObject()
        {
            Name = "Part";
        }
    }

    public class PartTypeGroupObject : GroupObject<Part>
    {
        public PartTypeGroupObject()
        {
            Name = "Part Type";
        }
        public override List<ItemGroup<Part>> Group(List<ItemGroup<Part>> itemGroups, int index = 0)
        {
            Dictionary<string, ItemGroup<Part>> PartTypeItemGroup = new Dictionary<string, ItemGroup<Part>>();

            foreach (var itemGroup in itemGroups)
            {
                var allPartTypeGroups = new List<string>();

                foreach (var part in itemGroup.Items)
                {
                    var groupObject = PartTypeItemGroup[part.Type];
                    if (groupObject != null)
                        groupObject.Items.Add(part);
                }
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
