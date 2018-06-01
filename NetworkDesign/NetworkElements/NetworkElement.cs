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
    public class NetworkElement : ICloneable
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
        public Notes notes = new Notes();
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
            double xmin = (double)texture.location.X * MainForm.zoom;
            double xmax = (double)(texture.location.X + texture.width) * MainForm.zoom;
            double ymin = (double)texture.location.Y * MainForm.zoom;
            double ymax = (double)(texture.location.Y + texture.width) * MainForm.zoom;
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
            networkWires.CheckNW(texture.location.X + (int)((double)texture.width / 2), texture.location.Y + (int)((double)texture.width / 2), id, false, -1, DL);
        }

        /// <summary>
        /// Пересчет точек элемента в соответсии с зумом при добавлении временного в основной список
        /// </summary>
        public void RecalWithZoom()
        {
            texture.location = MainForm._GenZoomPoint(texture.location);
            texture.width = (texture.width / (float)MainForm.zoom);
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
                x += (double)p.X * MainForm.zoom;
                y += (double)p.Y * MainForm.zoom;
                count++;
            }
            CenterPointX = x / (double)count;
            CenterPointY = y / (double)count;
        }

        internal void SetPoint(int x, int y, int id, GroupOfNW networkWires)
        {
            texture.width = (int)((Math.Abs(texture.location.X - x) + Math.Abs(texture.location.Y - y)));
            networkWires.CheckNW(texture.location.X + (int)((double)texture.width / 2 / MainForm.zoom), texture.location.Y + (int)((double)texture.width / 2 / MainForm.zoom), id, false, -1, DL);
        }

        public object Clone()
        {
            return new NetworkElement
            {
                texture = (Texture)this.texture.Clone(),
                Options = (NetworkSettings)this.Options.Clone(),
                //notes = notes.Copy(),
                DL = this.DL,
                CenterPointX = this.CenterPointX,
                CenterPointY = this.CenterPointY,
                delete = this.delete
            };
        }
    }
}