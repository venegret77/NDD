﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfCircle: GroupOfElements
    {
        public List<Circle> Circles = new List<Circle>();
        public Circle TempCircle = new Circle();

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
                if (dl == Circles[i].MainDL)
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

        public int SearchEnt(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                //Внутри
                if (dl.Level != -1)
                {
                    if (dl == Circles[i].LocalDL)
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
                    if (dl == Circles[i].MainDL)
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

        public int SearchIW(int x, int y, DrawLevel dl)
        {
            int index = -1;
            double dist = Int32.MaxValue;
            for (int i = 0; i < Circles.Count; i++)
            {
                //Внутри
                if (dl.Level != -1)
                {
                    if (dl == Circles[i].LocalDL)
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
                    if (dl == Circles[i].MainDL)
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
            TempCircle.DrawTemp();
        }

        public void DrawEnt()
        {
            foreach (var circle in Circles)
            {
                circle.DrawEnt();
            }
            TempCircle.DrawTempEnt();
        }

        public void DrawIW()
        {
            foreach (var circle in Circles)
            {
                circle.DrawIW();
            }
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
            for (int i = 0; i < Circles.Count; i++)
            {
                if (Circles[i].MainDL == MainForm.drawLevel &!Circles[i].delete)
                    _EditRects.Add(new EditRect(new Point(Circles[i].MainCenterPoint.X + Circles[i].radius, Circles[i].MainCenterPoint.Y), 360, i, 0));
            }
            return _EditRects;
        }
    }
}
