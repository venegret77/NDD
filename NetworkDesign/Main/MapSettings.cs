using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct MapSettings
    {
        public string Name; //Имя
        public int Height; //Высота
        public int Width; //Ширина

        public MapSettings(string _Name, int _Height, int _Width)
        {
            Name = _Name;
            Height = _Height;
            Width = _Width;
        }
    }
}
