using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Параметры отображения
    /// </summary>
    public struct SizeRenderingArea
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name;
        /// <summary>
        /// Высота
        /// </summary>
        public int Height;
        /// <summary>
        /// Ширина
        /// </summary>
        public int Width;
        /// <summary>
        /// Лево
        /// </summary>
        public int Left;
        /// <summary>
        /// Право
        /// </summary>
        public int Right;
        /// <summary>
        /// Верх
        /// </summary>
        public int Top;
        /// <summary>
        /// Низ
        /// </summary>
        public int Bottom;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="height">Высота</param>
        /// <param name="width">Ширина</param>
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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="left">Лево</param>
        /// <param name="right">Право</param>
        /// <param name="top">Верх</param>
        /// <param name="bottom">Низ</param>
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
