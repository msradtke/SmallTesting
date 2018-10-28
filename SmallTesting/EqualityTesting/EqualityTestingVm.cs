using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.EqualityTesting
{
    public class EqualityTestingVm : BaseViewModel
    {

        public EqualityTestingVm()
        {
            Pattern1 = new Pattern();
            Pattern2 = new Pattern();

            var item1 = new Item { Length = 34, Width = 54 };
            var item2 = new Item { Length = 34, Width = 54 };

            Pattern1.PatternItems.Add(new PatternItem { Item = item1, Pattern = Pattern1 });
            Pattern2.PatternItems.Add(new PatternItem { Item = item2, Pattern = Pattern2 });

            Equal = item1.Equals(item2);
            EqualsOperator = item1 == item2;

        }

        public bool Equal { get; set; }
        public bool EqualsOperator { get; set; }

        public Pattern Pattern1 { get; set; }
        public Pattern Pattern2 { get; set; }

    }

    public class Pattern
    {
        public Pattern()
        {
            PatternItems = new List<PatternItem>();
        }
        public List<PatternItem> PatternItems { get; set; }
    }
    public class Item : IEquatable<Item>
    {
        public double Length { get; set; }
        public double Width { get; set; }

        public static bool operator ==(Item a, Item b)
        {
            return a.Length == b.Length && a.Width == b.Width;
        }
        public static bool operator !=(Item a, Item b)
        {
            return a.Length != b.Length || a.Width != b.Width;
        }

        public bool Equals(Item other)
        {
            return Length == other.Length && Width == other.Width;
        }

    }
    public class PatternDemand
    {
        public PatternDemand()
        {
            Pattern = new Pattern();
        }
        public Pattern Pattern { get; set; }
        public int Demand { get; set; }
    }
    public class PatternItem
    {
        public PatternItem()
        {
            Item = new Item();
            Pattern = new Pattern();
        }
        public Item Item { get; set; }
        public Pattern Pattern { get; set; }

    }
    public enum Orientation
    {
        XLength, XWidth
    }
}
