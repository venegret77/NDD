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

namespace NetworkDesign
{
    public partial class ColorDialogForm : Form
    {
        public ColorDialogForm()
        {
            InitializeComponent();
            linesbtn.BackColor = MainForm.colorSettings.LinesColor;
            polygonbtn.BackColor = MainForm.colorSettings.PolygonColor;
            rectbtn.BackColor = MainForm.colorSettings.RectColor;
            activeelembtn.BackColor = MainForm.colorSettings.ActiveElemColor;
            buildbtn.BackColor = MainForm.colorSettings.BuildColor;
            circlebtn.BackColor = MainForm.colorSettings.CircleColor;
            entrancebtn.BackColor = MainForm.colorSettings.EntranceColor;
            iwbtn.BackColor = MainForm.colorSettings.InputWireColor;
            button1.BackColor = MainForm.colorSettings.NWmin;
            button2.BackColor = MainForm.colorSettings.NWmax;
            numericUpDown2.Value = MainForm.colorSettings.EntranceRadius;
            numericUpDown3.Value = MainForm.colorSettings.InputWireRadius;
            numericUpDown1.Value = (decimal)MainForm.colorSettings.LineWidth;
            numericUpDown4.Value = (decimal)MainForm.colorSettings.TextureWidth;
            if (MainForm.PingDeviceTimer.Interval < 1000)
                MainForm.PingDeviceTimer.Interval = 60000;
            numericUpDown5.Value = MainForm.PingDeviceTimer.Interval / 1000;
            numericUpDown6.Value = (decimal)MainForm.colorSettings.fontsize;
            if (MainForm.colorSettings.backgroundurl != "")
                pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.colorSettings.backgroundurl);
            checkBox1.Checked = MainForm.colorSettings.isDrawBackground;
            numericUpDown7.Value = (decimal)MainForm.colorSettings.IWWidth;
            StartPosition = FormStartPosition.CenterParent;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            linesbtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.LinesColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            polygonbtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.PolygonColor = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            rectbtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.RectColor = colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            activeelembtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.ActiveElemColor = colorDialog1.Color;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.LineWidth = (float)numericUpDown1.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            buildbtn.BackColor = colorDialog1.Color;
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
            circlebtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.CircleColor = colorDialog1.Color;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            entrancebtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.EntranceColor = colorDialog1.Color;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            iwbtn.BackColor = colorDialog1.Color;
            MainForm.colorSettings.InputWireColor = colorDialog1.Color;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button1.BackColor = colorDialog1.Color;
            MainForm.colorSettings.NWmin = colorDialog1.Color;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button2.BackColor = colorDialog1.Color;
            MainForm.colorSettings.NWmax = colorDialog1.Color;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName))
                {
                    MainForm.colorSettings.backgroundurl = openFileDialog1.SafeFileName;
                    MainForm.GenTex(Application.StartupPath + @"\Textures\" + MainForm.colorSettings.backgroundurl);
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.colorSettings.backgroundurl);
                }
                else
                {
                    File.Copy(openFileDialog1.FileName, Application.StartupPath + @"\Textures\" + openFileDialog1.SafeFileName);
                    MainForm.colorSettings.backgroundurl = openFileDialog1.SafeFileName;
                    MainForm.GenTex(Application.StartupPath + @"\Textures\" + MainForm.colorSettings.backgroundurl);
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\Textures\" + MainForm.colorSettings.backgroundurl);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.isDrawBackground = checkBox1.Checked;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.TextureWidth = (float)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.TimerInterval = (int)(numericUpDown5.Value * 1000);
            MainForm.PingDeviceTimer.Interval = (int)(numericUpDown5.Value * 1000);
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (MainForm.textBox != null)
                MainForm.textBox.Font = new Font(MainForm.textBox.Font.FontFamily, (float)numericUpDown6.Value);
            MainForm.colorSettings.fontsize = (float)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            MainForm.colorSettings.IWWidth = (float)numericUpDown7.Value;
        }
    }
}
