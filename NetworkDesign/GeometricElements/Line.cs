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
                if (((double)Points[0].X * MainForm.Zoom) > (double)(Points[1].X * MainForm.Zoom))
                {
                    maxx = (int)((double)Points[0].X * MainForm.Zoom);
                    minx = (int)((double)Points[1].X * MainForm.Zoom);
                }
                else
                {
                    maxx = (int)((double)Points[1].X * MainForm.Zoom);
                    minx = (int)((double)Points[0].X * MainForm.Zoom);
                }
                if (((double)Points[0].Y * MainForm.Zoom) > (double)(Points[1].Y * MainForm.Zoom))
                {
                    maxy = (int)((double)Points[0].Y * MainForm.Zoom);
                    miny = (int)((double)Points[1].Y * MainForm.Zoom);
                }
                else
                {
                    maxy = (int)((double)Points[1].Y * MainForm.Zoom);
                    miny = (int)((double)Points[0].Y * MainForm.Zoom);
                }
                int lw = (int)MainForm.colorSettings.LineWidth + 1;
                maxx += lw;
                maxy += lw;
                minx -= lw;
                miny -= lw;
                if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                {
                    double A = ((double)Points[1].Y * MainForm.Zoom) - ((double)Points[0].Y * MainForm.Zoom);
                    double B = ((double)Points[0].X * MainForm.Zoom) - ((double)Points[1].X * MainForm.Zoom);
                    double C = ((double)Points[0].X * MainForm.Zoom) * (((double)Points[0].Y * MainForm.Zoom) - ((double)Points[1].Y * MainForm.Zoom)) + ((double)Points[0].Y * MainForm.Zoom) * (((double)Points[1].X * MainForm.Zoom) - ((double)Points[0].X * MainForm.Zoom));
                    double d = Math.Abs(A * x + B * y + C) / Math.Sqrt((A * A) + (B * B));
                    if (d < lw * MainForm.Zoom + 2)
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
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom);
                    }
                    Gl.glPushMatrix();
                    Gl.glScaled(MainForm.Zoom, MainForm.Zoom, MainForm.Zoom);
                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glColor4f(R, G, B, A);
                    Gl.glVertex2d(Points[0].X, Points[0].Y);
                    Gl.glVertex2d(Points[1].X, Points[1].Y);
                    Gl.glEnd();
                    Gl.glPopMatrix();
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

        public override void DrawTemp()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.Zoom);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }
    }
}
