using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;

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

        public Texture(bool vect, float width, Point location)
        {
            this.vect = vect;
            this.width = width;
            this.location = location;
        }

        /// <summary>
        /// Отрисовка растрового изображения
        /// </summary>
        private void DrawRastrImage()
        {
            //
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
