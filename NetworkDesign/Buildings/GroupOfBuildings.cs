using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfBuildings: GroupOfElements
    {
        public List<Building> Buildings = new List<Building>();

        public GroupOfBuildings()
        {

        }

        public override void TempDefault()
        {
            foreach(var b in Buildings)
            {
                b.Entrances.Enterances.TempDefault();
            }
        }

        public override void Add(object elem)
        {
            Buildings.Add((Building)elem);
            Buildings.Last().CalcCenterPoint();
        }

        public override void Remove(int i)
        {
            Buildings[i] = new Building();
            //Доделать проверку на наличие элементов в здании
        }

        public override void Choose(int i)
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

        public override int Search(int x, int y, out double dist, DrawLevel dl)
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

        public override void Draw()
        {
            foreach (var build in Buildings)
            {
                build.Draw();
            }
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            throw new NotImplementedException();
        }
    }
}
