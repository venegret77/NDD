using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfPolygons
    {
        public List<Polygon> Polygons = new List<Polygon>();
        public Polygon TempPolygon = new Polygon(); //Текущий многоугольник
        public bool polygon_active = false;
        public bool step_polygon = false;

        public GroupOfPolygons()
        {

        }

        public void TempDefault()
        {
            TempPolygon = new Polygon();
            polygon_active = false;
            step_polygon = false;
        }

        public void Add(Polygon br)
        {
            Polygons.Add(br);
        }

        public void Remove(int i)
        {
            Polygons[i] = new Polygon();
        }

        public void Choose(int i)
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

        public int CalcNearestBLine(int x, int y, out double dist, DrawLevel dl)
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

        public void Draw()
        {
            foreach (var br in Polygons)
            {
                br.Draw();
            }
            TempPolygon.Draw();
        }
    }
}
