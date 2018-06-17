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
    /// Многоугольник
    /// </summary>
    public class Polygon: GeometricFigure
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Polygon()
        {
            delete = true;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="_drawLevel">Уровень отображения</param>
        public Polygon(int x, int y, DrawLevel _drawLevel)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
            Points.Add(TempPoint);
            ClearTempPoint();
            DL = _drawLevel;
            delete = false;
        }
        /// <summary>
        /// Установка временной точки 
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        public void SetTempPoint(int x, int y)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
        }

        public override void SetPoint(int x, int y, int i)
        {
            Points[i] = new Point(x, y);
        }
        /// <summary>
        /// Добавление точки
        /// </summary>
        public void AddPoint()
        {
            Points.Add(TempPoint);
            TempPoint = new Point(Points.Last().X, Points.Last().Y);
        }
        /// <summary>
        /// Удаление временной точки
        /// </summary>
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
                    res += (int)((double)p.X * MainForm.zoom) - x;
                    res += (int)((double)p.Y * MainForm.zoom) - y;
                }
                res = Math.Abs(res);
                CalcMaxMinWidthZoom(out int maxx, out int minx, out int maxy, out int miny);
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
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom + 1);
                    }
                    //Gl.glPushMatrix();
                    //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                    Gl.glBegin(Gl.GL_LINE_LOOP);
                    Gl.glColor4f(R, G, B, A);
                    foreach (var point in Points)
                        Gl.glVertex2d(point.X, point.Y);
                    Gl.glEnd();
                    //Gl.glPopMatrix();
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
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                }
                else
                {
                    R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                    G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                    B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                    A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                    Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom + 1);
                }
                //Gl.glPushMatrix();
                //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(R, G, B, A);
                foreach (var point in Points)
                    Gl.glVertex2d(point.X, point.Y);
                Gl.glEnd();
                //Gl.glPopMatrix();
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
        /// <summary>
        /// Отрисовка здания с заданным коэффициентом
        /// </summary>
        /// <param name="koef">Коэффициент</param>
        internal void DrawB(double koef)
        {
            if (!delete)
            {
                R = (float)MainForm.colorSettings.BuildColor.R / 255;
                G = (float)MainForm.colorSettings.BuildColor.G / 255;
                B = (float)MainForm.colorSettings.BuildColor.B / 255;
                A = (float)MainForm.colorSettings.BuildColor.A / 255;
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom * (float)koef);
                //Gl.glPushMatrix();
                //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(R, G, B, A);
                foreach (var point in Points)
                    Gl.glVertex2d(point.X, point.Y);
                Gl.glEnd();
                //Gl.glPopMatrix();
            }
        }
        /// <summary>
        /// Добавление новой точки
        /// </summary>
        internal void AddNewPoint()
        {
            int x = (Points.Last().X + Points[0].X) / 2;
            int y = (Points.Last().Y + Points[0].Y) / 2;
            Points.Add(new Point(x, y));
        }
        /// <summary>
        /// Удаление последней точки
        /// </summary>
        internal void RemovePoint()
        {
            if (Points.Count > 3)
                Points.RemoveAt(Points.Count - 1);
        }

        public override object Clone()
        {
            List<Point> points = new List<Point>();
            foreach (var p in Points)
            {
                points.Add(new Point(p.X, p.Y));
            }
            return new Polygon
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
