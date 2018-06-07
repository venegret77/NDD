using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class MyRectangle : GeometricFigure
    {
        public Point Point13;
        public Point Point12;
        public Point Point24;
        public Point Point34;
        private int _widthrotate;

        public MyRectangle()
        {
            delete = true;
        }

        public MyRectangle(int x, int y, DrawLevel _drawLevel)
        {
            Points.Add(new Point(x, y));
            Points.Add(new Point(x, y));
            Points.Add(new Point(x, y));
            Points.Add(new Point(x, y));
            DL = _drawLevel;
            delete = false;
        }

        public void Calc13()
        {
            Point13.X = (Points[0].X + Points[2].X) / 2;
            Point13.Y = (Points[0].Y + Points[2].Y) / 2;
        }

        public void Calc12()
        {
            Point12.X = (Points[0].X + Points[1].X) / 2;
            Point12.Y = (Points[0].Y + Points[1].Y) / 2;
        }

        public void Calc24()
        {
            Point24.X = (Points[1].X + Points[3].X) / 2;
            Point24.Y = (Points[1].Y + Points[3].Y) / 2;
        }

        public void Calc34()
        {
            Point34.X = (Points[2].X + Points[3].X) / 2;
            Point34.Y = (Points[2].Y + Points[3].Y) / 2;
        }

        public void CalcTempPoints()
        {
            CalcCenterPoint();
            Calc12();
            Calc13();
            Calc24();
            Calc34();
        }

        private Point NearestPoints(int x, int y, Point p1, Point p2)
        {
            double a = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double c = p1.Y * (p1.X - p2.X) + p1.X * (p2.Y - p1.Y);
            int _x = (int)((b * (b * x - a * y) - a * c) / (Math.Pow(a, 2) + Math.Pow(b, 2)));
            int _y = (int)((a * (-b * x + a * y) - b * c) / (Math.Pow(a, 2) + Math.Pow(b, 2)));
            return new Point(_x, _y);
        }

        private int CalcY(int x, Point p1, Point p2)
        {
            double a = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double c = p1.Y * (p1.X - p2.X) + p1.X * (p2.Y - p1.Y);
            return (int)((-x * a - c) / b);
        }

        public override void SetPoint(int x, int y, int i)
        {
            int Width;
            Point _Point;
            switch (i)
            {
                case 0:
                    Points[1] = new Point(x, y);
                    if (Points[1] == Points[0])
                    {
                        Points[1] = new Point(x + 1, y);
                    }
                    Points[3] = Points[1];
                    alfa = CalcAlfa(Points[0], Points[1]);
                    //Width = (int)CalcWidth(x, y, Points[0], Points[1]);
                    _Point = RotatePoint(-alfa, Points[0], Points[1]);
                    if (Points[0].X <= Points[1].X)
                    {
                        Points[2] = new Point(Points[0].X, Points[0].Y - _widthrotate);
                        Points[3] = new Point(_Point.X, _Point.Y - _widthrotate);
                    }
                    else
                    {
                        Points[2] = new Point(Points[0].X, Points[0].Y + _widthrotate);
                        Points[3] = new Point(_Point.X, _Point.Y + _widthrotate);
                    }
                    Points[2] = RotatePoint(alfa, Points[0], Points[2]);
                    Points[3] = RotatePoint(alfa, Points[0], Points[3]);
                    break;
                case 2:
                    Points[1] = new Point(x, y);
                    if (Points[1] == Points[0])
                    {
                        Points[1] = new Point(x + 1, y);
                    }
                    Points[3] = Points[1];
                    break;
                case 34:
                    alfa = CalcAlfa(Points[0], Points[1]);
                    _widthrotate = Width = (int)CalcWidth(x, y, Points[0], Points[1]);
                    _Point = RotatePoint(-alfa, Points[0], Points[1]);
                    if (Points[0].X <= Points[1].X)
                    {
                        Points[2] = new Point(Points[0].X, Points[0].Y - Width);
                        Points[3] = new Point(_Point.X, _Point.Y - Width);
                    }
                    else
                    {
                        Points[2] = new Point(Points[0].X, Points[0].Y + Width);
                        Points[3] = new Point(_Point.X, _Point.Y + Width);
                    }
                    Points[2] = RotatePoint(alfa, Points[0], Points[2]);
                    Points[3] = RotatePoint(alfa, Points[0], Points[3]);
                    break;
                case 12:
                    alfa = CalcAlfa(Points[2], Points[3]);
                    Width = (int)CalcWidth(x, y, Points[2], Points[3]);
                    _widthrotate = -Width;
                    _Point = RotatePoint(-alfa, Points[2], Points[3]);
                    if (Points[2].X <= Points[3].X)
                    {
                        Points[0] = new Point(Points[2].X, Points[2].Y - Width);
                        Points[1] = new Point(_Point.X, _Point.Y - Width);
                    }
                    else
                    {
                        Points[0] = new Point(Points[2].X, Points[2].Y + Width);
                        Points[1] = new Point(_Point.X, _Point.Y + Width);
                    }
                    Points[0] = RotatePoint(alfa, Points[2], Points[0]);
                    Points[1] = RotatePoint(alfa, Points[2], Points[1]);
                    break;
                case 13:
                    alfa = CalcAlfa(Points[1], Points[3]);
                    Width = (int)CalcWidth(x, y, Points[1], Points[3]);
                    _Point = RotatePoint(-alfa, Points[1], Points[3]);
                    if (Points[1].X <= Points[3].X)
                    {
                        Points[0] = new Point(Points[1].X, Points[1].Y - Width);
                        Points[2] = new Point(_Point.X, _Point.Y - Width);
                    }
                    else
                    {
                        Points[0] = new Point(Points[1].X, Points[1].Y + Width);
                        Points[2] = new Point(_Point.X, _Point.Y + Width);
                    }
                    Points[0] = RotatePoint(alfa, Points[1], Points[0]);
                    Points[2] = RotatePoint(alfa, Points[1], Points[2]);
                    break;
                case 24:
                    alfa = CalcAlfa(Points[0], Points[2]);
                    Width = (int)CalcWidth(x, y, Points[0], Points[2]);
                    _Point = RotatePoint(-alfa, Points[0], Points[2]);
                    if (Points[0].X <= Points[2].X)
                    {
                        Points[1] = new Point(Points[0].X, Points[0].Y - Width);
                        Points[3] = new Point(_Point.X, _Point.Y - Width);
                    }
                    else
                    {
                        Points[1] = new Point(Points[0].X, Points[0].Y + Width);
                        Points[3] = new Point(_Point.X, _Point.Y + Width);
                    }
                    Points[1] = RotatePoint(alfa, Points[0], Points[1]);
                    Points[3] = RotatePoint(alfa, Points[0], Points[3]);
                    break;
            }
        }

        public double CalcAlfa(Point Point1, Point Point2)
        {
            double cat1 = Point2.Y - Point1.Y; //Противолежащий
            double cat2 = Point2.X - Point1.X; //Прилежащий
            double alfa = Math.Atan(cat1 / cat2);
            return alfa;
        }

        private Point RotatePoint(double alfa, Point PointMain, Point PointTemp)
        {
            Point Temp = new Point();
            Temp.X = (int)((PointTemp.X - PointMain.X) * Math.Cos(alfa) - (PointTemp.Y - PointMain.Y) * Math.Sin(alfa) + PointMain.X);
            Temp.Y = (int)((PointTemp.X - PointMain.X) * Math.Sin(alfa) + (PointTemp.Y - PointMain.Y) * Math.Cos(alfa) + PointMain.Y);
            return Temp;
        }

        public double CalcWidth(int x, int y, Point point1, Point point2)
        {
            return ((point2.Y - point1.Y) * x - (point2.X - point1.X) * y + point2.X * point1.Y - point2.Y * point1.X)
                / (Math.Sqrt(Math.Pow((point2.Y - point1.Y), 2) + Math.Pow((point2.X - point1.X), 2)));
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
                        R = (float)MainForm.colorSettings.RectColor.R / 255;
                        G = (float)MainForm.colorSettings.RectColor.G / 255;
                        B = (float)MainForm.colorSettings.RectColor.B / 255;
                        A = (float)MainForm.colorSettings.RectColor.A / 255;
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
                    Gl.glVertex2d(Points[0].X, Points[0].Y);
                    Gl.glVertex2d(Points[1].X, Points[1].Y);
                    Gl.glVertex2d(Points[3].X, Points[3].Y);
                    Gl.glVertex2d(Points[2].X, Points[2].Y);
                    Gl.glEnd();
                    //Gl.glPopMatrix();
                }
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
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glVertex2d(Points[3].X, Points[3].Y);
                Gl.glVertex2d(Points[2].X, Points[2].Y);
                Gl.glEnd();
                Gl.glPopMatrix();
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
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glVertex2d(Points[3].X, Points[3].Y);
                Gl.glVertex2d(Points[2].X, Points[2].Y);
                Gl.glEnd();
                //Gl.glPopMatrix();
            }
        }

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
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glVertex2d(Points[3].X, Points[3].Y);
                Gl.glVertex2d(Points[2].X, Points[2].Y);
                Gl.glEnd();
                //Gl.glPopMatrix();
            }
        }

        public override object Clone()
        {
            List<Point> points = new List<Point>();
            foreach (var p in Points)
            {
                points.Add(new Point(p.X, p.Y));
            }
            return new MyRectangle
            {
                alfa = this.alfa,
                DL = this.DL,
                Points = points,
                CenterPointX = this.CenterPointX,
                CenterPointY = this.CenterPointY,
                delete = false
            };
        }
    }
}
