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
        public Point location;
        public Size size;
        public uint idtexture = 0;
        public string text = "";

        public MyText()
        {
            delete = true;
        }

        public MyText(DrawLevel dL, Point location, TextBox textBox)
        {
            DL = dL;
            this.location = MainForm._GenZoomPoint(location);
            this.text = textBox.Text;
            InitText(textBox);
        }

        public void InitText(TextBox textBox)
        {
            // ! Создаем шрифт 
            Font font = new Font(FontFamily.GenericSansSerif, (float)(textBox.Font.Size / (float)MainForm.zoom));
            size = TextRenderer.MeasureText(textBox.Text, font);
            size.Height += 2;
            size.Width += 2;
            Bitmap text_bmp = new Bitmap(size.Width, size.Height);
            // ! Создаем поверхность рисования GDI+ из картинки 
            Graphics gfx = Graphics.FromImage(text_bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            // ! Очищаем поверхность рисования цветом 
            gfx.Clear(Color.FromArgb(0, 255, 255, 255));
            // ! Отрисовываем строку в поверхность рисования (в картинку) 
            gfx.DrawString(textBox.Text, font, Brushes.Black, new PointF(1, 1));
            // ! Вытягиваем данные из картинки 
            //BitmapData data = text_bmp.LockBits(new Rectangle(0, 0, text_bmp.Width, text_bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Il.ilGenImages(1, out int imageId);
            // делаем изображение текущим 
            Il.ilBindImage(imageId);
            string url = Application.StartupPath + @"\###temp.mttex.###";
            if (File.Exists(url))
                File.Delete(url);
            text_bmp.Save(url);
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
                        idtexture = (uint)MainForm.MTTextures.Count - 1;
                        break;
                    case 32:
                        MainForm.MTTextures.Add(MainForm.MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                        idtexture = (uint)MainForm.MTTextures.Count - 1;
                        break;
                }
                // очищаем память 
                Il.ilDeleteImages(1, ref imageId);
            }
            if (File.Exists(url))
                File.Delete(url);
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
                Gl.glPushMatrix();
                Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, MainForm.MTTextures[(int)idtexture]);
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
                Gl.glPopMatrix();
            }
        }

        public object Clone()
        {
            return new MyText
            {
                delete = this.delete,
                DL = this.DL,
                location = new Point(this.location.X, this.location.Y),
                size = new Size(this.size.Width,this.size.Height),
                text = this.text,
                idtexture = this.idtexture
            };
        }

        /*public void DrawTB()
        {
            if (DL == MainForm.drawLevel)
            {
                TextBox.Location = MainForm.GenZoomPoint(ML);
                TextBox.Font = new Font(MF.FontFamily, MF.Size * (float)MainForm.zoom);
                Size size = TextRenderer.MeasureText(TextBox.Text, TextBox.Font);
                TextBox.Width = size.Width + 1;
                TextBox.Height = size.Height;
                TextBox.Visible = true;
            }
            else
            {
                TextBox.Visible = false;
            }
        }*/
    }

    /*public struct MyTB
    {
        public string Text;
        public Point Location;
        public Size size;

        public MyTB(string text, Point location, Size size)
        {
            Text = text;
            Location = location;
            this.size = size;
        }
    }*/
}
