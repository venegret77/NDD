using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfLines: GroupOfElements
    {
        public List<Line> Lines = new List<Line>();
        public Line TempLine = new Line(); //Текущая линия

        public GroupOfLines()
        {

        }

        public override void TempDefault()
        {
            TempLine = new Line();
            step = false;
        }

        public override void Add(object elem)
        {
            Lines.Add((Line)elem);
        }

        public override void Remove(int i)
        {
            Lines[i] = new Line();
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < Lines.Count; j++)
            {
                if (j != i)
                {
                    Lines[j].SetActive(false);
                }
                else
                {
                    Lines[j].SetActive(true);
                }
            }
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (dl == Lines[i].DL)
                {
                    if (Lines[i].Search(x, y) == 1)
                        return i;
                }
            }
            return -1;
        }

        public override void Draw()
        {
            foreach (var _Line in Lines)
            {
                _Line.Draw();
            }
            TempLine.Draw();
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }
    }
}
