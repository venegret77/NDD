using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    /// <summary>
    /// Прямоугольник редактирования
    /// </summary>
    public class EditRect
    {
        /// <summary>
        /// Точка
        /// </summary>
        Point Point1;
        /// <summary>
        /// Точка
        /// </summary>
        Point Point2;
        /// <summary>
        /// Точка
        /// </summary>
        Point Point3;
        /// <summary>
        /// Точка
        /// </summary>
        Point Point4;
        /// <summary>
        /// Кординаты
        /// </summary>
        public Point coords;
        /// <summary>
        /// Размеры
        /// </summary>
        static public double width = 10;
        /// <summary>
        /// Список элементов редактирования
        /// </summary>
        public List<ElemOfEdit> elems = new List<ElemOfEdit>();

        public EditRect()
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_coords">Координаты точки</param>
        /// <param name="_type">Тип элемента</param>
        /// <param name="_point">Точка</param>
        /// <param name="_id">ID</param>
        public EditRect(Point _coords, int _type, int _id, int _point)
        {
            coords = _coords;
            CalcPoints();
            elems.Add(new ElemOfEdit(_type, _id, _point));
        }
        /// <summary>
        /// Обновление
        /// </summary>
        /// <param name="_coords"></param>
        public void Refresh(Point _coords)
        {
            coords = _coords;
            CalcPoints();
        }
        /// <summary>
        /// Расчет точек
        /// </summary>
        private void CalcPoints()
        {
            Point1 = new Point((int)(coords.X - width), (int)(coords.Y + width));
            Point2 = new Point((int)(coords.X + width), (int)(coords.Y + width));
            Point3 = new Point((int)(coords.X - width), (int)(coords.Y - width));
            Point4 = new Point((int)(coords.X + width), (int)(coords.Y - width));
        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Draw()
        {
            Gl.glLineWidth((float)MainForm.zoom);
            //Gl.glPushMatrix();
            //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glColor4f(1, 0, 0, 1);
            Gl.glVertex2d(Point1.X, Point1.Y);
            Gl.glVertex2d(Point2.X, Point2.Y);
            Gl.glVertex2d(Point4.X, Point4.Y);
            Gl.glVertex2d(Point3.X, Point3.Y);
            Gl.glEnd();
            //Gl.glPopMatrix();
        }
    }
}
