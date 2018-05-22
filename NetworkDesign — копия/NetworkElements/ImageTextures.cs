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
        
        public int imageindex = -1;
        //int i = 0;

        public ImageTextures(ref GroupOfNE NetworkElements)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            this.NetworkElements = NetworkElements;
            GetImages();
        }

        private void GetImages()
        {
            //MainForm.isLoad = false;
            listView1.Items.Clear();
            ListView images = MainForm.LoadImages();
            listView1.LargeImageList = images.LargeImageList;
            listView1.SmallImageList = images.SmallImageList;
            for (int i = 0; i < images.Items.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = "";
                item.ImageIndex = i;
                listView1.Items.Add(item);
            }
            //listView1.Items.AddRange(images.Items);
        }

        private void ImageTextures_Load(object sender, EventArgs e)
        {

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
                        MainForm.isLoad = false;
                        GetImages();
                        //Возможно доделать не генерировать каждый раз новые текстуры
                        if (delete)
                            MainForm.DeleteImages.Remove(_item);
                    }
                }
            }
            catch
            {

            }
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
                MainForm.ImagesURL.RemoveAt(imageindex);
                MainForm.isLoad = false;
                MainForm.LoadImages();
                foreach (var item in NetworkElements.NetworkElements)
                {
                    if (item.texture.idimage > imageindex)
                        item.texture.idimage--;
                }
            }
        }
    }
}
