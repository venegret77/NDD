using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign
{
    public partial class ColorDialogForm : Form
    {
        public ColorDialogForm()
        {
            InitializeComponent();
            button1.BackColor = MainForm.colorSettings.LinesColor;
            button2.BackColor = MainForm.colorSettings.PolygonColor;
            button3.BackColor = MainForm.colorSettings.RectColor;
            button4.BackColor = MainForm.colorSettings.ActiveElemColor;
            button6.BackColor = MainForm.colorSettings.BuildColor;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button1.BackColor = colorDialog1.Color;
            MainForm.colorSettings.LinesColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button2.BackColor = colorDialog1.Color;
            MainForm.colorSettings.PolygonColor = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button3.BackColor = colorDialog1.Color;
            MainForm.colorSettings.RectColor = colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button4.BackColor = colorDialog1.Color;
            MainForm.colorSettings.ActiveElemColor = colorDialog1.Color;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.LineWidth = (float)numericUpDown1.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button6.BackColor = colorDialog1.Color;
            MainForm.colorSettings.BuildColor = colorDialog1.Color;
        }

        private void ColorDialogForm_Load(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.EntranceRadius = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.InputWireRadius = (int)numericUpDown3.Value;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button9.BackColor = colorDialog1.Color;
            MainForm.colorSettings.CircleColor = colorDialog1.Color;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button7.BackColor = colorDialog1.Color;
            MainForm.colorSettings.EntranceColor = colorDialog1.Color;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button8.BackColor = colorDialog1.Color;
            MainForm.colorSettings.InputWireColor = colorDialog1.Color;
        }
    }
}
