using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class GroupOfEditRects: GroupOfElements
    {
        public List<EditRect> EditRects = new List<EditRect>();
        public bool edit_mode = true;
        public int editRect = -1;
        public bool edit_active = false;

        public GroupOfEditRects()
        {

        }

        public override void TempDefault()
        {
            edit_active = false;
            edit_mode = false;
            editRect = -1;
        }

        public override void Add(object elem)
        {
            EditRects.Add((EditRect)elem);
        }

        public override void Draw()
        {
            foreach (var _editRect in EditRects)
            {
                _editRect.Draw();
            }
        }

        public int Search(int x, int y)
        {
            double xydif = EditRect.width * 2;
            int num = -1;
            for (int i = 0; i < EditRects.Count; i ++)
            {
                int difX = Math.Abs(x - EditRects[i].coords.X);
                int difY = Math.Abs(y - EditRects[i].coords.Y);
                if (difX <= EditRect.width & difY <= EditRect.width)
                {
                    if (difX + difY < xydif)
                    {
                        xydif = difX + difY;
                        num = i;
                    }
                }
            }
            return num;
        }

        public void RefreshEditRect(GroupOfLines lines, GroupOfRectangles rectangles, GroupOfPolygons bl)
        {
            List<EditRect> _EditRects = new List<EditRect>();
            bool b = false;
            for (int i = 0; i < lines.Lines.Count; i++)
            {
                if (lines.Lines[i].DL == MainForm.drawLevel)
                {
                    _EditRects.Add(new EditRect(lines.Lines[i].Points[1], 1, 1, i));
                    _EditRects.Add(new EditRect(lines.Lines[i].Points[2], 1, 2, i));
                }
            }
            /*for (int i = 0; i < rectangles.Rectangles.Count; i++)
            {
                if (rectangles.Rectangles[i].DL == MainForm.drawLevel)
                {
                    _EditRects.Add(new EditRect(rectangles.Rectangles[i].Point12, 2, 12, i));
                    _EditRects.Add(new EditRect(rectangles.Rectangles[i].Point13, 2, 13, i));
                    _EditRects.Add(new EditRect(rectangles.Rectangles[i].Point24, 2, 24, i));
                    _EditRects.Add(new EditRect(rectangles.Rectangles[i].Point34, 2, 34, i));
                }
            }*/
            for (int i = 0; i < bl.Polygons.Count; i++)
            {
                if (bl.Polygons[i].DL == MainForm.drawLevel)
                {
                    for (int j = 0; j < bl.Polygons[i].Points.Count; j++)
                    {
                        _EditRects.Add(new EditRect(bl.Polygons[i].Points[j], 3, j, i));
                    }
                }
            }
            for (int i = 0; i < _EditRects.Count; i++)
            {
                foreach (var elem in EditRects)
                {
                    if (elem.coords == _EditRects[i].coords)
                    {
                        elem.elems.Add(_EditRects[i].elems[0]);
                        b = true;
                        break;
                    }
                }
                if (!b)
                {
                    EditRects.Add(_EditRects[i]);
                }
                b = false;
            }
        }

        public override void Remove(int i)
        {
            throw new NotImplementedException();
        }

        public override void Choose(int i)
        {
            throw new NotImplementedException();
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }
    }
}
