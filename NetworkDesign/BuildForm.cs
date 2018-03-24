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

        public BuildForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
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
    }
}
