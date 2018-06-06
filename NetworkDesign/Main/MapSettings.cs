using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public struct SizeRenderingArea
    {
        public string Name;
        public int Height;
        public int Width;
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public SizeRenderingArea(string name, int height, int width)
        {
            Name = name;
            Height = height;
            Width = width;
            Left = -Width / 2;
            Right = Width / 2;
            Top = Height / 2;
            Bottom = -Height / 2;
        }

        public SizeRenderingArea(string name, int left, int right, int top, int bottom)
        {
            Name = name;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Height = Top - Bottom;
            Width = Right - Left;
        }
    }
}
