using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct Element
    {
        public int type;
        public int index;
        public object elem;
        public int transform;

        public Element(int _type, int _index, object _elem, int _transform)
        {
            type = _type;
            index = _index;
            elem = _elem;
            transform = _transform;
        }
    }
}
