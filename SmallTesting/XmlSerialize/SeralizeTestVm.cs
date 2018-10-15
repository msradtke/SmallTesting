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
        }

        void CreateData()
        {
            GroupRootObject = new PartGroupObject();
            var partGroup = new PartGroupObject();
            var partFilter = new PartAttributeFilterObject();

            GroupRootObject.GroupObjects = new List<GroupObject>();

            partGroup.FilterObjects = new List<FilterObject>();
            partGroup.GroupObjects = new List<GroupObject>();
            partGroup.FilterObjects.Add(partFilter);
            GroupRootObject.GroupObjects.Add(partGroup);

            XmlSerializer xsSubmit = new XmlSerializer(GroupRootObject.GetType(), new Type[] { typeof(PartGroupObject), typeof(PartAttributeFilterObject) });

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, GroupRootObject);
                    Xml = sww.ToString(); // Your XML
                }
            }

        }
        void GetGroupObjectsFromXml()
        {

        }
        public string Xml { get; set; }
        public GroupObject GroupRootObject { get; set; }
        public bool SameString { get; set; }

    }


    public class GroupObject
    {
        public GroupObject()
        {
        }

        public List<ItemGroup> Items { get; set; }

        public int Id { get; set; }
        public List<FilterObject> FilterObjects { get; set; }
        public List<GroupObject> GroupObjects { get; set; }
        public string Name { get; set; }
        public virtual List<ItemGroup> Group(List<ItemGroup> groupObjects, int index = 0)
        {
            Id = ++index;
            var newGroupObjects = new List<ItemGroup>();
            foreach (var group in GroupObjects)
                group.Group(Items);
            return newGroupObjects;
        }

        //public Func<List<object>, List<object>> Group { get; set; }
    }
    public class ItemGroup
    {
        public List<int> Ancestry { get; set; }
        public string Name { get; set; }
        public List<object> Items { get; set; }
    }
    public abstract class FilterObject
    {
        public string Name { get; set; }
        //public Func<List<object>, List<object>> Filter { get; set; }
    }
    public class PartAttributeFilterObject : FilterObject
    {
        public PartAttributeFilterObject()
        {
            //Filter = FilterAttributes;
        }

        List<object> FilterAttributes(List<object> parts)
        {
            return null;
        }
    }
    public class PartGroupObject : GroupObject
    {
        public PartGroupObject()
        {
        }

    }
    public class AttributeGroupObject : PartGroupObject
    {
        public AttributeGroupObject()
        {
            CreateAttributeGroupObjects();

        }

        void CreateAttributeGroupObjects()
        {
        }
        List<PartGroupObject> GroupMethod(List<PartGroupObject> groupObjects)
        {
            throw new NotImplementedException();
        }
    }

}
