using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
