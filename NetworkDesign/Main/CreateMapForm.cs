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
    public partial class CreateMapForm : Form
    {
        public CreateMapForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "" & richTextBox1.Text != " ")
            {
                Map map = new Map();
                map.SetNewSettings(new SizeRenderingArea(richTextBox1.Text, (int)numericUpDown2.Value, (int)numericUpDown1.Value));
                MainForm.MyMap.MapLoad(map);
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста введите название");
            }
        }
    }
}
