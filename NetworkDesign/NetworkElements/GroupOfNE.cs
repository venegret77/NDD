using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    public class GroupOfNE : GroupOfElements
    {
        public List<NetworkElement> NetworkElements = new List<NetworkElement>();
        public NetworkElement TempNetworkElement = new NetworkElement();

        public GroupOfNE()
        {
        }

        public override void Add(object elem)
        {
            NetworkElements.Add((NetworkElement)elem);
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
            TempNetworkElement.Draw();
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
        }
    }
}
