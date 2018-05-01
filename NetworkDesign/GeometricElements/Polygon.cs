using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class Polygon: GeometricFigure
    {
        public Polygon()
        {
            delete = true;
        }

        public Polygon(int x, int y, DrawLevel _drawLevel)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
            Points.Add(TempPoint);
            ClearTempPoint();
            DL = _drawLevel;
            delete = false;
        }

        public void SetTempPoint(int x, int y)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
        }

        public override void SetPoint(int x, int y, int i)
        {
            Points[i] = new Point(x, y);
        }

        public void AddPoint()
        {
            Points.Add(TempPoint);
            ClearTempPoint();
        }

        public void ClearTempPoint()
        {
            TempPoint = new Point();
        }

        public override void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny)
        {
            maxx = Points[0].X; minx = Points[0].X;
            maxy = Points[0].Y; miny = Points[0].Y;
            foreach (var point in Points)
            {
                if (point.X > maxx)
                    maxx = point.X;
                if (point.X < minx)
                    minx = point.X;
                if (point.Y > maxy)
                    maxy = point.Y;
                if (point.Y < miny)
                    miny = point.Y;
            }
        }

        public override double Search(int x, int y)
        {
            if (!delete)
            {
                double res = 0;
                CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
                foreach (var point in Points)
                {
                    res += point.X - x + point.Y - y;
                }
                res = Math.Abs(res);
                if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                {
                    return res;
                }
                else
                {
                    return -1;
                }
            }
            return -1;
        }

        public override void Draw()
        {
            if (!delete)
            {
                if (DL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.PolygonColor.R / 255;
                        G = (float)MainForm.colorSettings.PolygonColor.G / 255;
                        B = (float)MainForm.colorSettings.PolygonColor.B / 255;
                        A = (float)MainForm.colorSettings.PolygonColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth + 1);
                    }
                    Gl.glBegin(Gl.GL_LINE_LOOP);
                    Gl.glColor4f(R, G, B, A);
                    foreach (var point in Points)
                    {
                        Gl.glVertex2d(point.X, point.Y);
                    }
                    if (TempPoint.X != 0 & TempPoint.Y != 0)
                        Gl.glVertex2d(TempPoint.X, TempPoint.Y);
                    Gl.glEnd();
                }
            }
        }

        public override void DrawB()
        {
            if (!delete)
            {
                if (!active)
                {
                    R = (float)MainForm.colorSettings.BuildColor.R / 255;
                    G = (float)MainForm.colorSettings.BuildColor.G / 255;
                    B = (float)MainForm.colorSettings.BuildColor.B / 255;
                    A = (float)MainForm.colorSettings.BuildColor.A / 255;
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth);
                }
                else
                {
                    R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                    G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                    B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                    A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth + 1);
                }
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(R, G, B, A);
                foreach (var point in Points)
                {
                    Gl.glVertex2d(point.X, point.Y);
                }
                if (TempPoint.X != 0 & TempPoint.Y != 0)
                    Gl.glVertex2d(TempPoint.X, TempPoint.Y);
                Gl.glEnd();
            }
        }
    }
}
