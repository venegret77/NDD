using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Структура для хранения к чему прикреплен провод
    /// </summary>
    public struct IDandIW
    {
        /// <summary>
        /// Идентификатор элемента
        /// </summary>
        public int ID;
        /// <summary>
        /// Показывает, вход провода или сетевой элемент
        /// </summary>
        public bool IW;
        /// <summary>
        /// Идентификатор здания
        /// </summary>
        public int Build;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="iD">Идентификатор элемента</param>
        /// <param name="iW">Показывает, вход провода или сетевой элемент</param>
        /// <param name="build">Идентификатор здания</param>
        public IDandIW(int iD, bool iW, int build)
        {
            ID = iD;
            IW = iW;
            Build = build;
        }

        public static bool operator ==(IDandIW idiw1, IDandIW idiw2)
        {
            if (idiw1.ID == idiw2.ID & idiw1.IW == idiw2.IW & idiw1.Build == idiw2.Build)
                return true;
            else
                return false;
        }

        public static bool operator !=(IDandIW idiw1, IDandIW idiw2)
        {
            if (idiw1.ID == idiw2.ID & idiw1.IW == idiw2.IW & idiw1.Build == idiw2.Build)
                return false;
            else
                return true;
        }
    }
    /// <summary>
    /// Сетевой провод
    /// </summary>
    public class NetworkWire : GeometricFigure
    {
        /// <summary>
        /// Пропускная способность
        /// </summary>
        public Int64 Throughput = 100000000;
        /// <summary>
        /// Заметки
        /// </summary>
        public Notes notes = new Notes();
        /// <summary>
        /// Закрепленный элемент 1
        /// </summary>
        public IDandIW idiw1 = new IDandIW(-1, false, -1);
        /// <summary>
        /// Закрепленный элемент 2
        /// </summary>
        public IDandIW idiw2 = new IDandIW(-1, false, -1);
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="_drawLevel">Уровень отображения</param>
        /// <param name="_idiw1">Закрепленный элемент 1</param>
        public NetworkWire(int x, int y, DrawLevel _drawLevel, IDandIW _idiw1)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
            Points.Add(TempPoint);
            //ClearTempPoint();
            idiw1 = _idiw1;
            DL = _drawLevel;
            delete = false;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public NetworkWire()
        {
            delete = true;
        }
        /// <summary>
        /// Добавление точки
        /// </summary>
        public void AddPoint()
        {
            Points.Add(TempPoint);
            /*TempPoint = new Point
            {

            }*/
            //ClearTempPoint();
        }
        /// <summary>
        /// Очистка временной точки
        /// </summary>
        public void ClearTempPoint()
        {
            TempPoint = new Point();
        }
        /// <summary>
        /// Движение временной точки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetTempPoint(int x, int y)
        {
            TempPoint.X = x;
            TempPoint.Y = y;
        }
        /// <summary>
        /// Движение точки
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="i">Идентификатор точки</param>
        public override void SetPoint(int x, int y, int i)
        {
            Points[i] = new Point(x, y);
        }

        public override void Draw()
        {
            if (!delete)
            {
                if (DL == MainForm.drawLevel)
                {
                    if (!active)
                    {
                        int min = MainForm.colorSettings.NWmin.ToArgb();
                        int max = MainForm.colorSettings.NWmax.ToArgb();
                        double koef = (double)Throughput / 1000000000000000d;
                        int col = (int)((double)min + ((double)(max - min) * koef));
                        Color color = Color.FromArgb(col);
                        R = (float)color.R / 255;
                        G = (float)color.G / 255;
                        B = (float)color.B / 255;
                        A = (float)color.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.IWWidth * (float)MainForm.zoom);
                    }
                    else
                    {
                        R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                        G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                        B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                        A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                        Gl.glLineWidth(MainForm.colorSettings.IWWidth * (float)MainForm.zoom + 1);
                    }
                    //Gl.glPushMatrix();
                    //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                    Gl.glBegin(Gl.GL_LINE_STRIP);
                    Gl.glColor4f(R, G, B, A);
                    foreach (var point in Points)
                        Gl.glVertex2d(point.X, point.Y);
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
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
                foreach (var point in Points)
                    Gl.glVertex2d(point.X, point.Y);
                Gl.glVertex2d(TempPoint.X, TempPoint.Y);
                Gl.glEnd();
                Gl.glPopMatrix();
            }
        }

        public override double Search(int x, int y)
        {
            if (!delete)
            {
                for (int i = 0; i < Points.Count - 1; i++)
                {
                    int maxx = 0, minx = 0, maxy = 0, miny = 0;
                    if (((double)Points[i].X * MainForm.zoom) > (double)(Points[i + 1].X * MainForm.zoom))
                    {
                        maxx = (int)((double)Points[i].X * MainForm.zoom);
                        minx = (int)((double)Points[i + 1].X * MainForm.zoom);
                    }
                    else
                    {
                        maxx = (int)((double)Points[i + 1].X * MainForm.zoom);
                        minx = (int)((double)Points[i].X * MainForm.zoom);
                    }
                    if (((double)Points[i].Y * MainForm.zoom) > (double)(Points[i + 1].Y * MainForm.zoom))
                    {
                        maxy = (int)((double)Points[i].Y * MainForm.zoom);
                        miny = (int)((double)Points[i + 1].Y * MainForm.zoom);
                    }
                    else
                    {
                        maxy = (int)((double)Points[i + 1].Y * MainForm.zoom);
                        miny = (int)((double)Points[i].Y * MainForm.zoom);
                    }
                    int lw = (int)MainForm.colorSettings.LineWidth + 1;
                    maxx += lw;
                    maxy += lw;
                    minx -= lw;
                    miny -= lw;
                    if (x <= maxx & x >= minx & y <= maxy & y >= miny)
                    {
                        double A = ((double)Points[i + 1].Y * MainForm.zoom) - ((double)Points[i].Y * MainForm.zoom);
                        double B = ((double)Points[i].X * MainForm.zoom) - ((double)Points[i + 1].X * MainForm.zoom);
                        double C = ((double)Points[i].X * MainForm.zoom) * (((double)Points[i].Y * MainForm.zoom) - ((double)Points[i + 1].Y * MainForm.zoom)) + ((double)Points[i].Y * MainForm.zoom) * (((double)Points[i + 1].X * MainForm.zoom) - ((double)Points[i].X * MainForm.zoom));
                        double d = Math.Abs(A * x + B * y + C) / Math.Sqrt((A * A) + (B * B));
                        if (d < lw * MainForm.zoom + 2)
                            return d;
                    }
                }
            }
            return -1;
        }

        public override void DrawB()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Добавление точки
        /// </summary>
        internal void AddNewPoint()
        {
            int x = (Points.Last().X + Points[Points.Count - 2].X) / 2;
            int y = (Points.Last().Y + Points[Points.Count - 2].Y) / 2;
            Points.Insert(Points.Count - 1, new Point(x, y));
        }
        /// <summary>
        /// Удаление точки
        /// </summary>
        internal void RemovePoint()
        {
            if (Points.Count > 2)
                Points.RemoveAt(Points.Count - 2);
        }

        public override object Clone()
        {
            List<Point> points = new List<Point>();
            foreach (var p in Points)
            {
                points.Add(new Point(p.X, p.Y));
            }
            return new NetworkWire
            {
                idiw1 = new IDandIW(this.idiw1.ID, this.idiw1.IW, this.idiw1.Build),
                idiw2 = new IDandIW(this.idiw2.ID, this.idiw2.IW, this.idiw2.Build),
                notes = notes.Copy(),
                Throughput = this.Throughput,
                DL = this.DL,
                Points = points,
                CenterPointX = this.CenterPointX,
                CenterPointY = this.CenterPointY,
                delete = this.delete
            };
        }
    }
}
