using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Уровень отображения
    /// </summary>
    public struct DrawLevel
    {
        /// <summary>
        /// Уровень (Здание)
        /// </summary>
        public int Level;
        /// <summary>
        /// Этаж
        /// </summary>
        public int Floor;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_Level">Уровнь (Здание)</param>
        /// <param name="_Floor">Этаж</param>
        public DrawLevel(int _Level, int _Floor)
        {
            Level = _Level;
            Floor = _Floor;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator == (DrawLevel dl, DrawLevel _dl)
        {
            if (dl.Level == _dl.Level & dl.Floor == _dl.Floor)
                return true;
            else
                return false;
        }

        public static bool operator !=(DrawLevel dl, DrawLevel _dl)
        {
            if (dl.Level == _dl.Level & dl.Floor == _dl.Floor)
                return false;
            else
                return true;
        }
    }
}
