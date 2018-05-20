using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign.NetworkElements
{
    public struct IDandIW
    {
        public int ID;
        public bool IW;

        public IDandIW(int iD, bool iW)
        {
            ID = iD;
            IW = iW;
        }
    }

    public class NetworkWire : GeometricFigure
    {
        /// <summary>
        /// Пропускная способность
        /// </summary>
        public Int64 Throughput = 100000000;
        /// <summary>
        /// Заметки
        /// </summary>
        public Notes notes;

        public IDandIW idiw1 = new IDandIW(-1, false);
        public IDandIW idiw2 = new IDandIW(-1, false);

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

        public NetworkWire()
        {
            delete = true;
        }

        public void AddPoint()
        {
            Points.Add(TempPoint);
            /*TempPoint = new Point
            {

            }*/
            //ClearTempPoint();
        }

        public void ClearTempPoint()
        {
            TempPoint = new Point();
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
                    Gl.glBegin(Gl.GL_LINE_STRIP);
                    Gl.glColor4f(R, G, B, A);
                    foreach (var point in Points)
                        Gl.glVertex2d(point.X, point.Y);
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
            throw new NotImplementedException();
        }

        public override void DrawB()
        {
            throw new NotImplementedException();
        }
    }
}
