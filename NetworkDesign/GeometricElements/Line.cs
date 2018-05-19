using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class Line : GeometricFigure
    {
        public Line()
        {
            delete = true;
        }

        public Line(int x, int y, DrawLevel _drawLevel)
        {
            Points.Add(new Point(x, y));
            Points.Add(new Point(x, y));
            DL = _drawLevel;
            delete = false;
        }

        public override void SetPoint(int x, int y, int i)
        {
            Points[i] = new Point(x, y);
        }

        public override double Search(int x, int y)
        {

            if (!delete)
            {
                int maxx = 0, minx = 0, maxy = 0, miny = 0;
                if (Points[0].X > Points[1].X)
                {
                    maxx = Points[0].X;
                    minx = Points[1].X;
                }
                else
                {
                    maxx = Points[1].X;
                    minx = Points[0].X;
                }
                if (Points[0].Y > Points[1].Y)
                {
                    maxy = Points[0].Y;
                    miny = Points[1].Y;
                }
                else
                {
                    maxy = Points[1].Y;
                    miny = Points[0].Y;
                }
                int lw = (int)MainForm.colorSettings.LineWidth + 1;
                maxx += lw;
                maxy += lw;
                minx -= lw;
                miny -= lw;
                if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                {
                    double A = Points[1].Y - Points[0].Y;
                    double B = Points[0].X - Points[1].X;
                    double C = Points[0].X * (double)(Points[0].Y - Points[1].Y) + (double)Points[0].Y * (double)(Points[1].X - Points[0].X);
                    double d = Math.Abs(A * x + B * y + C) / Math.Sqrt((A * A) + (B * B));
                    if (d < lw + 2)
                        return d;
                    else
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
                        R = (float)MainForm.colorSettings.LinesColor.R / 255;
                        G = (float)MainForm.colorSettings.LinesColor.G / 255;
                        B = (float)MainForm.colorSettings.LinesColor.B / 255;
                        A = (float)MainForm.colorSettings.LinesColor.A / 255;
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
                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glColor4f(R, G, B, A);
                    Gl.glVertex2d(Points[0].X, Points[0].Y);
                    Gl.glVertex2d(Points[1].X, Points[1].Y);
                    Gl.glEnd();
                }
            }
        }

        public override void DrawB()
        {
            throw new NotImplementedException();
        }

        public override void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny)
        {
            throw new NotImplementedException();
        }
    }
}
