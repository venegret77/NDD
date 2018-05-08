using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tao.DevIl;
using Tao.OpenGl;

namespace NetworkDesign.NetworkElements
{
    public partial class ImageTextures : Form
    {
        GroupOfNE NetworkElements = new GroupOfNE();
        public ImageList Images = new ImageList();
        public int imageindex = -1;
        int i = 0;

        public ImageTextures(ref GroupOfNE NetworkElements)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            this.NetworkElements = NetworkElements;
            LoadImages();
        }

        private void LoadImages()
        {
            if (!MainForm.isLoad)
                MainForm.Textures.Clear();
            i = 0;
            Images = new ImageList();
            Images.ImageSize = new Size(200, 200);
            listView1.Clear();
            listView1.LargeImageList = Images;
            listView1.SmallImageList = Images;
            bool noexist = false;
            for (int j = 0; j < MainForm.ImagesURL.Count; j++)
            {
                if (File.Exists(Application.StartupPath + @"\Textures\" + MainForm.ImagesURL[j]))
                {
                    Image image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.ImagesURL[j]);
                    double koef = image.Height / 1000;
                    if (image.Width / 1000 > koef)
                        koef = image.Width / 1000;
                    if (koef > 1)
                    {
                        Bitmap bitmap = new Bitmap(image, (int)(image.Width * koef), (int)(image.Height * koef));
                        bitmap.Save(Application.StartupPath + @"\Textures\" + MainForm.ImagesURL[j]);
                        image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.ImagesURL[j]);
                        Images.Images.Add(image);
                    }
                    else
                    {
                        Images.Images.Add(image);
                    }
                    if (MainForm.isLoad)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = "";
                        item.ImageIndex = i;
                        listView1.Items.Add(item);
                        i++;
                    }
                    else
                    {
                        if (!GenTex(j, Application.StartupPath + @"\Textures\" + MainForm.ImagesURL[j]))
                        {
                            MainForm.ImagesURL.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = "";
                            item.ImageIndex = i;
                            listView1.Items.Add(item);
                            i++;
                        }
                    }
                }
                else
                {
                    noexist = true;
                    MainForm.ImagesURL.RemoveAt(j);
                    j--;
                }
            }
            if (noexist)
            {
                MessageBox.Show("Один или несколько файлов недоступны");
            }
            MainForm.isLoad = true;
        }

        private void ImageTextures_Load(object sender, EventArgs e)
        {

        }

        static public void Save(List<string> imglist)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Create))
            {
                formatter.Serialize(fs, imglist);
            }
        }

        static public List<string> Open()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Textures"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Textures");
                Save(new List<string>());
                return new List<string>();
            }
            if (!File.Exists(Application.StartupPath + @"\Textures\ListOfTextures"))
            {
                Save(new List<string>());
                return new List<string>();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Open))
            {
                return (List<string>)formatter.Deserialize(fs);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                imageindex = listView1.SelectedIndices[0];
                toolStripButton2.Enabled = true;
                toolStripButton3.Enabled = true;
            }
            else
            {
                imageindex = -1;
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bool delete = false;
                    string _item = "";
                    foreach (var item in MainForm.DeleteImages)
                    {
                        if (item == openFileDialog1.SafeFileName)
                        {
                            delete = true;
                            _item = item;
                            break;
                        }
                    }
                    if (File.Exists(Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName) & MainForm.ImagesURL.IndexOf(openFileDialog1.SafeFileName) >= 0 & !delete)
                    {
                        MessageBox.Show("Невозможно загрузить файл, т.к. он уже загружен");
                    }
                    else
                    {
                        Image image = Image.FromFile(openFileDialog1.FileName);
                        double koef = (double)image.Height / 1000;
                        if (image.Width / 1000 > koef)
                            koef = (double)image.Width / 1000;
                        if (koef > 1)
                        {
                            Size newsize = new Size((int)(image.Width / koef), (int)(image.Height / koef));
                            Bitmap bitmap = new Bitmap(image, newsize);
                            bitmap.Save(Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName);
                        }
                        else
                        {
                            File.Copy(openFileDialog1.FileName, Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName, true);
                        }
                        MainForm.ImagesURL.Add(openFileDialog1.SafeFileName);
                        LoadImages();
                        GenTex(MainForm.ImagesURL.Count - 1, Application.StartupPath + @"\Textures\" + MainForm.ImagesURL.Last());
                        if (delete)
                            MainForm.DeleteImages.Remove(_item);
                    }
                }
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Генерация текстуры
        /// </summary>
        /// <param name="id">Идентификатор текстуры в списке ссылок</param>
        /// <param name="url">Ссылка</param>
        private bool GenTex(int id, string url)
        {
            // создаем изображение с идентификатором imageId 
            Il.ilGenImages(1, out int imageId);
            // делаем изображение текущим 
            Il.ilBindImage(imageId);
            // пробуем загрузить изображение 
            if (Il.ilLoadImage(url))
            {
                // если загрузка прошла успешно 
                // сохраняем размеры изображения 
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);

                // определяем число бит на пиксель 
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);

                switch (bitspp) // в зависимости от полученного результата 
                {
                    // создаем текстуру, используя режим GL_RGB или GL_RGBA 
                    case 24:
                        MainForm.Textures.Add(MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                        break;
                    case 32:
                        MainForm.Textures.Add(MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                        break;
                }
                // очищаем память 
                Il.ilDeleteImages(1, ref imageId);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Создание текстуры в памяти OpenGL
        /// </summary>
        /// <param name="Format">Формат изображения</param>
        /// <param name="pixels">Пиксели</param>
        /// <param name="w">Ширина</param>
        /// <param name="h">Высота</param>
        /// <returns></returns>
        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {
            // идентификатор текстурного объекта 
            uint texObject;

            // генерируем текстурный объект 
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей 
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры 
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            //Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);

            // создаем RGB или RGBA текстуру 
            switch (Format)
            {
                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;
            }

            // возвращаем идентификатор текстурного объекта 
            return texObject;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            imageindex = listView1.SelectedIndices[0];
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool used = false;
            foreach (var item in NetworkElements.NetworkElements)
            {
                if (item.texture.idimage == imageindex)
                    used = true;
            }
            if (used)
                MessageBox.Show("Невозможно удалить данную текстуру, т.к. она используется другим элементом");
            else
            {
                MainForm.DeleteImages.Add(MainForm.ImagesURL[imageindex]);
                //Gl.glDeleteTextures(1, ref imageindex);
                //GenNewTextures();
                MainForm.ImagesURL.RemoveAt(imageindex);
                MainForm.isLoad = false;
                LoadImages();
                foreach (var item in NetworkElements.NetworkElements)
                {
                    if (item.texture.idimage > imageindex)
                        item.texture.idimage--;
                }
            }
        }
    }
}
