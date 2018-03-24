using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public abstract class GroupOfGeometricFigure
    {
        public bool step = false;
        public bool active = false;
        public int step_rect = 0; //0 - нет точек, 1 - одна точка, 2 - две точки

        public abstract void TempDefault();

        public abstract void Add(object elem);

        public abstract void Remove(int i);

        public abstract void Choose(int i);

        public abstract int Search(int x, int y, DrawLevel dl);
        public abstract int Search(int x, int y,out double dist, DrawLevel dl);

        public abstract void Draw();
    }
}
