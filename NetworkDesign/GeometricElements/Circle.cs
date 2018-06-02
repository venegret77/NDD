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
        public Point LocalCenterPoint;
        public int radius;
        public DrawLevel MainDL;
        public DrawLevel LocalDL;
        public double koef;
        internal bool side;

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

        public Circle(int x, int y, int _radius, DrawLevel _MDL, DrawLevel _LDL, bool side)
        {
            MainCenterPoint = new Point(x, y);
            radius = _radius;
            MainDL = _MDL;
            LocalDL = _LDL;
            delete = false;
            this.side = side;
        }

        public override double Search(int x, int y)
        {
            if (Math.Pow((x - (double)MainCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)MainCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)radius * MainForm.zoom,2))
                return Math.Abs((double)MainCenterPoint.X * MainForm.zoom - x + (double)MainCenterPoint.Y * MainForm.zoom - y);
            else
                return -1;
        }

        public double SearchEnt(int x, int y)
        {
            if (Math.Pow((x - (double)MainCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)MainCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)MainForm.colorSettings.EntranceRadius * MainForm.zoom, 2))
                return Math.Abs((double)MainCenterPoint.X * MainForm.zoom - x + (double)MainCenterPoint.Y * MainForm.zoom - y);
            else
                return -1;
        }

        public double SearchIW(int x, int y)
        {
            if (Math.Pow((x - (double)MainCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)MainCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)MainForm.colorSettings.InputWireRadius * MainForm.zoom, 2))
                return Math.Abs((double)MainCenterPoint.X * MainForm.zoom - x + (double)MainCenterPoint.Y * MainForm.zoom - y);
            else
                return -1;
        }

        internal double SearchEntInBuild(int x, int y)
        {
            if (Math.Pow((x - (double)LocalCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)LocalCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)MainForm.colorSettings.EntranceRadius * MainForm.zoom, 2))
                return Math.Abs((double)LocalCenterPoint.X * MainForm.zoom - x + (double)LocalCenterPoint.Y * MainForm.zoom - y);
            else
                return -1;
        }

        public double SearchIWInBuild(int x, int y, DrawLevel dl)
        {
            if (dl == LocalDL)
            {
                if (Math.Pow((x - (double)LocalCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)LocalCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)MainForm.colorSettings.InputWireRadius * MainForm.zoom, 2))
                    return Math.Abs((double)LocalCenterPoint.X * MainForm.zoom - x + (double)LocalCenterPoint.Y * MainForm.zoom - y);
            }
            else if (dl == MainDL)
            {
                if (Math.Pow((x - (double)MainCenterPoint.X * MainForm.zoom), 2) + Math.Pow((y - (double)MainCenterPoint.Y * MainForm.zoom), 2) <= Math.Pow((double)MainForm.colorSettings.InputWireRadius * MainForm.zoom, 2))
                    return Math.Abs((double)MainCenterPoint.X * MainForm.zoom - x + (double)MainCenterPoint.Y * MainForm.zoom - y);
            }
            return -1;
        }

        public void SetRadius(int x, int y)
        {
            radius = (int)Math.Sqrt(Math.Pow((x - MainCenterPoint.X), 2) + Math.Pow((y - MainCenterPoint.Y), 2));
        }

        public override void Draw()
        {
            unsafe
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
                        //Gl.glPopMatrix();
                    }
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
                    //Gl.glPopMatrix();
                }
                else if (LocalDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.EntranceColor.R / 255;
                        G = (float)MainForm.colorSettings.EntranceColor.G / 255;
                        B = (float)MainForm.colorSettings.EntranceColor.B / 255;
                        A = (float)MainForm.colorSettings.EntranceColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(LocalCenterPoint.X, LocalCenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.EntranceRadius /** koef*/ * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.EntranceRadius /** koef*/ * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + LocalCenterPoint.X, y + LocalCenterPoint.Y);
                    }
                    Gl.glEnd();
                    //Gl.glPopMatrix();
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
                    //Gl.glPopMatrix();
                }
                else if (LocalDL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        R = (float)MainForm.colorSettings.InputWireColor.R / 255;
                        G = (float)MainForm.colorSettings.InputWireColor.G / 255;
                        B = (float)MainForm.colorSettings.InputWireColor.B / 255;
                        A = (float)MainForm.colorSettings.InputWireColor.A / 255;
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
                    // Прорисовка окружности непосредственно.
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glColor4f(R, G, B, A);
                    // Устанавливаем центр окружности.
                    Gl.glVertex2d(LocalCenterPoint.X, LocalCenterPoint.Y);
                    // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                    // Если нужна большая точность окружности в ущерб производительности, 
                    // то изменяем шаг "10" на более мелкий, например, "1".
                    for (int angle = 0; angle <= 360; angle += 1)
                    {
                        // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                        double x = MainForm.colorSettings.InputWireRadius /** koef*/ * Math.Cos(angle * Math.PI / 180);
                        double y = MainForm.colorSettings.InputWireRadius /** koef*/ * Math.Sin(angle * Math.PI / 180);
                        // Смещаем окрущность к её центру [xCentre; yCentre].
                        Gl.glVertex2d(x + LocalCenterPoint.X, y + LocalCenterPoint.Y);
                    }
                    Gl.glEnd();
                    //Gl.glPopMatrix();
                }
            }
        }

        public override void SetPoint(int x, int y, int i)
        {
            radius = (int)((Math.Abs(MainCenterPoint.X - x) + Math.Abs(MainCenterPoint.Y - y)));
        }

        public override void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny)
        {
            throw new NotImplementedException();
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
                //Gl.glPopMatrix();
            }
        }

        public override void RecalWithZoom()
        {
            radius = (int)((double)radius / MainForm.zoom);
            MainCenterPoint = MainForm._GenZoomPoint(MainCenterPoint);//new Point((int)(MainCenterPoint.X / MainForm.Zoom), (int)(MainCenterPoint.Y / MainForm.Zoom));
            //LocalCenterPoint = new Point((int)(LocalCenterPoint.X / MainForm.Zoom), (int)(LocalCenterPoint.Y / MainForm.Zoom));
        }

        public override void DrawTemp()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                // Устанавливаем центр окружности.
                //Gl.glVertex2d(CenterPoint.X, CenterPoint.Y);
                // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                // Если нужна большая точность окружности в ущерб производительности, 
                // то изменяем шаг "10" на более мелкий, например, "1".
                for (int angle = 0; angle <= 360; angle += 1)
                {
                    // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                    double x = (double)radius * Math.Cos(angle * Math.PI / 180);
                    double y = (double)radius * Math.Sin(angle * Math.PI / 180);
                    // Смещаем окрущность к её центру [xCentre; yCentre].
                    Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                }
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        internal void DrawTempEnt()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                // Устанавливаем центр окружности.
                //Gl.glVertex2d(CenterPoint.X, CenterPoint.Y);
                // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                // Если нужна большая точность окружности в ущерб производительности, 
                // то изменяем шаг "10" на более мелкий, например, "1".
                for (int angle = 0; angle <= 360; angle += 1)
                {
                    // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                    double x = (double)MainForm.colorSettings.EntranceRadius * MainForm.zoom * Math.Cos(angle * Math.PI / 180);
                    double y = (double)MainForm.colorSettings.EntranceRadius * MainForm.zoom * Math.Sin(angle * Math.PI / 180);
                    // Смещаем окрущность к её центру [xCentre; yCentre].
                    Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                }
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        internal void DrawTempIW()
        {
            if (!delete)
            {
                Gl.glLineWidth(MainForm.colorSettings.LineWidth * (float)MainForm.zoom);
                Gl.glPushMatrix();
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                // Устанавливаем центр окружности.
                //Gl.glVertex2d(CenterPoint.X, CenterPoint.Y);
                // Берём точку c координатой [radius; 0] и начинаем её поворачивать на 360 градусов.
                // Если нужна большая точность окружности в ущерб производительности, 
                // то изменяем шаг "10" на более мелкий, например, "1".
                for (int angle = 0; angle <= 360; angle += 1)
                {
                    // Координаты x, y повёрнутые на заданный угол относительно начала координат.
                    double x = (double)MainForm.colorSettings.InputWireRadius * MainForm.zoom * Math.Cos(angle * Math.PI / 180);
                    double y = (double)MainForm.colorSettings.InputWireRadius * MainForm.zoom * Math.Sin(angle * Math.PI / 180);
                    // Смещаем окрущность к её центру [xCentre; yCentre].
                    Gl.glVertex2d(x + MainCenterPoint.X, y + MainCenterPoint.Y);
                }
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        public override void MoveElem(int x, int y)
        {
            MainCenterPoint = new Point((int)((double)x / MainForm.zoom), (int)((double)y / MainForm.zoom));
        }

        public override object Clone()
        {
            return new Circle
            {
                DL = this.DL,
                MainDL = this.MainDL,
                LocalDL = this.LocalDL,
                MainCenterPoint = this.MainCenterPoint,
                koef = this.koef,
                LocalCenterPoint = this.LocalCenterPoint,
                radius = this.radius,
                side = this.side,
                delete = false
            };
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
                //Gl.glPopMatrix();
            }
        }
    }
}
