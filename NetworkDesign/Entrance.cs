﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class Entrances
    {
        public GroupOfCircle Enterances = new GroupOfCircle();
        private int rad = 5;
        public bool step = false;
        
        public Entrances()
        {

        }

        public void Add()
        {
            Enterances.Add(Enterances.TempCircle);
            Enterances.TempCircle = new Circle();
        }

        public void AddTemp(int x, int y, DrawLevel MDL, DrawLevel LDL)
        {
            Enterances.TempCircle = new Circle(x, y, rad, MDL, LDL);
        }

        public int CalcNearestEnterise(int x, int y, DrawLevel dl)
        {
            return Enterances.CalcNearestCicrcleEnt(x, y, dl);
        }

        private double CalcPointToLine(int x, int y, Point point1, Point point2)
        {
            return Math.Abs(((point2.Y - point1.Y) * x - (point2.X - point1.X) * y + point2.X * point1.Y - point2.Y * point1.X)
                / (Math.Sqrt(Math.Pow((point2.Y - point1.Y), 2) + Math.Pow((point2.X - point1.X), 2))));
        }

        private Point CalcNearestPoint(int x, int y, Point p1, Point p2)
        {
            double a = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double c = p1.Y * (p1.X - p2.X) + p1.X * (p2.Y - p1.Y);
            int _x = (int)((b * (b * x - a * y) - a * c) / (Math.Pow(a, 2) + Math.Pow(b, 2)));
            int _y = (int)((a * (-b * x + a * y) - b * c) / (Math.Pow(a, 2) + Math.Pow(b, 2)));
            return new Point(_x, _y);
        }

        public void NearestPoints(int x, int y, Rectangle rect)
        {
            double d = double.MaxValue;
            int _d = 0;
            double d1 = CalcPointToLine(x, y, rect.Points[0], rect.Points[1]);
            if (d1 < d)
            {
                d = d1;
                _d = 1;
            }
            double d2 = CalcPointToLine(x, y, rect.Points[1], rect.Points[3]);
            if (d2 < d)
            {
                d = d2;
                _d = 2;
            }
            double d3 = CalcPointToLine(x, y, rect.Points[3], rect.Points[2]);
            if (d3 < d)
            {
                d = d3;
                _d = 3;
            }
            double d4 = CalcPointToLine(x, y, rect.Points[2], rect.Points[0]);
            if (d4 < d)
            {
                d = d4;
                _d = 4;
            }
            switch (_d)
            {
                case 1:
                    if (CheckInterval(x, y, rect.Points[0], rect.Points[1]))
                        Enterances.TempCircle.MainCenterPoint = CalcNearestPoint(x, y, rect.Points[0], rect.Points[1]);
                    break;
                case 2:
                    if (CheckInterval(x, y, rect.Points[1], rect.Points[3]))
                        Enterances.TempCircle.MainCenterPoint = CalcNearestPoint(x, y, rect.Points[1], rect.Points[3]);
                    break;
                case 3:
                    if (CheckInterval(x, y, rect.Points[3], rect.Points[2]))
                        Enterances.TempCircle.MainCenterPoint = CalcNearestPoint(x, y, rect.Points[3], rect.Points[2]);
                    break;
                case 4:
                    if (CheckInterval(x, y, rect.Points[2], rect.Points[0]))
                        Enterances.TempCircle.MainCenterPoint = CalcNearestPoint(x, y, rect.Points[2], rect.Points[0]);
                    break;
            }
        }

        public void NearestPoints(int x, int y, Polygon pol)
        {
            double d = double.MaxValue;
            int p1 = 0, p2 = 0;
            double _d;
            for (int i = 0; i < pol.Points.Count - 1; i++)
            {
                _d = CalcPointToLine(x, y, pol.Points[i], pol.Points[i + 1]);
                if (_d < d)
                {
                    d = _d;
                    p1 = i;
                    p2 = i + 1;
                }
            }
            _d = CalcPointToLine(x, y, pol.Points[pol.Points.Count - 1], pol.Points[1]);
            if (_d < d)
            {
                d = _d;
                p1 = pol.Points.Count - 1;
                p2 = 0;
            }
            if (CheckInterval(x,y, pol.Points[p1], pol.Points[p2]))
                Enterances.TempCircle.MainCenterPoint = CalcNearestPoint(x, y, pol.Points[p1], pol.Points[p2]);
        }

        private bool CheckInterval(int x, int y, Point p1, Point p2)
        {
            int minx, miny, maxx, maxy;
            if (p1.X >= p2.X)
            {
                maxx = p1.X;
                minx = p2.X;
            }
            else
            {
                maxx = p2.X;
                minx = p1.X;
            }
            if (p1.Y >= p2.Y)
            {
                maxy = p1.Y;
                miny = p2.Y;
            }
            else
            {
                maxy = p2.Y;
                miny = p1.Y;
            }
            if (maxx - minx < 10)
            {
                maxx += 10;
                minx -= 10;
            }
            if (maxy - miny < 10)
            {
                maxy += 10;
                miny -= 10;
            }
            if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                return true;
            else
                return false;
        }

        public void Draw()
        {
            Enterances.DrawEnt();
        }
    }
}
