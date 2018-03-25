using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfPolygons: GroupOfGeometricFigure
    {
        public List<Polygon> Polygons = new List<Polygon>();
        public Polygon TempPolygon = new Polygon(); //Текущий многоугольник

        public GroupOfPolygons()
        {

        }

        public override void TempDefault()
        {
            TempPolygon = new Polygon();
            active = false;
            step = false;
        }

        public override void Add(object elem)
        {
            Polygons.Add((Polygon)elem);
        }

        public override void Remove(int i)
        {
            Polygons[i] = new Polygon();
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < Polygons.Count; j++)
            {
                if (j != i)
                {
                    Polygons[j].SetActive(false);
                }
                else
                {
                    Polygons[j].SetActive(true);
                }
            }
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            int index = -1;
            dist = Int32.MaxValue;
            for (int i = 0; i < Polygons.Count; i++)
            {
                if (dl == Polygons[i].DL)
                {
                    double _dist = Polygons[i].Search(x, y);
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
            foreach (var br in Polygons)
            {
                br.Draw();
            }
            TempPolygon.Draw();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }
    }
}
