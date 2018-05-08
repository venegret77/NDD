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
        /// Идентификатор изображения
        /// </summary>
        public int idimage = -1;

        public Texture(bool vect, float width, Point location, int idimage)
        {
            this.vect = vect;
            this.width = width;
            this.location = location;
            this.idimage = idimage;
        }

        public Texture()
        {
        }

        /// <summary>
        /// Отрисовка растрового изображения
        /// </summary>
        private void DrawRastrImage()
        {
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
        }

        /// <summary>
        /// Отрисовка векторного изображения
        /// </summary>
        private void DrawVectImage()
        {

        }

        public void Draw()
        {
            if (vect)
                DrawVectImage();
            else
                DrawRastrImage();
        }
    }
}
