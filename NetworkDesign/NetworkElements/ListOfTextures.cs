using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Список текстур
    /// </summary>
    public class ListOfTextures: ICloneable
    {
        /// <summary>
        /// Количество
        /// </summary>
        public int Count;
        /// <summary>
        /// Список класса URL_ID (текстуры)
        /// </summary>
        public List<URL_ID> Textures = new List<URL_ID>();
        /// <summary>
        /// Конструктор
        /// </summary>
        public ListOfTextures()
        {
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="url">Ссылка</param>
        /// <param name="id">Тип</param>
        public void Add(string url, int id)
        {
            Textures.Add(new URL_ID(url, id));
            Count++;
        }
        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="url_id">Экземпляр класса URL_ID</param>
        public void Add(URL_ID url_id)
        {
            Textures.Add(url_id);
            Count++;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<URL_ID> _Textures = new List<URL_ID>();
            foreach (var t in Textures)
                _Textures.Add(t);
            return new ListOfTextures
            {
                Count = this.Count,
                Textures = _Textures
            };
        }
        /// <summary>
        /// Очистка списка текстур
        /// </summary>
        internal void Clear()
        {
            Textures = new List<URL_ID>();
            Count = 0;
        }
        /// <summary>
        /// Удаление заданной текстуры
        /// </summary>
        /// <param name="id">Идентификатор текстуры</param>
        internal void RemoveAt(int id)
        {
            Textures.RemoveAt(id);
            Count--;
        }
        /// <summary>
        /// Возвращает первое вхождение текстуры с заданной ссылкой
        /// </summary>
        /// <param name="v">Ссылка на текстуру</param>
        /// <returns>Возвращает идентификатор текстуры</returns>
        internal int IndexOf(string v)
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                if (Textures[i].URL == v)
                    return i;
            }
            return -1;
        }
    }
    /// <summary>
    /// Структура для хранения ссылки на текстуру, описания, наименования и типа текструты
    /// </summary>
    public struct URL_ID: ICloneable
    {
        /// <summary>
        /// Ссылка
        /// </summary>
        public string URL;
        /// <summary>
        /// Тип
        /// </summary>
        public int Type;
        /// <summary>
        /// Наименование
        /// </summary>
        public string name;
        /// <summary>
        /// Описание
        /// </summary>
        public string description;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uRL">Ссылка</param>
        /// <param name="iD">Тип</param>
        public URL_ID(string uRL, int iD) : this()
        {
            URL = uRL;
            Type = iD;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uRL">Ссылка</param>
        /// <param name="iD">Тип</param>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        public URL_ID(string uRL, int iD, string name, string description)
        {
            this.URL = uRL;
            this.Type = iD;
            this.name = name;
            this.description = description;
        }

        public object Clone()
        {
            return new URL_ID
            {
                URL = this.URL,
                Type = this.Type,
                name = this.name,
                description = this.description
            };
        }
    }
}
