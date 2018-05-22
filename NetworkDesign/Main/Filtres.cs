using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Main
{
    public struct Filtres
    {
        public bool Line;
        public bool Rect;
        public bool Poly;
        public bool Circ;
        public bool Build;
        public bool Ent;
        public bool IW;
        public bool Text;
        public bool NE;
        public bool NW;

        public Filtres(bool line, bool rect, bool poly, bool circ, bool build, bool ent, bool iW, bool text, bool nE, bool nW)
        {
            Line = line;
            Rect = rect;
            Poly = poly;
            Circ = circ;
            Build = build;
            Ent = ent;
            IW = iW;
            Text = text;
            NE = nE;
            NW = nW;
        }
    }
}
