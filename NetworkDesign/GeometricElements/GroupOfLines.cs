using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Группа линий
    /// </summary>
    public class GroupOfLines: GroupOfElements
    {
        /// <summary>
        /// Список линий
        /// </summary>
        public List<Line> Lines = new List<Line>();
        /// <summary>
        /// Временная линия
        /// </summary>
        public Line TempLine = new Line(); 
        /// <summary>
        /// Конструктор
        /// </summary>
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
            Lines.Last().CalcCenterPoint();
            Lines.Last().RecalWithZoom();
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
            int _i = -1;
            double _count = Double.MaxValue;
            for (int i = 0; i < Lines.Count; i++)
            {
                if (dl == Lines[i].DL)
                {
                    double count = Lines[i].Search(x, y);
                    if (count < _count & count != -1)
                    {
                        _count = count;
                        _i = i;
                    }
                }
            }
            if (_i != -1)
                return _i;
            else
                return -1;
        }

        public override void Draw()
        {
            foreach (var _Line in Lines)
            {
                _Line.Draw();
            }
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in Lines)
            {
                if (elem.DL.Level == build)
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public override List<EditRect> GenEditRects()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].DL == MainForm.drawLevel & !Lines[i].delete)
                {
                    _EditRects.Add(new EditRect(Lines[i].Points[0], 1, i, 0));
                    _EditRects.Add(new EditRect(Lines[i].Points[1], 1, i, 1));
                }
            }
            return _EditRects;
        }

        public override void DrawTemp()
        {
            TempLine.DrawTemp();
        }
    }
}
