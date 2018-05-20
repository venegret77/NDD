using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class MyRectangle: GeometricFigure
    {
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

        /*public void Calc13()
        {
            Point13.X = (Point1.X + Point3.X) / 2;
            Point13.Y = (Point1.Y + Point3.Y) / 2;
        }

        public void Calc12()
        {
            Point12.X = (Point1.X + Point2.X) / 2;
            Point12.Y = (Point1.Y + Point2.Y) / 2;
        }

        public void Calc24()
        {
            Point24.X = (Point2.X + Point4.X) / 2;
            Point24.Y = (Point2.Y + Point4.Y) / 2;
        }

        public void Calc34()
        {
            Point34.X = (Point3.X + Point4.X) / 2;
            Point34.Y = (Point3.Y + Point4.Y) / 2;
        }

        public void CalcTempPoints()
        {
            Calc12();
            Calc13();
            Calc24();
            Calc34();
        }*/

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

        /*public void SetPoint13(int x, int y)
        {
            Point point = NearestPoints(x, y, Point1, Point3);
            int difx = x - point.X;
            int dify = y - point.Y;
            int x1 = Point1.X + difx; int y1 = Point1.Y + dify;
            int x2 = Point3.X + difx; int y2 = Point3.Y + dify;
            Point1 = new Point(x1, y1);
            Point3 = new Point(x2, y2);
            /*
            //Создаем временные точки для проверки какая это грань
            //3 точку поворачиваем, чтобы грань лежала ровно относительно оси
            Point _Point1 = Point1; 
            Point _Point3 = RotatePoint(-alfa, Point1, Point3);
            double k = 0d;
            if (Point3.X != Point1.X) 
            {
                //Считаем коэффициент К для прямой, проходящей через 1 и 3 точки
                k = ((double)Point3.Y - (double)Point1.Y) / ((double)Point3.X - (double)Point1.X);
            }
            //Считаем отдельно коэффициент b для обоих точек
            //Считаем оба, т.к. в случае если прямоугольник изначально
            //не наклонен то точки сольются в одну
            double b = (double)Point1.Y - (k * (double)Point1.X);
            double _b = (double)Point3.Y - (k * (double)Point3.X);*/
            //double width = CalcWidth(x, y, Point1, Point3);
            //Проверка К для того, чтобы корректно посчитать новое b для линии
            /*if (k <= 1 & k >= -1 & k != 0)
            {
                b -= width;
                _b -= width;
            }
            else
            {
                //Считаем новое b для линии исходя из формулы
                b += width * k; 
                _b += width * k;
            }
            //Для левой и правой грани
            if (_Point1.X >= _Point3.X - 5 & _Point1.X <= _Point3.X + 5)
            {
                //Меняем X для обоих точек и расчитываем новый Y исходя из уравнения прямой
                Point1.X -= (int)width;
                Point1.Y = (int)((k * (double)Point1.X) + b);
                Point3.X -= (int)width;
                Point3.Y = (int)((k * (double)Point3.X) + _b);
            }
            //Для верхней и нижней грани
            else if (_Point1.Y >= _Point3.Y - 5 & _Point1.Y <= _Point3.Y + 5)
            {
                //Меняем Y для обоих точек и расчитываем новый X исходя из уравнения прямой
                Point1.Y -= (int)width;
                Point1.X = (int)(((double)Point1.Y - b)/k);
                Point3.Y -= (int)width;
                Point3.X = (int)(((double)Point1.Y - _b)/k);
            }*/
        //}

        /*public void SetPoint12(int x, int y)
        {
            /*Point _Point1 = new Point(x, y);
            int difx = _Point1.X - Point1.X;
            int dify = _Point1.Y - Point1.Y;
            Point _Point2 = RotatePoint(-alfa, Point1, Point2);
            Point _Point3 = RotatePoint(-alfa, Point1, Point3);
            if (Point1.X >= _Point3.X - 3 & Point1.X <= _Point3.X + 3)
            {
                _Point3.X = _Point1.X;
                _Point2.Y = _Point1.Y;
            }
            else if (_Point1.Y >= _Point3.Y - 3 & Point1.Y <= _Point3.Y + 3)
            {
                _Point3.Y = _Point1.Y;
                _Point2.X = _Point1.X;
            }
            Point1 = _Point1;
            Point2 = RotatePoint(alfa, _Point1, _Point2);
            Point3 = RotatePoint(alfa, _Point1, _Point3);*/
        //}

        /*public void SetPoint24(int x, int y)
        {
            /*Point3.X = x;
            Point3.Y = y;*/
        //}

        /*public void SetPoint34(int x, int y)
        {
            /*Point4.X = x;
            Point4.Y = y;*/
        //}

        public override void SetPoint(int x, int y, int i)
        {
            switch(i)
            {
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
                    int Width = (int)CalcWidth(x, y, Points[0], Points[1]);
                    Point _Point2 = RotatePoint(-alfa, Points[0], Points[1]);
                    if (Points[0].X <= Points[1].X)
                    {
                        Points[2] = new Point(Points[0].X,Points[0].Y - Width);
                        Points[3] = new Point(_Point2.X, _Point2.Y - Width);
                    }
                    else
                    {
                        Points[2] = new Point(Points[0].X, Points[0].Y + Width);
                        Points[3] = new Point(_Point2.X, _Point2.Y + Width);
                    }
                    Points[2] = RotatePoint(alfa, Points[0], Points[2]);
                    Points[3] = RotatePoint(alfa, Points[0], Points[3]);
                    break;
            }
        }

        /*public void SetPoint3and4(int x, int y)
        {
            alfa = CalcAlfa();
            
            double Width = CalcWidth(x, y, Point1, Point2);
            Point3 = Point1;
            Point4 = _Point2;
            if (Point1.X <= Point2.X)
            {
                Point3.Y = (int)(Point1.Y - Width);
                Point4.Y = (int)(_Point2.Y - Width);
            }
            else
            {
                Point3.Y = (int)(Point1.Y + Width);
                Point4.Y = (int)(_Point2.Y + Width);
            }
            Point3 = RotatePoint(alfa, Point1, Point3);
            Point4 = RotatePoint(alfa, Point1, Point4);
            CalcTempPoints();
        }*/

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
                    res += (int)((double)p.X * MainForm.Zoom) - x;
                    res += (int)((double)p.Y * MainForm.Zoom) - y;
                }
                res = Math.Abs(res);
                int maxx, minx;
                int maxy, miny;
                CalcMaxMin(out maxx, out minx, out maxy, out miny);
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
                    Gl.glVertex2d(Points[0].X, Points[0].Y);
                    Gl.glVertex2d(Points[1].X, Points[1].Y);
                    Gl.glVertex2d(Points[3].X, Points[3].Y);
                    Gl.glVertex2d(Points[2].X, Points[2].Y);
                    Gl.glEnd();
                    Gl.glPopMatrix();
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
                Gl.glVertex2d(Points[0].X, Points[0].Y);
                Gl.glVertex2d(Points[1].X, Points[1].Y);
                Gl.glVertex2d(Points[3].X, Points[3].Y);
                Gl.glVertex2d(Points[2].X, Points[2].Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }
    }
}
