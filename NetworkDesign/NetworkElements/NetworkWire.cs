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

    public class NetworkWire : Line
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
            Points.Add(new Point(x, y));
            Points.Add(new Point(x, y));
            idiw1 = _idiw1;
            DL = _drawLevel;
            delete = false;
        }

        public NetworkWire()
        {
            delete = true;
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
    }
}
