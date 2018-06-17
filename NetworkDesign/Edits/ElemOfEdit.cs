using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Элемент для изменения
    /// </summary>
    public struct ElemOfEdit
    {
        /// <summary>
        /// Тип
        /// </summary>
        public int type; 
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int id;
        /// <summary>
        /// Точка
        /// </summary>
        public int point;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="point">Точка</param>
        public ElemOfEdit(int type, int id, int point)
        {
            this.type = type;
            this.id = id;
            this.point = point;
        }
    }
}
