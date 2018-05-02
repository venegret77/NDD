using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class Router : NetworkElement
    {
        public override void Draw()
        {
            texture.Draw();
        }

        public override double Search(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void SetPoint(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
