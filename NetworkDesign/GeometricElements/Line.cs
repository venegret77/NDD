using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    /// <summary>
    /// Линия
    /// </summary>
    public class Line : GeometricFigure
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Line()
        {
            delete = true;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="_drawLevel">Уровень отображения</param>
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
                if (((double)Points[0].X * MainForm.zoom) > (double)(Points[1].X * MainForm.zoom))
                {
                    maxx = (int)((double)Points[0].X * MainForm.zoom);
                    minx = (int)((double)Points[1].X * MainForm.zoom);
                }
                else
                {
                    maxx = (int)((double)Points[1].X * MainForm.zoom);
                    minx = (int)((double)Points[0].X * MainForm.zoom);
                }
                if (((double)Points[0].Y * MainForm.zoom) > (double)(Points[1].Y * MainForm.zoom))
                {
                    maxy = (int)((double)Points[0].Y * MainForm.zoom);
                    miny = (int)((double)Points[1].Y * MainForm.zoom);
                }
                else
                {
                    maxy = (int)((double)Points[1].Y * MainForm.zoom);
                    miny = (int)((double)Points[0].Y * MainForm.zoom);
                }
                int lw = (int)MainForm.colorSettings.LineWidth + 1;
                maxx += lw;
                maxy += lw;
                minx -= lw;
                miny -= lw;
                if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                {
                    double A = ((double)Points[1].Y * MainForm.zoom) - ((double)Points[0].Y * MainForm.zoom);
                    double B = ((double)Points[0].X * MainForm.zoom) - ((double)Points[1].X * MainForm.zoom);
                    double C = ((double)Points[0].X * MainForm.zoom) * (((double)Points[0].Y * MainForm.zoom) - ((double)Points[1].Y * MainForm.zoom)) + ((double)Points[0].Y * MainForm.zoom) * (((double)Points[1].X * MainForm.zoom) - ((double)Points[0].X * MainForm.zoom));
                    double d = Math.Abs(A * x + B * y + C) / Math.Sqrt((A * A) + (B * B));
                    if (d < lw * MainForm.zoom + 2)
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
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                    }
                    //Gl.glPushMatrix();
                    //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glColor4f(R, G, B, A);
                    Gl.glVertex2d(Points[0].X, Points[0].Y);
                    Gl.glVertex2d(Points[1].X, Points[1].Y);
                    Gl.glEnd();
                    //Gl.glPopMatrix();
                }
            }
        }

        public override void DrawB()
        {
            throw new NotImplementedException();
        }

        public override void CalcMaxMinWidthZoom(out int maxx, out int minx, out int maxy, out int miny)
        {
            throw new NotImplementedException();
        }

        public override void DrawTemp()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        public override object Clone()
        {
            List<Point> points = new List<Point>();
            foreach (var p in Points)
            {
                points.Add(new Point(p.X, p.Y));
            }
            return new Line
            {
                alfa = this.alfa,
                DL = this.DL,
                Points = points,
                CenterPointX = this.CenterPointX,
                CenterPointY = this.CenterPointY,
                delete = this.delete
            };
        }
    }
}
