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
    public class Texture : ICloneable
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
        /// <summary>
        /// Ссылка на изображение (имя)
        /// </summary>
        public string url = "";
        /// <summary>
        /// Переменные для обозначения цветов
        /// </summary>
        float R, G, B, A;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="location">Расположение</param>
        /// <param name="idimage">Идентификатор изображения</param>
        /// <param name="url">Ссылка на изображение</param>
        public Texture(float width, Point location, int idimage, string url)
        {
            this.url = url;
            this.vect = false;
            this.width = width;
            this.location = location;
            this.idimage = idimage;
            CalcPoints();
        }
        /// <summary>
        /// Расчет точек отрисовки
        /// </summary>
        internal void CalcPoints()
        {
            Points.Clear();
            Points.Add(new Point(location.X, location.Y));
            Points.Add(new Point(location.X, location.Y + (int)width));
            Points.Add(new Point(location.X + (int)width, location.Y + (int)width));
            Points.Add(new Point(location.X + (int)width, location.Y));
        }
        /// <summary>
        /// Отрисовка временного элемента
        /// </summary>
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
        /// <summary>
        /// Отрисовка временного элемента
        /// </summary>
        private void DrawRastrImageTemp()
        {
            Gl.glLineWidth(MainForm.colorSettings.LineWidth);
            //Gl.glPushMatrix();
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
            //Gl.glPopMatrix();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public Texture()
        {
        }

        /// <summary>
        /// Отрисовка растрового изображения
        /// </summary>
        private void DrawRastrImage(Int64 Throughput, bool active, bool isPing)
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
                Gl.glLineWidth(2 * (float)MainForm.zoom);
            }
            else
            {
                R = (float)MainForm.colorSettings.ActiveElemColor.R / 255;
                G = (float)MainForm.colorSettings.ActiveElemColor.G / 255;
                B = (float)MainForm.colorSettings.ActiveElemColor.B / 255;
                A = (float)MainForm.colorSettings.ActiveElemColor.A / 255;
                Gl.glLineWidth(3 * (float)MainForm.zoom);
            }
            //Gl.glPushMatrix();
            //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
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
            //Gl.glPopMatrix();
            Gl.glLineWidth(1);
            if (isPing == null)
                Gl.glColor4f(0.6f, 0.6f, 0.6f, 1);
            else if (isPing)
                Gl.glColor4f(0, 1, 0, 1);
            else
                Gl.glColor4f(1, 0, 0, 1);
            int _x = location.X + (int)width + 1;
            int _y = location.Y - 1;
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex2d(_x, _y);
            for (int angle = 0; angle <= 360; angle += 1)
            {
                double x = width / 10f * Math.Cos(angle * Math.PI / 180);
                double y = width / 10f * Math.Sin(angle * Math.PI / 180);
                Gl.glVertex2d(x + _x, y + _y);
            }
            Gl.glEnd();
        }

        /// <summary>
        /// Отрисовка векторного изображения
        /// </summary>
        private void DrawVectImage()
        {

        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        /// <param name="Throughput">Пропускная способность</param>
        /// <param name="active">Активность</param>
        /// <param name="isPing">Доступность</param>
        public void Draw(Int64 Throughput, bool active, bool isPing)
        {
            if (vect)
                DrawVectImage();
            else
                DrawRastrImage(Throughput, active, isPing);
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            return new Texture
            {
                idimage = this.idimage,
                location = new Point(this.location.X, this.location.Y),
                vect = this.vect,
                width = this.width
            };
        }
    }
}
