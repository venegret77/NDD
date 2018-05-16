using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    public class GroupOfNW : GroupOfElements
    {
        public List<NetworkWire> NetworkWires = new List<NetworkWire>();
        public NetworkWire TempNetworkWire = new NetworkWire();

        public GroupOfNW()
        {
        }

        public override void Add(object elem)
        {
            NetworkWires.Add((NetworkWire)elem);
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
            TempNetworkWire.Draw();
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
            for (int i = 0; i < NetworkWires.Count; i++)
            {
                if (dl == NetworkWires[i].DL)
                {
                    if (NetworkWires[i].Search(x, y) != -1)
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
            TempNetworkWire = new NetworkWire();
            step = false;
        }
    }
}
