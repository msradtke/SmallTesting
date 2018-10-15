using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.ListTesting
{
    public class GroupTestVm
    {
        public GroupTestVm()
        {
            //CreateData();
        }

        public List<Part> Parts { get; set; }
        void Group()
        {
            var partTypes = Parts.GroupBy(x => x.Type).SelectMany(x => x.ToList()).Where(x => x.Name == "Grooved board");

            //var groovedBoards = partTypes.Select(x => x.Select(z => z.Name == "Grooved Board").ToList()).ToList();
        }
        void group(List<Predicate<Part>> groupFilters, List<Part> parts)
        {
            foreach (var filter in groupFilters)
            {
                foreach (var part in parts)
                {

                }
            }
        }


        void CreateData()
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
    }
}
public class Part
{
    public string Name { get; set; }
    public int Length { get; set; }
    public string Type { get; set; }
}


