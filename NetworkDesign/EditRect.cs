using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class EditRect
    {
        Point Point1;
        Point Point2;
        Point Point3;
        Point Point4;
        public Point coords;
        static public double width = 10;
        public List<ElemOfEdit> elems = new List<ElemOfEdit>();

        public EditRect()
        {

        }

        public EditRect(Point _coords, int _elem, int _point, int _count_elem)
        {
            coords = _coords;
            CalcPoints();
            elems.Add(new ElemOfEdit(_elem, _point, _count_elem));
        }

        public void Refresh(Point _coords)
        {
            coords = _coords;
            CalcPoints();
        }

        private void CalcPoints()
        {
            Point1 = new Point((int)(coords.X - width), (int)(coords.Y + width));
            Point2 = new Point((int)(coords.X + width), (int)(coords.Y + width));
            Point3 = new Point((int)(coords.X - width), (int)(coords.Y - width));
            Point4 = new Point((int)(coords.X + width), (int)(coords.Y - width));
        }

        public void Draw()
        {
            Gl.glLineWidth(1);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glColor4f(1, 0, 0, 1);
            Gl.glVertex2d(Point1.X, Point1.Y);
            Gl.glVertex2d(Point2.X, Point2.Y);
            Gl.glVertex2d(Point4.X, Point4.Y);
            Gl.glVertex2d(Point3.X, Point3.Y);
            Gl.glEnd();
        }
    }
}
