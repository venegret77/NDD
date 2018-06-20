using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Buildings
{
    public class Buildlist: ICloneable
    {
        public Building building = new Building();
        public List<int> added = new List<int>();
        public List<int> deleted = new List<int>();

        public Buildlist() { }

        public Buildlist(Building building, List<int> added, List<int> deteled)
        {
            this.building = building;
            this.added = added;
            this.deleted = deteled;
        }

        public object Clone()
        {
            List<int> a = new List<int>();
            foreach (var add in added)
                a.Add(add);
            List<int> d = new List<int>();
            foreach (var del in deleted)
                d.Add(del);
            return new Buildlist
            {
                building = (Building)this.building.Clone(),
                added = a,
                deleted = d
            };
        }
    }
}
