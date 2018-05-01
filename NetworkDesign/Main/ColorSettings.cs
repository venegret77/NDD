using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct ColorSettings
    {
        public Color LinesColor;
        public Color PolygonColor;
        public Color RectColor;
        public Color BuildColor;
        public Color ActiveElemColor;
        public Color CircleColor;
        public Color EntranceColor;
        public Color InputWireColor;
        public float LineWidth;
        public int EntranceRadius;
        public int InputWireRadius;

        public ColorSettings(string todefault)
        {
            LinesColor = Color.Black;
            PolygonColor = Color.Black;
            RectColor = Color.Black;
            ActiveElemColor = Color.Black;
            BuildColor = Color.Black;
            CircleColor = Color.Black;
            EntranceColor = Color.Black;
            InputWireColor = Color.Black;
            EntranceRadius = 5;
            InputWireRadius = 5;
            LineWidth = 1;
        }
    }
}
