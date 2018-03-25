using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public abstract class GeometricFigure
    {
        /// <summary>
        /// Переменная, показывающая выбран в текущий момент элемент или нет
        /// </summary>
        protected bool active = false;
        /// <summary>
        /// Уровень отображения элемента
        /// </summary>
        public DrawLevel DL;
        /// <summary>
        /// Переменные, отвечающие за цвет
        /// </summary>
        protected float R = 0, G = 0, B = 0, A = 1;
        /// <summary>
        /// Список точек элемента
        /// </summary>
        public List<Point> Points = new List<Point>();
        /// <summary>
        /// Временная точка (для многоугольника)
        /// </summary>
        protected Point TempPoint = new Point();
        /// <summary>
        /// Переменная, показывающая удален элемент или нет
        /// </summary>
        public bool delete = false;
        /// <summary>
        /// Угол поворота (для прямоугольников)
        /// </summary>
        protected double alfa = 0;

        /// <summary>
        /// Устанавливает заданную активность
        /// </summary>
        /// <param name="_active"></param>
        public void SetActive(bool _active)
        {
            active = _active;
        }

        /// <summary>
        /// Проверка активности
        /// </summary>
        /// <returns>Возвращает активен элемент или нет</returns>
        public bool CheckActive()
        {
            return active;
        }

        /// <summary>
        /// Устанавливает заданную точку на заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="i">Номер точки в списке</param>
        public abstract void SetPoint(int x, int y, int i);
        /// <summary>
        /// Поиск элемента, попавшего в заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Коодината Y</param>
        /// <returns></returns>
        public abstract double Search(int x, int y);
        /// <summary>
        /// Расчет минимума и максимума элемента по X и по Y
        /// </summary>
        /// <param name="maxx">Возвращаемый параметр X максимальное</param>
        /// <param name="minx">Возвращаемый параметр X минимальное</param>
        /// <param name="maxy">Возвращаемый параметр Y максимальное</param>
        /// <param name="miny">Возвращаемый параметр Y минимальное</param>
        public abstract void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
        /// <summary>
        /// Отрисовка
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Отрисовка для здания
        /// </summary>
        public abstract void DrawB();
    }
}
