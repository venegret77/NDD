using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfRectangles
    {
        public List<Rectangle> Rectangles = new List<Rectangle>();
        public Rectangle TempRectangle = new Rectangle(); //Текущий прямоугольник
        public int step_rect = 0; //0 - нет точек, 1 - одна точка, 2 - две точки

        public GroupOfRectangles()
        {

        }

        public void TempDefault()
        {
            TempRectangle = new Rectangle();
            step_rect = 0;
        }

        public void Add(Rectangle rect)
        {
            Rectangles.Add(rect);
        }

        public void Remove(int i)
        {
            Rectangles[i] = new Rectangle();
        }

        public void Choose(int i)
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

        public int CalcNearestRect(int x, int y, out double dist, DrawLevel dl)
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

        public void Draw()
        {
            foreach (var _rect in Rectangles)
            {
                _rect.Draw();
            }
            TempRectangle.Draw();
        }
    }
}
