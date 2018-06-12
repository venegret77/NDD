using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Buildings
{
    static public class TEMP
    {
        static public string[] WrapText(this string text, int max)
        {
            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                            ? max - (charCount % max) : 0) + w.Length + 1) / max)
                        .Select(g => string.Join(" ", g.ToArray()))
                        .ToArray();
        }
    }

    public struct BUILDLIST
    {
        public Building building;
        public List<int> Added;
        public List<int> Deteled;

        public BUILDLIST(Building building, List<int> added, List<int> deteled)
        {
            this.building = building;
            Added = added;
            Deteled = deteled;
        }
    }
}
