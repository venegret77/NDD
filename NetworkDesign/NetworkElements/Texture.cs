using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;
using Tao.OpenGl;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Текстура
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Векторная текстура или нет
        /// </summary>
        public bool vect = false;
        /// <summary>
        /// Ширина
        /// </summary>
        public float width = 50;
        /// <summary>
        /// Расположение на карте
        /// </summary>
        public Point location;
        /// <summary>
        /// Список точек
        /// </summary>
        public List<Point> Points = new List<Point>();
        /// <summary>
        /// Идентификатор изображения
        /// </summary>
        public int idimage = -1;
        float R, G, B, A;

        public Texture(bool vect, float width, Point location, int idimage)
        {
            this.vect = vect;
            this.width = width;
            this.location = location;
            this.idimage = idimage;
            CalcPoints();
        }

        internal void CalcPoints()
        {
            Points.Clear();
            Points.Add(new Point(location.X, location.Y));
            Points.Add(new Point(location.X, location.Y + (int)width));
            Points.Add(new Point(location.X + (int)width, location.Y + (int)width));
            Points.Add(new Point(location.X + (int)width, location.Y));
        }

        internal void DrawTemp()
        {
            if (vect)
                DrawVectImageTemp();
            else
                DrawRastrImageTemp();
        }

        private void DrawVectImageTemp()
        {
            throw new NotImplementedException();
        }

        private void DrawRastrImageTemp()
        {
            Gl.glLineWidth(MainForm.colorSettings.LineWidth);
            Gl.glPushMatrix();
            Gl.glColor4f(0.6f, 0.6f, 0.6f, 0.5f);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex2d(location.X - 1, location.Y - 1);
            Gl.glVertex2d(location.X - 1, location.Y + width + 1);
            Gl.glVertex2d(location.X + width + 1, location.Y + width + 1);
            Gl.glVertex2d(location.X + width + 1, location.Y - 1);
            Gl.glEnd();
            Gl.glColor4f(1, 1, 1, 1);
            // включаем режим текстурирования 
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, MainForm.Textures[idimage]);
            // отрисовываем полигон 
            Gl.glBegin(Gl.GL_QUADS);
            // указываем поочередно вершины и текстурные координаты 
            Gl.glVertex2d(location.X, location.Y);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex2d(location.X, location.Y + width);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex2d(location.X + width, location.Y + width);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex2d(location.X + width, location.Y);
            Gl.glTexCoord2f(0, 0);
            // завершаем отрисовку 
            Gl.glEnd();
            // отключаем режим текстурирования 
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();
        }

        public Texture()
        {
        }

        /// <summary>
        /// Отрисовка растрового изображения
        /// </summary>
        private void DrawRastrImage(Int64 Throughput, bool active)
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
                Gl.glLineWidth(2 * (float)MainForm.Zoom);
            }
            else
            {
                R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                Gl.glLineWidth(3 * (float)MainForm.Zoom);
            }
            Gl.glPushMatrix();
            Gl.glScaled(MainForm.Zoom, MainForm.Zoom, MainForm.Zoom);
            Gl.glColor4f(R, G, B, A);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex2d(location.X - 1, location.Y - 1);
            Gl.glVertex2d(location.X - 1, location.Y + width + 1);
            Gl.glVertex2d(location.X + width + 1, location.Y + width + 1);
            Gl.glVertex2d(location.X + width + 1, location.Y - 1);
            Gl.glEnd();
            Gl.glColor4f(1, 1, 1, 1);
            // включаем режим текстурирования 
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, MainForm.Textures[idimage]);
            // отрисовываем полигон 
            Gl.glBegin(Gl.GL_QUADS);
            // указываем поочередно вершины и текстурные координаты 
            Gl.glVertex2d(location.X, location.Y);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex2d(location.X, location.Y + width);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex2d(location.X + width, location.Y + width);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex2d(location.X + width, location.Y);
            Gl.glTexCoord2f(0, 0);
            // завершаем отрисовку 
            Gl.glEnd();
            // отключаем режим текстурирования 
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();
        }

        /// <summary>
        /// Отрисовка векторного изображения
        /// </summary>
        private void DrawVectImage()
        {

        }

        public void Draw(Int64 Throughput, bool active)
        {
            if (vect)
                DrawVectImage();
            else
                DrawRastrImage(Throughput, active);
        }
    }
}
