using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Buildings
{
    public class Buildlist
    {
        public Building building = new Building();
        public List<int> added = new List<int>();
        public List<int> deteled = new List<int>();

        public Buildlist() { }

        public Buildlist(Building building, List<int> added, List<int> deteled)
        {
            this.building = building;
            this.added = added;
            this.deteled = deteled;
        }
    }
}
