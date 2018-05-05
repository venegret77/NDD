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
    /// Элементы сети
    /// </summary>
    public abstract class NetworkElement
    {
        /// <summary>
        /// Выбран элемент или нет
        /// </summary>
        public bool active;
        /// <summary>
        /// Текстура
        /// </summary>
        public Texture texture;
        /// <summary>
        /// Наименование
        /// </summary>
        public string name;
        /// <summary>
        /// Заметки
        /// </summary>
        public Notes notes;
        /// <summary>
        /// Уровень отображения
        /// </summary>
        public DrawLevel DL;
        /// <summary>
        /// Общее количество сетевых портов
        /// </summary>
        public int TotalPorts;
        /// <summary>
        /// Занятое количество сетевых портов
        /// </summary>
        public int BusyPorts;
        /// <summary>
        /// Сетевые параметры устройства
        /// </summary>
        public NetworkSettings Options;
        /// <summary>
        /// Удалено или нет
        /// </summary>
        public bool delete;
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
            double vect = x - xmin + x - xmax + y - ymin + y - ymax;
            if (vect <= texture.width)
                return vect;
            else
                return -1;
        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Draw()
        {
            texture.Draw();
        }
    }
}