using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tao.OpenGl;

namespace NetworkDesign.Main
{
    public class MyText
    {
        /// <summary>
        /// Переменная, показывающая выбран в текущий момент элемент или нет
        /// </summary>
        //protected bool active = false;
        /// <summary>
        /// Уровень отображения элемента
        /// </summary>
        public DrawLevel DL;
        /// <summary>
        /// Переменная, показывающая удален элемент или нет
        /// </summary>
        public bool delete = false;
        //public Point location;
        //public Size size;
        //public uint texture_text = 0;
        [XmlIgnore()]
        public TextBox TextBox;

        [XmlElement("TextBox")]
        public MyTB tb
        {
            get { return new MyTB(TextBox.Text, TextBox.Location, TextBox.Size); }
            set
            {
                TextBox = new TextBox
                 {
                     Text = value.Text,
                     Location = value.Location,
                     Size = value.size,
                     Parent = MainForm.AnT,
                     BackColor = Color.White,
                     BorderStyle = BorderStyle.None
                 };
            }
        }

        public MyText()
        {
            delete = true;
        }

        public MyText(DrawLevel dL, Point location, TextBox textBox)
        {
            DL = dL;
            //this.location = location;
            //InitText(textBox);
            TextBox = new TextBox
            {
                Text = textBox.Text,
                Location = textBox.Location,
                Size = textBox.Size,
                Parent = textBox.Parent,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            this.TextBox.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Size size = TextRenderer.MeasureText(TextBox.Text, TextBox.Font);
            TextBox.Width = size.Width;
            TextBox.Height = size.Height;
            if (e.KeyCode == Keys.Enter)
            {
                Unfocus();
            }
        }

        public void Unfocus()
        {
            MainForm.focusbox.Focus();
        }

        /*public void InitText(TextBox textBox)
        {
            size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            size.Height += 2;
            size.Width += 2;
            Bitmap text_bmp = new Bitmap(size.Width, size.Height);
            // ! Создаем поверхность рисования GDI+ из картинки 
            Graphics gfx = Graphics.FromImage(text_bmp);
            // ! Очищаем поверхность рисования цветом 
            gfx.Clear(Color.FromArgb(255, Color.White));
            // ! Создаем шрифт 
            Font font = new Font(Control.DefaultFont, FontStyle.Regular);
            // ! Отрисовываем строку в поверхность рисования (в картинку) 
            gfx.DrawString(textBox.Text, font, Brushes.Black, new PointF(1, 1));
            // ! Вытягиваем данные из картинки 
            BitmapData data = text_bmp.LockBits(new Rectangle(0, 0, text_bmp.Width, text_bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // ! Генерируем тектурный ID
            Gl.glGenTextures(1, out texture_text);
            // ! Делаем текстуру текущей
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);

            // ! Настраиваем свойства текстура
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            // ! Подгружаем данные из картинки в текстуру
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, text_bmp.Width, text_bmp.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, data.Scan0);

            text_bmp.UnlockBits(data);
        }*/

        /// <summary>
        /// Устанавливает заданную активность
        /// </summary>
        /// <param name="_active"></param>
        /*public void SetActive(bool _active)
        {
            active = _active;
        }*/

        /// <summary>
        /// Проверка активности
        /// </summary>
        /// <returns>Возвращает активен элемент или нет</returns>
        /*public bool CheckActive()
        {
            return active;
        }*/

        /// <summary>
        /// Устанавливает заданную точку на заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="i">Номер точки в списке</param>
        /*public void SetPoint(int x, int y, int i)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Поиск элемента, попавшего в заданные координаты
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Коодината Y</param>
        /// <returns></returns>
        /*public double Search(int x, int y)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Отрисовка
        /// </summary>
        /*public void Draw()
        {
            if (!delete & DL == MainForm.drawLevel)
            {
                Gl.glColor4f(1, 1, 1, 1);
                // включаем режим текстурирования 
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);
                // отрисовываем полигон 
                Gl.glBegin(Gl.GL_QUADS);
                // указываем поочередно вершины и текстурные координаты 
                Gl.glVertex2d(location.X, location.Y - size.Height);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex2d(location.X, location.Y);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2d(location.X + size.Width, location.Y);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex2d(location.X + size.Width, location.Y - size.Height);
                Gl.glTexCoord2f(0, 1);
                // завершаем отрисовку 
                Gl.glEnd();
                // отключаем режим текстурирования 
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }
        }*/

        public void DrawTB()
        {
            if (DL == MainForm.drawLevel)
            {
                TextBox.Visible = true;
            }
            else
            {
                TextBox.Visible = false;
            }
        }
    }

    public struct MyTB
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
    }
}
