using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    public class GroupOfMT : GroupOfElements
    {
        public List<MyText> MyTexts = new List<MyText>();
        //public MyText TempMyText = new MyText();

        public GroupOfMT()
        {
        }

        public override void Add(object elem)
        {
            MyTexts.Add((MyText)elem);
        }

        public override void Choose(int i)
        {
            /*for (int j = 0; j < MyTexts.Count; j++)
            {
                if (j != i)
                {
                    MyTexts[j].SetActive(false);
                }
                else
                {
                    MyTexts[j].SetActive(true);
                }
            }*/
        }

        public override void Draw()
        {
            foreach (var elem in MyTexts)
            {
                //elem.Draw();
                elem.DrawTB();
            }
            //TempMyText.Draw();
        }

        public override List<EditRect> GenEditRects()
        {
            throw new NotImplementedException();
        }

        public override List<object> GetInBuild(int build)
        {
            List<object> elems = new List<object>();
            foreach (var elem in MyTexts)
            {
                if (elem.DL.Level == build)
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public override void Remove(int i)
        {
            MyTexts[i] = new MyText();
        }

        public DrawLevel Search(string text)
        {
            foreach (var elem in MyTexts)
                if (elem.TextBox.Text == text)
                    return elem.DL;
            return new DrawLevel(-2, -2);
        }

        public override int Search(int x, int y, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override int Search(int x, int y, out double dist, DrawLevel dl)
        {
            throw new NotImplementedException();
        }

        public override void TempDefault()
        {
            //TempMyText = new MyText();
            step = false;
            active = false;
        }
    }
}
