using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Группа зданий
    /// </summary>
    public class GroupOfBuildings: GroupOfElements
    {
        /// <summary>
        /// Список зданий
        /// </summary>
        public List<Building> Buildings = new List<Building>();
        /// <summary>
        /// Конструктор
        /// </summary>
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
        /// <summary>
        /// Проверка входа провода
        /// </summary>
        /// <param name="build">Идентификатор зданий</param>
        /// <param name="networkWires">Группа проводов</param>
        public void CheckIW(int build, GroupOfNW networkWires)
        {
            int i = 0;
            foreach (var iw in Buildings[build].InputWires.InputWires.Circles)
            {
                Point point = iw.MainCenterPoint;
                Point _point = iw.LocalCenterPoint;
                networkWires.CheckNW(point.X, point.Y, i, true, build, iw.MainDL);
                networkWires.CheckNW(_point.X, _point.Y, i, true, build, iw.LocalDL);
                i++;
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

        public override void DrawTemp()
        {
            foreach (var build in Buildings)
            {
                build.DrawTemp();
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

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < Buildings.Count; i++)
            {
                if (Buildings[i].MainMapDL == MainForm.drawLevel & !Buildings[i].delete)
                {
                    if (Buildings[i].type == 3)
                    {
                        _EditRects.Add(new EditRect(Buildings[i].MainPolygon.Points[0], 4, i, 3));
                    }
                    else if (Buildings[i].type == 2)
                    {
                        _EditRects.Add(new EditRect(Buildings[i].MainRectangle.Points[0], 4, i, 2));
                    }
                    /*else if (Buildings[i].type == 360)
                    {
                        Point p = new Point(Buildings[i].MainCircle.MainCenterPoint.X + Buildings[i].MainCircle.radius, Buildings[i].MainCircle.MainCenterPoint.Y);
                        _EditRects.Add(new EditRect(p, 3, i, 360));
                    }*/
                }
            }
            return _EditRects;
        }
    }
}
