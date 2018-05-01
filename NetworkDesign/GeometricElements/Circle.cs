using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class Circle: GeometricFigure
    {
        public Point MainCenterPoint;
        public Point LocalCencerPoint;
        public int radius;
        public DrawLevel MainDL;
        public DrawLevel LocalDL;
        public double koef;

        public Circle()
        {
            delete = true;
        }

        public Circle(int x, int y, DrawLevel _DL)
        {
            MainCenterPoint = new Point(x, y);
            radius = 1;
            MainDL = _DL;
            delete = false;
        }

        public Circle(int x, int y, int _radius, DrawLevel _DL)
        {
            MainCenterPoint = new Point(x, y);
            radius = _radius;
            MainDL = _DL;
            delete = false;
        }

        public Circle(int x, int y, DrawLevel _MDL, DrawLevel _LDL)
        {
            MainCenterPoint = new Point(x, y);
            radius = 1;
            MainDL = _MDL;
            LocalDL = _LDL;
            delete = false;
        }

        public Circle(int x, int y, int _radius, DrawLevel _MDL, DrawLevel _LDL)
        {
            MainCenterPoint = new Point(x, y);
            radius = _radius;
            MainDL = _MDL;
            LocalDL = _LDL;
            delete = false;
        }

        public override double Search(int x, int y)
        {
            if (Math.Pow((x - MainCenterPoint.X), 2) + Math.Pow((y - MainCenterPoint.Y), 2) <= Math.Pow(radius,2))
                return Math.Abs(MainCenterPoint.X - x + MainCenterPoint.Y - y);
            else
                return -1;
        }

        public double SearchEnt(int x, int y)
        {
            if (Math.Pow((x - MainCenterPoint.X), 2) + Math.Pow((y - MainCenterPoint.Y), 2) <= Math.Pow(MainForm.colorSettings.EntranceRadius, 2))
                return Math.Abs(MainCenterPoint.X - x + MainCenterPoint.Y - y);
            else
                return -1;
        }

        public double SearchIW(int x, int y)
        {
            if (Math.Pow((x - MainCenterPoint.X), 2) + Math.Pow((y - MainCenterPoint.Y), 2) <= Math.Pow(MainForm.colorSettings.InputWireRadius, 2))
                return Math.Abs(MainCenterPoint.X - x + MainCenterPoint.Y - y);
            else
                return -1;
        }

        public void SetRadius(int x, int y)
        {
            radius = (int)Math.Sqrt(Math.Pow((x - MainCenterPoint.X), 2) + Math.Pow((y - MainCenterPoint.Y), 2));
        }

        public override void Draw()
        {
            if (!delete)
            {
                if (MainDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.CircleColor.R / 255;
                        G = (float)MainForm.colorSettings.CircleColor.G / 255;
                        B = (float)MainForm.colorSettings.CircleColor.B / 255;
                        A = (float)MainForm.colorSettings.CircleColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_LINE_LOOP);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    //Gl.glVertex2d(CenterPoint.X, CenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = radius * Math.Cos(angle * Math.PI / 180);
                        double y = radius * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                    }
                    Gl.glEnd();
                }
            }
        }

        public void DrawEnt()
        {
            if (!delete)
            {
                if (MainDL == MainForm.drawLevel)
                {

                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.EntranceColor.R / 255;
                        G = (float)MainForm.colorSettings.EntranceColor.G / 255;
                        B = (float)MainForm.colorSettings.EntranceColor.B / 255;
                        A = (float)MainForm.colorSettings.EntranceColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(MainCenterPoint.X, MainCenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.EntranceRadius * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.EntranceRadius * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                    }
                    Gl.glEnd();
                }
                else if (LocalDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.EntranceColor.R / 255;
                        G = (float)MainForm.colorSettings.EntranceColor.G / 255;
                        B = (float)MainForm.colorSettings.EntranceColor.B / 255;
                        A = (float)MainForm.colorSettings.EntranceColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(LocalCencerPoint.X, LocalCencerPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.EntranceRadius /** koef*/ * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.EntranceRadius /** koef*/ * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + LocalCencerPoint.X, y + LocalCencerPoint.Y);
                    }
                    Gl.glEnd();
                }
            }
        }

        public void DrawIW()
        {
            if (!delete)
            {
                if (MainDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.InputWireColor.R / 255;
                        G = (float)MainForm.colorSettings.InputWireColor.G / 255;
                        B = (float)MainForm.colorSettings.InputWireColor.B / 255;
                        A = (float)MainForm.colorSettings.InputWireColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(MainCenterPoint.X, MainCenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.InputWireRadius * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.InputWireRadius * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                    }
                    Gl.glEnd();
                }
                else if (LocalDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.InputWireColor.R / 255;
                        G = (float)MainForm.colorSettings.InputWireColor.G / 255;
                        B = (float)MainForm.colorSettings.InputWireColor.B / 255;
                        A = (float)MainForm.colorSettings.InputWireColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(LocalCencerPoint.X, LocalCencerPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.InputWireRadius /** koef*/ * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.InputWireRadius /** koef*/ * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + LocalCencerPoint.X, y + LocalCencerPoint.Y);
                    }
                    Gl.glEnd();
                }
            }
        }

        public override void SetPoint(int x, int y, int i)
        {
            throw new NotImplementedException();
        }

        public override void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny)
        {
            throw new NotImplementedException();
        }

        public override void DrawB()
        {
            if (!delete)
            {
                if (MainDL == MainForm.drawLevel)
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_LINE_LOOP);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    //Gl.glVertex2d(CenterPoint.X, CenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = radius * Math.Cos(angle * Math.PI / 180);
                        double y = radius * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                    }
                    Gl.glEnd();
                }
            }
        }
    }
}
