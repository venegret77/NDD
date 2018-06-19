using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.Main
{
    public partial class LegendForm : Form
    {
        public LegendForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
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
            listView1.LargeImageList.ImageSize = new Size(50, 50);
        }

        private void LegendForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
