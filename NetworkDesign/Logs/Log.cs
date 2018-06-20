using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Лог действий
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Список сообщений лога для стрелочки назад
        /// </summary>
        public List<LogMessage> Back = new List<LogMessage>();
        /// <summary>
        /// Список сообщений лога для стрелочки вперед
        /// </summary>
        public List<LogMessage> Forward = new List<LogMessage>();
        /// <summary>
        /// Конструктор
        /// </summary>
        public Log()
        {

        }
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="logMessage">Сообщение лога</param>
        public void Add(LogMessage logMessage)
        {
            if (logMessage._elem.type == 13 & logMessage._elem.transform == -4)
            {
                if (Back.Count != 0)
                {
                    LogMessage lastelem = Back.Last();
                    if (lastelem._elem.transform == -4 & lastelem._elem.type == logMessage._elem.type & lastelem._elem.index == logMessage._elem.index)
                    {
                        logMessage._elem = (Element)lastelem._elem.Clone();
                        Back.RemoveAt(Back.Count - 1);
                    }
                }
            }
            else if (logMessage._elem.transform == -2 & logMessage._elem.type != 4 & logMessage._elem.type != 13 & logMessage._elem.type != 14 & logMessage._elem.type != 15)
            {
                if (Back.Count != 0)
                {
                    LogMessage lastelem = Back.Last();
                    if (lastelem._elem.transform == -2 & lastelem._elem.type == logMessage._elem.type & lastelem._elem.index == logMessage._elem.index)
                    {
                        logMessage._elem = (Element)lastelem._elem.Clone();
                        Back.RemoveAt(Back.Count - 1);
                    }
                }
            }
            Back.Add(logMessage);
        }
        /// <summary>
        /// Удаление последнего элемента из списка назад
        /// </summary>
        /// <param name="el">Возвращает элемент 1</param>
        /// <param name="buildid">ВОзвращает идентификатор здания</param>
        /// <returns>Возвращает элемент 2</returns>
        public Element DeleteLastBack(out Element el, out int buildid)
        {
            LogMessage temp = Back[Back.Count - 1];
            Back.RemoveAt(Back.Count - 1);
            Forward.Add(temp);
            el = temp._elem;
            buildid = temp.buildid;
            return temp.elem;
        }
        /// <summary>
        /// Удаление последнего элемента из списка вперед
        /// </summary>
        /// <param name="el">Возвращает элемент 1</param>
        /// /// <param name="buildid">ВОзвращает идентификатор здания</param>
        /// <returns>Возвращает элемент 2</returns>
        public Element DeleteLastForward(out Element el, out int buildid)
        {
            LogMessage temp = Forward[Forward.Count - 1];
            Forward.RemoveAt(Forward.Count - 1);
            Back.Add(temp);
            el = temp.elem;
            buildid = temp.buildid;
            return temp._elem;
        }
        /// <summary>
        /// Очиска списка вперед
        /// </summary>
        public void ClearForward()
        {
            Forward.Clear();
        }
        /// <summary>
        /// Проверка на наличие элементов в логе
        /// </summary>
        /// <returns>Возвращает наличие элементов в логе</returns>
        public bool NotNullBack()
        {
            if (Back.Count != 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Проверка на наличие элементов в логе
        /// </summary>
        /// <returns>Возвращает наличие элементов в логе</returns>
        public bool NotNullForward()
        {
            if (Forward.Count != 0)
                return true;
            else
                return false;
        }

    }
}
