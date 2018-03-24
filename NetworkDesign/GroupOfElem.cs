using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public abstract class GroupOfElem
    {
        public abstract void TempDefault();

        public abstract void Add(object elem);

        public abstract void Remove(int i);

        public abstract void Choose(int i);

        public abstract int Search(int x, int y, DrawLevel dl);

        public abstract void Draw();
    }
}
