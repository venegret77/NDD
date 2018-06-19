using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Группа сетевых элементов
    /// </summary>
    public class GroupOfNE : GroupOfElements
    {
        /// <summary>
        /// Список сетевых элементов
        /// </summary>
        public List<NetworkElement> NetworkElements = new List<NetworkElement>();
        /// <summary>
        /// Временный сетевой элемент
        /// </summary>
        public NetworkElement TempNetworkElement = new NetworkElement();
        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupOfNE()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ne">Группа сетевых элементов</param>
        public GroupOfNE(GroupOfNE ne)
        {
            NetworkElements = ne.NetworkElements;
        }
        /// <summary>
        /// Обновление параметров при удалении
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void UpdateOptions(int id)
        {
            for (int i = 0; i < NetworkElements.Count; i++)
            {
                for (int j = 0; j < NetworkElements[i].Options.Options.Count; j++)
                {
                    if (NetworkElements[i].Options.Options[j].ID > id)
                        NetworkElements[i].Options.Options[j].ID--;
                }
            }
        }
        /// <summary>
        /// Обновление параметров и их идентификаторов
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Параметр</param>
        public void UpdateOptions(int id, string name)
        {
            for (int i = 0; i < NetworkElements.Count; i++)
            {
                NetworkElements[i].Options.Options[id].Name = MainForm.parametrs.Params[id];
            }
        }

        public override void Add(object elem)
        {
            NetworkElements.Add((NetworkElement)elem);
            NetworkElements.Last().RecalWithZoom();
            NetworkElements.Last().CalcCenterPoint();
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < NetworkElements.Count; j++)
            {
                if (j != i)
                {
                    NetworkElements[j].SetActive(false);
                }
                else
                {
                    NetworkElements[j].SetActive(true);
                }
            }
        }

        public override void Draw()
        {
            foreach (var elem in NetworkElements)
            {
                elem.Draw();
            }
        }

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < NetworkElements.Count; i++)
            {
                if (NetworkElements[i].DL == MainForm.drawLevel & !NetworkElements[i].delete)
                    _EditRects.Add(new EditRect(new Point(NetworkElements[i].texture.location.X + (int)NetworkElements[i].texture.width, NetworkElements[i].texture.location.Y), 8, i, 0));
            }
            return _EditRects;
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in NetworkElements)
            {
                if (elem.DL.Level == build)
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public override void Remove(int i)
        {
            NetworkElements[i] = new NetworkElement();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            for (int i = 0; i < NetworkElements.Count; i++)
            {
                if (dl == NetworkElements[i].DL)
                {
                    if (NetworkElements[i].Search(x, y) != -1)
                        return i;
                }
            }
            return -1;
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override void TempDefault()
        {
            TempNetworkElement = new NetworkElement();
            step = false;
            active = false;
        }

        public override void DrawTemp()
        {
            TempNetworkElement.DrawTemp();
        }
    }
}
