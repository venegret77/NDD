using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tao.DevIl;
using Tao.OpenGl;

namespace NetworkDesign.Main
{
    /// <summary>
    /// Надпись
    /// </summary>
    public class MyText : ICloneable
    {
        /// <summary>
        /// Переменная, показывающая выбран в текущий момент элемент или нет
        /// </summary>
        protected bool active = false;
        /// <summary>
        /// Уровень отображения элемента
        /// </summary>
        public DrawLevel DL;
        /// <summary>
        /// Переменная, показывающая удален элемент или нет
        /// </summary>
        public bool delete = false;
        /// <summary>
        /// Расположение
        /// </summary>
        public Point location;
        /// <summary>
        /// Размер
        /// </summary>
        public Size size;
        /// <summary>
        /// Идентификатор текстуры в списке
        /// </summary>
        public int idtexturefromlist = -1;
        /// <summary>
        /// Идентификатор текстуры
        /// </summary>
        public int idtexture = -1;
        /// <summary>
        /// Текст
        /// </summary>
        public string text = "";
        /// <summary>
        /// Размер шрифта по умолчанию
        /// </summary>
        public float fontsize = 40;
        /// <summary>
        /// Конструктор
        /// </summary>
        public MyText()
        {
            delete = true;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dL">Уровень отображения</param>
        /// <param name="location">Расположение</param>
        /// <param name="textBox">Текст бокс</param>
        public MyText(DrawLevel dL, Point location, TextBox textBox)
        {
            delete = false;
            DL = dL;
            this.location = MainForm._GenZoomPoint(location);
            this.text = textBox.Text;
            InitText(textBox);
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dL">Уровень отображения</param>
        /// <param name="location">Расположение</param>
        /// <param name="size">Размер</param>
        /// <param name="text">Текст</param>
        public MyText(DrawLevel dL, Point location, Size size, string text)
        {
            delete = false;
            DL = dL;
            this.location = location;
            this.text = text;
            this.size = size;
            GenTextureFromBuild();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dL">Уровень отображения</param>
        /// <param name="location">Расположение</param>
        /// <param name="size">Размер</param>
        /// <param name="text">Текст</param>
        /// <param name="fontsize">Размер шрифта</param>
        public MyText(DrawLevel dL, Point location, Size size, string text, float fontsize)
        {
            delete = false;
            DL = dL;
            this.fontsize = fontsize;
            this.location = location;
            this.text = text;
            this.size = size;
            GenTextureFromNE();
        }
        /// <summary>
        /// Генерация текстуры из текстбокса
        /// </summary>
        /// <param name="textBox">Текстбокс</param>
        public void InitText(TextBox textBox)
        {
            if (!delete)
            {
                fontsize = (float)(textBox.Font.Size / (float)MainForm.zoom);
                // ! Создаем шрифт 
                Font font = new Font(FontFamily.GenericSansSerif, (float)(textBox.Font.Size / (float)MainForm.zoom));
                size = TextRenderer.MeasureText(textBox.Text, font);
                size.Height += 2;
                size.Width += 2;
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, 30);
                _size = TextRenderer.MeasureText(textBox.Text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(textBox.Text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        /// <summary>
        /// Генерация текстуры для сетевого элемента
        /// </summary>
        public void GenTextureFromNE()
        {
            if (!delete)
            {
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, fontsize);
                _size = TextRenderer.MeasureText(text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                double koef = 1;
                if (bitmap.Size.Height < size.Height)
                    size.Height = bitmap.Size.Height;
                if (bitmap.Size.Width < size.Width)
                {
                    size.Width = bitmap.Size.Width;
                    size.Height = bitmap.Size.Height;
                }
                location = new Point(location.X - (size.Width / 2), location.Y + (size.Height / 2));
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        public void GenTextureFromNE(bool b)
        {
            if (!delete)
            {
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, fontsize);
                _size = TextRenderer.MeasureText(text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                double koef = 1;
                if (bitmap.Size.Height < size.Height)
                    size.Height = bitmap.Size.Height;
                if (bitmap.Size.Width < size.Width)
                {
                    size.Width = bitmap.Size.Width;
                    size.Height = bitmap.Size.Height;
                }
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        /// <summary>
        /// Генерация текстуры для здания
        /// </summary>
        public void GenTextureFromBuild()
        {
            if (!delete)
            {
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, fontsize);
                _size = TextRenderer.MeasureText(text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                double koef = _size.Width / _size.Height;
                if (bitmap.Size.Height < size.Height)
                {
                    _size.Height = bitmap.Size.Height;
                    _size.Width = (int)((double)bitmap.Size.Height * koef);
                }
                if (bitmap.Size.Width < size.Width)
                {
                    _size.Width = bitmap.Size.Width;
                    _size.Height = (int)((double)bitmap.Size.Width / koef);
                }
                if (_size.Width > size.Width)
                {
                    _size.Width = size.Width;
                    _size.Height = (int)((double)_size.Width / koef);
                }
                if (_size.Height > size.Height)
                {
                    _size.Height = size.Height;
                    _size.Width = (int)((double)_size.Height * koef);
                }
                size = _size;
                location = new Point(location.X - (size.Width / 2), location.Y + (size.Height / 2));
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        /// <summary>
        /// Генерация текстуры для здания
        /// </summary>
        public void GenTextureFromBuild(bool b)
        {
            if (!delete)
            {
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, fontsize);
                _size = TextRenderer.MeasureText(text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                double koef = _size.Width / _size.Height;
                if (bitmap.Size.Height < size.Height)
                {
                    _size.Height = bitmap.Size.Height;
                    _size.Width = (int)((double)bitmap.Size.Height * koef);
                }
                if (bitmap.Size.Width < size.Width)
                {
                    _size.Width = bitmap.Size.Width;
                    _size.Height = (int)((double)bitmap.Size.Width / koef);
                }
                if (_size.Width > size.Width)
                {
                    _size.Width = size.Width;
                    _size.Height = (int)((double)_size.Width / koef);
                }
                if (_size.Height > size.Height)
                {
                    _size.Height = size.Height;
                    _size.Width = (int)((double)_size.Height * koef);
                }
                size = _size;
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        /// <summary>
        /// Обновление текстуры
        /// </summary>
        public void GenNewTexture()
        {
            if (!delete)
            {
                Font font = new Font(FontFamily.GenericSansSerif, fontsize);
                size = TextRenderer.MeasureText(text, font);
                size.Height += 2;
                size.Width += 2;
                Size _size = new Size();
                Font _font = new Font(FontFamily.GenericSansSerif, 30);
                _size = TextRenderer.MeasureText(text, _font);
                _size.Height += 2;
                _size.Width += 2;
                Bitmap text_bmp = new Bitmap(_size.Width, _size.Height);
                // ! Создаем поверхность рисования GDI+ из картинки 
                Graphics gfx = Graphics.FromImage(text_bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // ! Очищаем поверхность рисования цветом 
                gfx.Clear(Color.FromArgb(0, 255, 255, 255));
                // ! Отрисовываем строку в поверхность рисования (в картинку) 
                gfx.DrawString(text, _font, Brushes.Black, new PointF(1, 1));
                if (File.Exists(Application.StartupPath + @"\###temp.mttex.###"))
                    File.Delete(Application.StartupPath + @"\###temp.mttex.###");
                string url = Application.StartupPath + @"\###temp.mttex.###";
                Bitmap bitmap = new Bitmap(text_bmp, RecalcSize(_size));
                bitmap.Save(url);
                bitmap.Dispose();
                // ! Вытягиваем данные из картинки 
                Il.ilGenImages(1, out int imageId);
                // делаем изображение текущим 
                Il.ilBindImage(imageId);
                if (Il.ilLoadImage(url))
                {
                    int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                    // определяем число бит на пиксель 
                    int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                        case 24:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                        case 32:
                            MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            idtexture = (int)MainForm.MTTextures.Last();
                            idtexturefromlist = MainForm.MTTextures.Count - 1;
                            break;
                    }
                    // очищаем память 
                    Il.ilDeleteImages(1, ref imageId);
                }
                if (File.Exists(url))
                    File.Delete(url);
            }
        }
        /// <summary>
        /// Расчет нового размера
        /// </summary>
        /// <param name="size">Размер</param>
        /// <returns>Возвращает новый размер</returns>
        private Size RecalcSize(Size size)
        {
            Size result = new Size();
            int sizewidth = size.Width;
            int sizeheight = size.Height;
            byte pow = 0;
            while (sizewidth > 0)
            {
                sizewidth >>= 1;
                pow++;
            }
            pow--;
            result.Width = (int)Math.Pow(2, pow);
            pow = 0;
            while (sizeheight > 0)
            {
                sizeheight >>= 1;
                pow++;
            }
            pow--;
            result.Height = (int)Math.Pow(2, pow);
            if (result.Width > 1024)
                result.Width = 1024;
            if (result.Height > 1024)
                result.Height = 1024;
            return result;
        }

        /// <summary>
        /// Устанавливает заданную активность
        /// </summary>
        /// <param name="_active"></param>
        public void SetActive(bool _active)
        {
            active = _active;
        }

        /// <summary>
        /// Проверка активности
        /// </summary>
        /// <returns>Возвращает активен элемент или нет</returns>
        public bool CheckActive()
        {
            return active;
        }

        /// <summary>
        /// Устанавливает заданную точку на заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="i">Номер точки в списке</param>
        public void SetPoint(int x, int y, int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Поиск элемента, попавшего в заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Коодината Y</param>
        /// <returns></returns>
        public double Search(int x, int y)
        {
            if (!delete)
            {
                int xmin = (int)((double)location.X * MainForm.zoom);
                int xmax = (int)((double)(location.X + size.Width) * MainForm.zoom);
                int ymax = (int)((double)location.Y * MainForm.zoom);
                int ymin = (int)((double)(location.Y - size.Height) * MainForm.zoom);
                if (x <= xmax & x >= xmin & y <= ymax & y >= ymin)
                    return (double)(xmin + xmax + ymin + ymax) / 4d;
            }
            return -1;
        }

        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Draw()
        {
            if (!delete & DL == MainForm.drawLevel)
            {
                Gl.glColor4f(1, 1, 1, 1);
                //Gl.glPushMatrix();
                //Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, MainForm.MTTextures[(int)idtexturefromlist]);
                // отрисовываем полигон 
                Gl.glBegin(Gl.GL_QUADS);
                // указываем поочередно вершины и текстурные координаты 
                Gl.glVertex2d(location.X, location.Y - size.Height);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex2d(location.X, location.Y);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex2d(location.X + size.Width, location.Y);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2d(location.X + size.Width, location.Y - size.Height);
                Gl.glTexCoord2f(0, 0);
                // завершаем отрисовку 
                Gl.glEnd();
                // отключаем режим текстурирования 
                Gl.glDisable(Gl.GL_TEXTURE_2D);
                //Gl.glPopMatrix();
            }
        }
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        internal void MoveElem(int x, int y)
        {
            location = MainForm._GenZoomPoint(new Point(x - (int)((double) size.Width / 2d * MainForm.zoom), y + (int)((double)size.Height / 2d * MainForm.zoom)));
        }
        /// <summary>
        /// Перемещение элемента
        /// </summary>
        /// <param name="difx">Разница  по Х</param>
        /// <param name="dify">Разница по У</param>
        internal void _MoveElem(int difx, int dify)
        {
            location = new Point(location.X + difx, location.Y + dify);
        }
        /// <summary>
        /// Перемещение элемента без учета зума
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        internal void __MoveElem(int x, int y)
        {
            location = new Point(x, y);
        }
        /// <summary>
        /// Копирвоание элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            return new MyText
            {
                delete = this.delete,
                DL = this.DL,
                location = new Point(this.location.X, this.location.Y),
                size = new Size(this.size.Width,this.size.Height),
                text = this.text,
                idtexturefromlist = this.idtexturefromlist,
                idtexture = this.idtexture,
                fontsize = this.fontsize                
            };
        }
    }
}
