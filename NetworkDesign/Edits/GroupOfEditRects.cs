using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Группа элементов редактирования
    /// </summary>
    public class GroupOfEditRects: GroupOfElements
    {
        /// <summary>
        /// Список прямоугольников редактирования
        /// </summary>
        public List<EditRect> EditRects = new List<EditRect>();
        /// <summary>
        /// Включен ли режим редактирования
        /// </summary>
        public bool edit_mode = true;
        /// <summary>
        /// Текущий элемент редактирования
        /// </summary>
        public int editRect = -1;
        /// <summary>
        /// Перемещяется ли элемент
        /// </summary>
        public bool edit_active = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupOfEditRects()
        {

        }

        public override void TempDefault()
        {
            edit_active = false;
            edit_mode = false;
            editRect = -1;
        }

        public override void Add(object elem)
        {
            EditRects.Add((EditRect)elem);
        }

        public override void Draw()
        {
            foreach (var _editRect in EditRects)
            {
                _editRect.Draw();
            }
        }

        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <returns></returns>
        public int Search(int x, int y)
        {
            x = (int)((double)x / MainForm.zoom);
            y = (int)((double)y / MainForm.zoom);
            double xydif = EditRect.width * 2;
            int num = -1;
            for (int i = 0; i < EditRects.Count; i ++)
            {
                int difX = Math.Abs(x - EditRects[i].coords.X);
                int difY = Math.Abs(y - EditRects[i].coords.Y);
                if (difX <= EditRect.width & difY <= EditRect.width)
                {
                    if (difX + difY < xydif)
                    {
                        xydif = difX + difY;
                        num = i;
                    }
                }
            }
            return num;
        }

        public override void Remove(int i)
        {
            throw new NotImplementedException();
        }

        public override void Choose(int i)
        {
            throw new NotImplementedException();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            throw new NotImplementedException();
        }

        public override List<EditRect> GenEditRects()
        {
            throw new NotImplementedException();
        }

        public override void DrawTemp()
        {
            throw new NotImplementedException();
        }
    }
}
