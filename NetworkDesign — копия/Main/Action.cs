using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct Action
    {
        public int action;
        public int type; //Тип элемента; 1 - линия, 2 - прямоугольник, 3 - многоугольник, 4 - здание
        public int item;

        public Action(int _action, int _type, int _item)
        {
            action = _action;
            type = _type;
            item = _item;
        }
    }
}
