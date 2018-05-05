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

namespace NetworkDesign.NetworkElements
{
    public partial class ImageTextures : Form
    {
        public int imageindex;
        int i = 0;

        public ImageTextures()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            MainForm.images = new ImageList();
            MainForm.images.ImageSize = new Size(200, 200);
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            listView1.LargeImageList = MainForm.images;
            listView1.SmallImageList = MainForm.images;
            bool noexist = false;
            for (int j = 0; j < MainForm.listOfImages.Count; j++)
            {
                if (File.Exists(Application.StartupPath + @"\Textures\" + MainForm.listOfImages[j]))
                {
                    Image image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.listOfImages[j]);
                    MainForm.images.Images.Add(image);
                    ListViewItem item = new ListViewItem();
                    item.Text = "";
                    item.ImageIndex = i;
                    listView1.Items.Add(item);
                    i++;
                }
                else
                {
                    noexist = true;
                    MainForm.listOfImages.RemoveAt(j);
                    j--;
                }
            }
            if (noexist)
            {
                MessageBox.Show("Один или несколько файлов недоступны");
            }
        }

        private void ImageTextures_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog1.FileName);
                File.Copy(openFileDialog1.FileName, Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName);
                MainForm.listOfImages.Add(openFileDialog1.SafeFileName);
                MainForm.images.Images.Add(image);
                ListViewItem item = new ListViewItem();
                item.Text = "";
                item.ImageIndex = i;
                listView1.Items.Add(item);
                i++;
            }
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
                Text = listView1.SelectedIndices[0].ToString();
            }
        }
    }
}
