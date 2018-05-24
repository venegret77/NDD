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
        public int action = -1;
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
                //toolStripButton3.Enabled = true;
                //toolStripButton4.Enabled = true;
                bool isLable = false;
                toolStripButton4.Enabled = true;
                toolStripButton5.Enabled = false;
                foreach (var neb in MainForm.neButtons)
                {
                    if (neb.id == imageindex)
                    {
                        isLable = true;
                    }
                    else
                    {
                        if (!isLable)
                            isLable = false;
                    }
                    if (isLable)
                    {
                        toolStripButton4.Enabled = false;
                        toolStripButton5.Enabled = true;
                    }
                    else
                    {
                        toolStripButton4.Enabled = true;
                        toolStripButton5.Enabled = false;
                    }
                }
            }
            else
            {
                imageindex = -1;
                toolStripButton2.Enabled = false;
                //toolStripButton3.Enabled = false;
                toolStripButton4.Enabled = false;
                toolStripButton5.Enabled = false;
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
                        if (!File.Exists(Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName))
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
                        }
                        MainForm.ImagesURL.Add(openFileDialog1.SafeFileName);
                        //MainForm.isLoad = false;
                        //GetImages();
                        //Возможно доделать не генерировать каждый раз новые текстуры
                        if (delete)
                            MainForm.DeleteImages.Remove(_item);
                        action = 0;
                        imageindex = MainForm.ImagesURL.Count - 1;
                        Close();
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
            action = 0;
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool used = false;
            foreach (var item in NetworkElements.NetworkElements)
            {
                if (!item.delete)
                    if (item.texture.idimage == imageindex)
                        used = true;
            }
            foreach (var neb in MainForm.neButtons)
            {
                if (neb.id == imageindex)
                    used = true;
            }
            if (used)
                MessageBox.Show("Невозможно удалить данную текстуру, т.к. она используется другим элементом");
            else
            {
                /*MainForm.DeleteImages.Add(MainForm.ImagesURL[imageindex]);
                MainForm.ImagesURL.RemoveAt(imageindex);
                MainForm.isLoad = false;
                MainForm.LoadImages();
                GetImages();
                foreach (var neb in MainForm.neButtons)
                {
                    if (neb.id > imageindex)
                    {
                        int newid = neb.id - 1;
                        neb.id = newid;
                        neb.toolStripButton.Image = MainForm.Images.Images[newid];
                        neb.textname = MainForm.ImagesURL[newid];
                    }
                }
                foreach (var item in NetworkElements.NetworkElements)
                {
                    if (!item.delete)
                        if (item.texture.idimage > imageindex)
                        item.texture.idimage--;
                }*/
                action = 3;
                Close();
            }
        }

        /// <summary>
        /// Добавить ярлык
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            action = 1;
            Close();
            //MainForm.neButtons.Add(new Main.NEButton(new ToolStripButton(MainForm.Images.Images[imageindex]), imageindex, MainForm.ImagesURL[imageindex]));
        }

        /// <summary>
        /// Удалить ярлык
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            action = 2;
            Close();
        }
    }
}
