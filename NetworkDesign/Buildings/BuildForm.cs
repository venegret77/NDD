using NetworkDesign.Buildings;
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
    public partial class BuildForm : Form
    {
        public string name = "DefaultName";
        public bool loft = false; //Чердак
        public bool basement = false; //Подвал
        public int count = 1;
        public DialogResult dialogResult;
        public int width = 800;

        public BuildForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            numericUpDown2.Value = numericUpDown2.Value = 1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            if (checkBox1.Checked)
                basement = true;
            if (checkBox2.Checked)
                loft = true;
            count = (int)numericUpDown1.Value;
            dialogResult = DialogResult.Yes;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.No;
            Close();
        }

        private void BuildForm_Load(object sender, EventArgs e)
        {

        }

        private void BuildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            int max = (int)((float)textBox1.Width / textBox1.Font.Size);
            var lines = TEMP.WrapText(textBox1.Text, max);
            name = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                if (i != lines.Count() - 1)
                    name += lines[i] + Environment.NewLine;
                else
                    name += lines[i];
            }
            if (name == "" | name == " ")
            {
                MessageBox.Show("Пожалуйста введите имя здания");
                e.Cancel = true;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            width = (int)numericUpDown2.Value;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dialogResult = DialogResult.No;
        }
    }
}
