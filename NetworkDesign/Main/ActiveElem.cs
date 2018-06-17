using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Структура для хранения информации о выбранном в данный момент элементе
    /// </summary>
    public struct ActiveElem
    {
        /// <summary>
        /// Тип
        /// </summary>
        public int type;
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int item;
        /// <summary>
        /// Идентификатор здания
        /// </summary>
        public int build;
    }
}
