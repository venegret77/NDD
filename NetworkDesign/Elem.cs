using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public abstract class Elem
    {
        protected bool active = false;
        public DrawLevel DL;
        protected float R = 0, G = 0, B = 0, A = 1;
        public List<Point> Points = new List<Point>();
        protected Point TempPoint = new Point();
        public bool delete = false;
        protected double alfa = 0;

        public void SetActive(bool b)
        {
            active = b;
        }

        public bool CheckActive()
        {
            return active;
        }

        public abstract void SetPoint(int x, int y, int i);
        public abstract double Search(int x, int y);
        public abstract void CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
        public abstract void Draw();
        public abstract void DrawB();
    }
}
