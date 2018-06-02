using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class GroupOfRectangles : GroupOfElements
    {
        public List<MyRectangle> Rectangles = new List<MyRectangle>();
        public MyRectangle TempRectangle = new MyRectangle(); //Текущий прямоугольник

        public GroupOfRectangles()
        {

        }

        public override void TempDefault()
        {
            TempRectangle = new MyRectangle();
            step_rect = 0;
        }

        public override void Add(object elem)
        {
            Rectangles.Add((MyRectangle)elem);
            Rectangles.Last().CalcCenterPoint();
            Rectangles.Last().RecalWithZoom();
        }

        public override void Remove(int i)
        {
            Rectangles[i] = new MyRectangle();
        }

        public override void Choose(int i)
        {
            for (int j = 0; j < Rectangles.Count; j++)
            {
                if (j != i)
                {
                    Rectangles[j].SetActive(false);
                }
                else
                {
                    Rectangles[j].SetActive(true);
                }
            }
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            int index = -1;
            dist = -1;
            dist = Int32.MaxValue;
            for (int i = 0; i < Rectangles.Count; i++)
            {
                if (dl == Rectangles[i].DL)
                {
                    double _dist = Rectangles[i].Search(x, y);
                    if (dist == -1 & _dist != -1)
                    {
                        dist = _dist;
                        index = i;
                    }
                    else if (_dist < dist & _dist != -1)
                    {
                        dist = _dist;
                        index = i;
                    }
                }
            }
            return index;
        }

        public override void Draw()
        {
            foreach (var _rect in Rectangles)
            {
                _rect.Draw();
            }
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in Rectangles)
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
            for (int i = 0; i < Rectangles.Count; i++)
            {
                if (Rectangles[i].DL == MainForm.drawLevel & !Rectangles[i].delete)
                {
                    Rectangles[i].CalcTempPoints();
                    _EditRects.Add(new EditRect(Rectangles[i].Points[1], 2, i, 0));
                    _EditRects.Add(new EditRect(Rectangles[i].Point12, 2, i, 12));
                    _EditRects.Add(new EditRect(Rectangles[i].Point13, 2, i, 13));
                    _EditRects.Add(new EditRect(Rectangles[i].Point24, 2, i, 24));
                    _EditRects.Add(new EditRect(Rectangles[i].Point34, 2, i, 34));
                }
            }
            return _EditRects;
        }

        public override void DrawTemp()
        {
            TempRectangle.DrawTemp();
        }
    }
}
