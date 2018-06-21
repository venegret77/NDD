using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Группа кругов
    /// </summary>
    public class GroupOfCircle: GroupOfElements , ICloneable
    {
        /// <summary>
        /// Список кругов
        /// </summary>
        public List<Circle> Circles = new List<Circle>();
        /// <summary>
        /// Временный круг
        /// </summary>
        public Circle TempCircle = new Circle();

        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupOfCircle()
        {

        }

        public override void TempDefault()
        {
            TempCircle = new Circle();
            step = false;
        }

        public override void Add(object elem)
        {
            Circles.Add((Circle)elem);
            //Circles.Last().CalcCenterPoint();
            Circles.Last().RecalWithZoom();
        }

        public override void Remove(int i)
        {
            Circles[i] = new Circle();
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < Circles.Count; j++)
            {
                if (j != i)
                {
                    Circles[j].SetActive(false);
                }
                else
                {
                    Circles[j].SetActive(true);
                }
            }
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            int index = -1;
            dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                if (dl == Circles[i].MainDL & !Circles[i].delete)
                {
                    double _dist = Circles[i].Search(x, y);
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
        /// <summary>
        /// Поиск входа в здание
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns>Возвращает идентификатор ближайшего входа в здание</returns>
        public int SearchEnt(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                //Внутри
                if (dl.Level != -1)
                {
                    if (dl == Circles[i].LocalDL & !Circles[i].delete)
                    {
                        double _dist = Circles[i].SearchEntInBuild(x, y);
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
                //Снаружи
                else
                {
                    if (dl == Circles[i].MainDL & !Circles[i].delete)
                    {
                        double _dist = Circles[i].SearchEnt(x, y);
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
            }
            return index;
        }
        /// <summary>
        /// Поиск входа провода в здание
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns>Возвращает идентификатор ближайшего входа провода в здание</returns>
        public int SearchIW(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                //Внутри
                if (dl.Level != -1 & !Circles[i].delete)
                {
                    double _dist = Circles[i].SearchIWInBuild(x, y, dl);
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
                //Снаружи
                else
                {
                    if (dl == Circles[i].MainDL & !Circles[i].delete)
                    {
                        double _dist = Circles[i].SearchIW(x, y);
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
            }
            return index;
        }

        public override void Draw()
        {
            foreach (var circle in Circles)
            {
                circle.Draw();
            }
        }
        /// <summary>
        /// Отрисовка входов в здание
        /// </summary>
        public void DrawEnt()
        {
            foreach (var circle in Circles)
            {
                circle.DrawEnt();
            }
        }
        /// <summary>
        /// Отрисовка входов проводов в здание
        /// </summary>
        public void DrawIW()
        {
            foreach (var circle in Circles)
            {
                circle.DrawIW();
            }
        }
        /// <summary>
        /// Отрисовка временного входа в здание
        /// </summary>
        public void DrawTempEnt()
        {
            TempCircle.DrawTempEnt();
        }
        /// <summary>
        /// Отрисовка временного входа провода в здание
        /// </summary>
        public void DrawTempIW()
        {
            TempCircle.DrawTempIW();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in Circles)
            {
                if (elem.DL.Level == build & !elem.delete)
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < Circles.Count; i++)
            {
                if (Circles[i].MainDL == MainForm.drawLevel &!Circles[i].delete)
                    _EditRects.Add(new EditRect(new Point(Circles[i].MainCenterPoint.X + Circles[i].radius, Circles[i].MainCenterPoint.Y), 360, i, 0));
            }
            return _EditRects;
        }

        public override void DrawTemp()
        {
            TempCircle.DrawTemp();
        }

        public object Clone()
        {
            List<Circle> circles = new List<Circle>();
            foreach (var c in Circles)
            {
                circles.Add((Circle)c.Clone());
            }
            return new GroupOfCircle
            {
                Circles = circles
            };
        }
    }
}
