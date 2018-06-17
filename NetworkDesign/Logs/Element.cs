using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Элемент для записи в лог
    /// </summary>
    public struct Element : ICloneable
    {
        /// <summary>
        /// Тип
        /// </summary>
        public int type;
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int index;
        /// <summary>
        /// Сам элемент
        /// </summary>
        public object elem;
        /// <summary>
        /// Преобразование
        /// </summary>
        public int transform;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_type">Тип элемента</param>
        /// <param name="_index">Идентификатор элемента</param>
        /// <param name="_elem">Элемент</param>
        /// <param name="_transform">Трансформация</param>
        public Element(int _type, int _index, object _elem, int _transform)
        {
            type = _type;
            index = _index;
            elem = _elem;
            transform = _transform;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            return new Element
            {
                type = this.type,
                index = this.index,
                elem = this.elem,
                transform = this.transform
            };
        }
    }
}
