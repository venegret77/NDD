using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfBuildings
    {
        public List<Building> Buildings = new List<Building>();
        public GroupOfBuildings()
        {

        }

        public void TempDefault()
        {
            foreach(var b in Buildings)
            {
                b.Entrances.Enterances.TempDefault();
            }
        }

        public void Add(Building build)
        {
            Buildings.Add(build);
        }

        public void Remove(int i)
        {
            Buildings[i] = new Building();
        }

        public void Choose(int i)
        {
            for (int j = 0; j < Buildings.Count; j++)
            {
                if (j != i)
                {
                    Buildings[j].Entrances.Enterances.Choose(-1);
                    Buildings[j].InputWires.InputWires.Choose(-1);
                    Buildings[j].SetActive(false);
                }
                else
                {
                    Buildings[j].Entrances.Enterances.Choose(-1);
                    Buildings[j].InputWires.InputWires.Choose(-1);
                    Buildings[j].SetActive(true);
                }
            }
        }

        public int CalcNearestBuild(int x, int y, out double dist, DrawLevel dl)
        {
            int index = -1;
            dist = -1;
            dist = Int32.MaxValue;
            for (int i = 0; i < Buildings.Count; i++)
            {
                if (dl == Buildings[i].MainMapDL)
                {
                    double _dist = Buildings[i].CalcPointInBuild(x, y);
                    if (dist == -1 & _dist != -1)
                    {
                        dist = _dist;
                        index = i;
                    }
                    else if (_dist < dist & _dist != -1)
                    {
                        dist = _dist;
                        index = i;
                    }
                }
            }
            return index;
        }

        public void Draw()
        {
            foreach (var build in Buildings)
            {
                build.Draw();
            }
        }
    }
}
