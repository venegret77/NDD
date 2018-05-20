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

        public override double Search(int x, int y)
        {
            if (!delete)
            {
                double res = 0;
                foreach (var p in Points)
                {
                    res += (int)((double)p.X * MainForm.Zoom) - x;
                    res += (int)((double)p.Y * MainForm.Zoom) - y;
                }
                res = Math.Abs(res);
                CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
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
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom + 1);
                    }
                    Gl.glPushMatrix();
                    Gl.glScaled(MainForm.Zoom, MainForm.Zoom, MainForm.Zoom);
                    Gl.glBegin(Gl.GL_LINE_LOOP);
                    Gl.glColor4f(R, G, B, A);
                    foreach (var point in Points)
                        Gl.glVertex2d(point.X, point.Y);
                    Gl.glEnd();
                    Gl.glPopMatrix();
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
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom);
                }
                else
                {
                    R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                    G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                    B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                    A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom + 1);
                }
                Gl.glPushMatrix();
                Gl.glScaled(MainForm.Zoom, MainForm.Zoom, MainForm.Zoom);
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(R, G, B, A);
                foreach (var point in Points)
                    Gl.glVertex2d(point.X, point.Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        public override void DrawTemp()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                foreach (var point in Points)
                    Gl.glVertex2d(point.X, point.Y);
                Gl.glVertex2d(TempPoint.X, TempPoint.Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }
    }
}
