using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class ElemOfEdit
    {
        public int type; //1 - линия //2 - прямоугольник
        public int point;
        public int count;

        public ElemOfEdit()
        {

        }

        public ElemOfEdit(int _type, int _point, int _count)
        {
            type = _type;
            point = _point;
            count = _count;
        }
    }
}
