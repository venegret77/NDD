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
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
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
                int gid = MainForm.ImagesURL.Textures[i].Type;
                ListViewItem item = new ListViewItem(listView1.Groups[gid]);
                item.Text = MainForm.ImagesURL.Textures[i].name;
                item.ToolTipText = MainForm.ImagesURL.Textures[i].description;
                item.ImageIndex = i;
                listView1.Items.Add(item);
            }
            listView1.LargeImageList.ImageSize = new Size(100, 100);
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
            AddTextureForm addTextureForm = new AddTextureForm();
            try
            {
                addTextureForm.ShowDialog();
                if (addTextureForm.dr == DialogResult.OK)
                {
                    if (!File.Exists(Application.StartupPath + @"\Textures\" + addTextureForm.openFileDialog1.SafeFileName))
                    {
                        File.Copy(addTextureForm.openFileDialog1.FileName, Application.StartupPath + @"\Textures\" + addTextureForm.openFileDialog1.SafeFileName, true);
                        Image image = Image.FromFile(Application.StartupPath + @"\Textures\" + addTextureForm.openFileDialog1.SafeFileName);
                        Bitmap bitmap = new Bitmap(image);
                        if (image.Height != 1024 | image.Width != 1024)
                        {
                            bitmap.Dispose();
                            bitmap = new Bitmap(image, 1024, 1024);
                            image.Dispose();
                            bitmap.Save(Application.StartupPath + @"\Textures\" + addTextureForm.openFileDialog1.SafeFileName);
                            bitmap.Dispose();
                        }
                    }
                    MainForm.ImagesURL.Add(addTextureForm.texture);
                    if (addTextureForm.delete)
                        MainForm.DeleteImages.Textures.Remove(addTextureForm._item);
                    action = 0;
                    imageindex = MainForm.ImagesURL.Count - 1;
                    Close();
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
                MessageBox.Show("Невозможно удалить данную текстуру, т.к. она используется другим элементом или создан ярлык");
            else
            {
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

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            ParametresForm parametresForm = new ParametresForm(ref NetworkElements);
            parametresForm.ShowDialog();
        }
    }
}
