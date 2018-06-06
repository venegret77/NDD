using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.Platform.Windows;

namespace NetworkDesign
{
    public partial class MapControl : Form
    {
        public SizeRenderingArea mapSettings;

        public MapControl(SizeRenderingArea _mapSettings)
        {
            InitializeComponent();
            mapSettings = _mapSettings;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void MapControl_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mapSettings = new SizeRenderingArea(richTextBox1.Text, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
