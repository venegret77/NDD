using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
}
