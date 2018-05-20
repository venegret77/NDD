using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Элемент сети
    /// </summary>
    public class NetworkElement
    {
        /// <summary>
        /// Выбран элемент или нет
        /// </summary>
        public bool active = false;
        /// <summary>
        /// Текстура
        /// </summary>
        public Texture texture;
        /// <summary>
        /// Заметки
        /// </summary>
        public Notes notes;
        /// <summary>
        /// Уровень отображения
        /// </summary>
        public DrawLevel DL;
        /// <summary>
        /// Сетевые параметры устройства
        /// </summary>
        public NetworkSettings Options = new NetworkSettings();
        /// <summary>
        /// Удалено или нет
        /// </summary>
        public bool delete = true;
        private double CenterPointX;
        private double CenterPointY;

        public NetworkElement()
        {
            delete = true;
        }

        internal void DrawTemp()
        {
            if (!delete)
                texture.DrawTemp();
        }

        public NetworkElement(Texture texture, DrawLevel dL)
        {
            delete = false;
            this.texture = texture;
            DL = dL;
        }

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
        /// Устанавливает точку на заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void SetPoint(int x, int y)
        {
            texture.location = new Point(x, y);
        }

        /// <summary>
        /// Поиск элемента, попавшего в заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Коодината Y</param>
        /// <returns></returns>
        public double Search(int x, int y)
        {
            double xmin = (double)texture.location.X * MainForm.Zoom;
            double xmax = (double)(texture.location.X + texture.width * MainForm.Zoom) * MainForm.Zoom;
            double ymin = (double)texture.location.Y * MainForm.Zoom;
            double ymax = (double)(texture.location.Y + texture.width * MainForm.Zoom) * MainForm.Zoom;
            if (x <= xmax & x >= xmin & y <= ymax & y >= ymin)
                return 1;
            else
                return -1;
        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Draw()
        {
            if (!delete & DL == MainForm.drawLevel)
                texture.Draw(Options.Throughput, active);
        }

        internal void MoveElem(int x, int y, int id, GroupOfNW networkWires)
        {
            int difx = x - (int)CenterPointX;
            int dify = y - (int)CenterPointY;
            texture.location = new Point(texture.location.X + difx, texture.location.Y + dify);
            networkWires.CheckNW(texture.location.X + difx + (int)((double)texture.width / 2 * MainForm.Zoom), texture.location.Y + dify + (int)((double)texture.width / 2 * MainForm.Zoom), id);
        }

        /// <summary>
        /// Пересчет точек элемента в соответсии с зумом при добавлении временного в основной список
        /// </summary>
        public void RecalWithZoom()
        {
            texture.location = MainForm._GenZoomPoint(texture.location);
            texture.width = (texture.width / (float)MainForm.Zoom);
        }

        /// <summary>
        /// Расчет центральной точки элемента
        /// </summary>
        internal void CalcCenterPoint()
        {
            texture.CalcPoints();
            double x = 0;
            double y = 0;
            int count = 0;
            foreach (var p in texture.Points)
            {
                x += (double)p.X * MainForm.Zoom;
                y += (double)p.Y * MainForm.Zoom;
                count++;
            }
            CenterPointX = x / (double)count;
            CenterPointY = y / (double)count;
        }
    }
}