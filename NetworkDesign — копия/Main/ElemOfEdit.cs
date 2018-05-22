using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct ElemOfEdit
    {
        public int type; //1 - линия //2 - прямоугольник
        public int id;
        public int point;

        public ElemOfEdit(int type, int id, int point)
        {
            this.type = type;
            this.id = id;
            this.point = point;
        }
    }
}
