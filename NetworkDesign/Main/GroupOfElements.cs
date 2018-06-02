using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public abstract class GroupOfElements
    {
        /// <summary>
        /// Переменная, показывающая какой сейчас шаг для элемента (кроме прямоугольников)
        /// </summary>
        public bool step = false;

        /// <summary>
        /// Переменная, показывающая активен или нет
        /// </summary>
        public bool active = false;

        /// <summary>
        /// Переменная, показывающая какой сейчас шаг для прямоугольника
        /// 0 - нет точек, 1 - одна точка, 2 - две точки
        /// </summary>
        public int step_rect = 0; 

        /// <summary>
        /// Возвращает переменные к состояниям по умолчанию
        /// </summary>
        public abstract void TempDefault();

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="elem">Элемент типа object</param>
        public abstract void Add(object elem);

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="i">Индекс элемента</param>
        public abstract void Remove(int i);

        /// <summary>
        /// Сделать активным заданный элемент
        /// </summary>
        /// <param name="i">Индекс элемента</param>
        public abstract void Choose(int i);
        
        /// <summary>
        /// Поиск ближайшего элемента к заданным координатам, находящимся на этом же уровне отображения
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns>Возвращает индекс элемента</returns>
        public abstract int Search(int x, int y, DrawLevel dl);

        /// <summary>
        /// Поиск ближайшего элемента к заданным координатам, находящимся на этом же уровне отображения
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="dist">Возвращаемый параметр расстояние до элемента</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns>Возвращает индекс элемента</returns>
        public abstract int Search(int x, int y,out double dist, DrawLevel dl);

        public void AddGroupElems(List<object> elems)
        {
            foreach (var elem in elems)
            {
                Add(elem);
            }
        }

        //public abstract List<object> ConvertToListObj(); 

        /// <summary>
        /// Получение списка объектов внутри здания. Используется при экспорте зданий
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public abstract List<object> GetInBuild(int build);

        public abstract List<EditRect> GenEditRects();

        /// <summary>
        /// Отрисовка элементов
        /// </summary>
        public abstract void Draw();
        public abstract void DrawTemp();
    }
}
