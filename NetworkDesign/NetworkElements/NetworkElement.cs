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

        public NetworkElement()
        {
            delete = true;
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
            double xmin = texture.location.X;
            double xmax = texture.location.X + texture.width;//+ или - решить позже
            double ymin = texture.location.Y;
            double ymax = texture.location.Y + texture.width;
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
                texture.Draw();
        }
    }
}