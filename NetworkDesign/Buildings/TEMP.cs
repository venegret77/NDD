using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Buildings
{
    /// <summary>
    /// Временный класс
    /// </summary>
    static public class TEMP
    {
        /// <summary>
        /// Автоматический перенос по словам 
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="max">Максимальная ширина</param>
        /// <returns>Возвращает строку с переносами</returns>
        static public string[] WrapText(this string text, int max)
        {
            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                            ? max - (charCount % max) : 0) + w.Length + 1) / max)
                        .Select(g => string.Join(" ", g.ToArray()))
                        .ToArray();
        }
    }
    /// <summary>
    /// Структура для хранения добавленных и удаленных этажей в здании
    /// </summary>
    public struct BUILDLIST
    {
        /// <summary>
        /// Здание
        /// </summary>
        public Building building;
        /// <summary>
        /// Список добавленных этажей
        /// </summary>
        public List<int> Added;
        /// <summary>
        /// Список удаленных этажей
        /// </summary>
        public List<int> Deteled;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="building">Здание</param>
        /// <param name="added">Список добавленных этажей</param>
        /// <param name="deteled">Список удаленных этажей</param>
        public BUILDLIST(Building building, List<int> added, List<int> deteled)
        {
            this.building = building;
            Added = added;
            Deteled = deteled;
        }
    }
}
