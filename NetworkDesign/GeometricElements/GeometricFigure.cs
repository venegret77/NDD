using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Абстрактный класс для геометрических фигур
    /// </summary>
    public abstract class GeometricFigure : ICloneable
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
        /// Х центральной точки
        /// </summary>
        public double CenterPointX = 0;
        /// <summary>
        /// У центральной точки
        /// </summary>
        public double CenterPointY = 0;

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
        /// <returns>Возвращает расстояние до точки</returns>
        public abstract double Search(int x, int y);
        /// <summary>
        /// Расчет минимума и максимума элемента по X и по Y с учетом зума
        /// </summary>
        /// <param name="maxx">Возвращаемый параметр X максимальное</param>
        /// <param name="minx">Возвращаемый параметр X минимальное</param>
        /// <param name="maxy">Возвращаемый параметр Y максимальное</param>
        /// <param name="miny">Возвращаемый параметр Y минимальное</param>
        public virtual void CalcMaxMinWidthZoom(out int maxx, out int minx, out int maxy, out int miny)
        {
            maxx = (int)((double)Points[0].X * MainForm.zoom);
            minx = (int)((double)Points[0].X * MainForm.zoom);
            maxy = (int)((double)Points[0].Y * MainForm.zoom);
            miny = (int)((double)Points[0].Y * MainForm.zoom);
            foreach (var point in Points)
            {
                if ((int)((double)point.X * MainForm.zoom) > maxx)
                    maxx = (int)((double)point.X * MainForm.zoom);
                if ((int)((double)point.X * MainForm.zoom) < minx)
                    minx = (int)((double)point.X * MainForm.zoom);
                if ((int)((double)point.Y * MainForm.zoom) > maxy)
                    maxy = (int)((double)point.Y * MainForm.zoom);
                if ((int)((double)point.Y * MainForm.zoom) < miny)
                    miny = (int)((double)point.Y * MainForm.zoom);
            }
        }
        /// <summary>
        /// Расчет минимума и максимума элемента по X и по Y без учета зума
        /// </summary>
        /// <param name="maxx">Возвращаемый параметр X максимальное</param>
        /// <param name="minx">Возвращаемый параметр X минимальное</param>
        /// <param name="maxy">Возвращаемый параметр Y максимальное</param>
        /// <param name="miny">Возвращаемый параметр Y минимальное</param>
        public virtual void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny)
        {
            maxx = (int)((double)Points[0].X);
            minx = (int)((double)Points[0].X);
            maxy = (int)((double)Points[0].Y);
            miny = (int)((double)Points[0].Y);
            foreach (var point in Points)
            {
                if ((int)((double)point.X) > maxx)
                    maxx = (int)((double)point.X);
                if ((int)((double)point.X) < minx)
                    minx = (int)((double)point.X);
                if ((int)((double)point.Y) > maxy)
                    maxy = (int)((double)point.Y);
                if ((int)((double)point.Y) < miny)
                    miny = (int)((double)point.Y);
            }
        }
        /// <summary>
        /// Расчет центральной точки элемента с учетом зума
        /// </summary>
        public virtual void CalcCenterPoint()
        {
            double x = 0;
            double y = 0;
            int count = 0;
            foreach (var p in Points)
            {
                x += (double)p.X * MainForm.zoom;
                y += (double)p.Y * MainForm.zoom;
                count++;
            }
            CenterPointX = x / (double)count;
            CenterPointY = y / (double)count;
        }
        /// <summary>
        /// Х центральной точки без зума
        /// </summary>
        public double _CenterPointX;
        /// <summary>
        /// У центральной точки без зума
        /// </summary>
        public double _CenterPointY;
        /// <summary>
        /// Расчет центральной точки элемента без учета зума
        /// </summary>
        public virtual void CalcCenterPointWOZ()
        {
            double x = 0;
            double y = 0;
            int count = 0;
            foreach (var p in Points)
            {
                x += (double)p.X;
                y += (double)p.Y;
                count++;
            }
            _CenterPointX = x / (double)count;
            _CenterPointY = y / (double)count;
        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Отрисовка для здания
        /// </summary>
        public abstract void DrawB();
        /// <summary>
        /// Отрисовка временного элемента без учета зума и цветов
        /// </summary>
        public abstract void DrawTemp();
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        public virtual void MoveElem(int x, int y)
        {
            int difx = x - (int)CenterPointX;
            int dify = y - (int)CenterPointY;
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point(Points[i].X + difx, Points[i].Y + dify);
            }
        }
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="difx">Разница по Х</param>
        /// <param name="dify">Разница по У</param>
        public virtual void _MoveElem(int difx, int dify)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point(Points[i].X + difx, Points[i].Y + dify);
            }
        }
        /// <summary>
        /// Пересчет точек элемента в соответсии с зумом при добавлении временного в основной список
        /// </summary>
        public virtual void RecalWithZoom()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point((int)(Points[i].X / MainForm.zoom), (int)(Points[i].Y / MainForm.zoom));
            }
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public abstract object Clone();
    }
}
