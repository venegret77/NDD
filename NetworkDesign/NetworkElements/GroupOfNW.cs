using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Группа проводов
    /// </summary>
    public class GroupOfNW : GroupOfElements
    {
        /// <summary>
        /// Список проводов
        /// </summary>
        public List<NetworkWire> NetworkWires = new List<NetworkWire>();
        /// <summary>
        /// Временный провод
        /// </summary>
        public NetworkWire TempNetworkWire = new NetworkWire();
        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupOfNW()
        {
        }

        public override void Add(object elem)
        {
            NetworkWires.Add((NetworkWire)elem);
            NetworkWires.Last().RecalWithZoom();
        }
        /// <summary>
        /// Движение проводов за точками или сетевыми элементами
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="IW">Показывает, вход провода или сетевой элемент</param>
        /// <param name="build">Идентификатор здания</param>
        /// <param name="dl">Уровень отображения</param>
        public void CheckNW(int x, int y, int id, bool IW, int build, DrawLevel dl)
        {
            foreach (var nw in NetworkWires)
            {
                if (!IW)
                {
                    if (nw.idiw1.IW == IW & id == nw.idiw1.ID & nw.idiw1.Build == build & nw.DL == dl)
                    {
                        nw.Points[0] = new Point(x, y);
                    }
                    if (nw.idiw2.IW == IW & id == nw.idiw2.ID & nw.idiw2.Build == build & nw.DL == dl)
                    {
                        nw.Points[nw.Points.Count - 1] = new Point(x, y);
                    }
                }
                else
                {
                    if (nw.idiw1.IW == IW & id == nw.idiw1.ID & nw.idiw1.Build == build)
                    {
                        nw.Points[0] = new Point(x, y);
                    }
                    if (nw.idiw2.IW == IW & id == nw.idiw2.ID & nw.idiw2.Build == build)
                    {
                        nw.Points[nw.Points.Count - 1] = new Point(x, y);
                    }
                }
            }
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < NetworkWires.Count; j++)
            {
                if (j != i)
                {
                    NetworkWires[j].SetActive(false);
                }
                else
                {
                    NetworkWires[j].SetActive(true);
                }
            }
        }

        public override void Draw()
        {
            foreach (var elem in NetworkWires)
            {
                elem.Draw();
            }
        }

        public override void DrawTemp()
        {
            TempNetworkWire.DrawTemp();
        }

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < NetworkWires.Count; i++)
            {
                if (NetworkWires[i].DL == MainForm.drawLevel & !NetworkWires[i].delete)
                {
                    for (int j = 1; j < NetworkWires[i].Points.Count - 1; j++)
                    {
                        _EditRects.Add(new EditRect(NetworkWires[i].Points[j], 9, i, j));
                    }
                }
            }
            return _EditRects;
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in NetworkWires)
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
            NetworkWires[i] = new NetworkWire();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            int _i = -1;
            double _count = Double.MaxValue;
            for (int i = 0; i < NetworkWires.Count; i++)
            {
                if (dl == NetworkWires[i].DL)
                {
                    double count = NetworkWires[i].Search(x, y);
                    if (count < _count & count != -1)
                    {
                        _count = count;
                        _i = i;
                    }
                }
            }
            if (_i != -1)
                return _i;
            else
                return -1;
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override void TempDefault()
        {
            TempNetworkWire = new NetworkWire();
            step = false;
            active = false;
        }
    }
}
