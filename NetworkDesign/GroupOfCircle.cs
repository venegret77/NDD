using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfCircle
    {
        public List<Circle> Circles = new List<Circle>();
        public Circle TempCircle = new Circle();
        public bool step_circle = false;

        public GroupOfCircle()
        {

        }

        public void TempDefault()
        {
            TempCircle = new Circle();
            step_circle = false;
        }

        public void Add(Circle circle)
        {
            Circles.Add(circle);
        }

        public void Remove(int i)
        {
            Circles[i] = new Circle();
        }

        public void Choose(int i)
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

        public int CalcNearestCicrcle(int x, int y, out double dist, DrawLevel dl)
        {
            int index = -1;
            dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                if (dl == Circles[i].MainDL)
                {
                    double _dist = Circles[i].PointInCircle(x, y);
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

        public int CalcNearestCicrcleEnt(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                if (dl == Circles[i].MainDL)
                {
                    double _dist = Circles[i].PointInCircleEnt(x, y);
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

        public int CalcNearestCicrcleIW(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                if (dl == Circles[i].MainDL)
                {
                    double _dist = Circles[i].PointInCircleIW(x, y);
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

        /*public int SearchLine(int x, int y, DrawLevel dl)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (dl.Level == Lines[i].DL.Level & dl.Floor == Lines[i].DL.Floor)
                {
                    if (Lines[i].Search(x, y))
                        return i;
                }
            }
            return -1;
        }*/

        public void Draw()
        {
            foreach (var circle in Circles)
            {
                circle.Draw();
            }
            TempCircle.Draw();
        }

        public void DrawEnt()
        {
            foreach (var circle in Circles)
            {
                circle.DrawEnt();
            }
            TempCircle.DrawEnt();
        }

        public void DrawIW()
        {
            foreach (var circle in Circles)
            {
                circle.DrawIW();
            }
            TempCircle.DrawIW();
        }
    }
}
