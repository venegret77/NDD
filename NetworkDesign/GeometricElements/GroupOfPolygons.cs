using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Группа многоугольников
    /// </summary>
    public class GroupOfPolygons: GroupOfElements
    {
        /// <summary>
        /// Список многоугольников
        /// </summary>
        public List<Polygon> Polygons = new List<Polygon>();
        /// <summary>
        /// Временный многоугольник
        /// </summary>
        public Polygon TempPolygon = new Polygon();
        /// <summary>
        /// Конструктор
        /// </summary>
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
            Polygons.Last().CalcCenterPoint();
            Polygons.Last().RecalWithZoom();
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
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in Polygons)
            {
                if (elem.DL.Level == build)
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < Polygons.Count; i++)
            {
                if (Polygons[i].DL == MainForm.drawLevel & !Polygons[i].delete)
                {
                    for (int j = 0; j < Polygons[i].Points.Count; j++)
                    {
                        _EditRects.Add(new EditRect(Polygons[i].Points[j], 3, i, j));
                    }
                }
            }
            return _EditRects;
        }

        public override void DrawTemp()
        {
            TempPolygon.DrawTemp();
        }
    }
}
