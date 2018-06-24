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

namespace NetworkDesign.NetworkElements
{
    public partial class AddTextureForm : Form
    {
        public URL_ID texture = new URL_ID();
        public DialogResult dr = new DialogResult();
        string url = "";
        bool isLoadImage = false;
        public OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public bool delete = false;
        public URL_ID _item;

        public AddTextureForm()
        {
            InitializeComponent();
            dr = DialogResult.Cancel;
            StartPosition = FormStartPosition.CenterScreen;
            foreach (var g in MainForm.groups.GroupsOfNE)
                comboBox1.Items.Add(g.name);
            openFileDialog1.Filter = "Image files (*.png) | *.png";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                delete = false;
                foreach (var item in MainForm.DeleteImages.Textures)
                {
                    if (item.URL == openFileDialog1.SafeFileName)
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
                    isLoadImage = true;
                    url = openFileDialog1.SafeFileName;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isLoadImage & comboBox1.SelectedIndex != -1)
            {
                texture = new URL_ID(url, comboBox1.SelectedIndex, textBox1.Text, richTextBox1.Text);
                dr = DialogResult.OK;
                Close();
            }
        }
    }
}
