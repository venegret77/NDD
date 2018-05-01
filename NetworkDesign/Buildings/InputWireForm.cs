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
    public partial class InputWireForm : Form
    {
        public DialogResult dialogResult;
        public bool side = false;
        public int floor_index = 0;

        public InputWireForm(List<string> floors_name)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            domainUpDown1.Items.AddRange(floors_name);
        }

        private void InputWireForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                side = false;
                domainUpDown1.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                domainUpDown1.Enabled = true;
                side = true;
                domainUpDown1.SelectedIndex = 0;
            }
            else
                domainUpDown1.Enabled = false;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            floor_index = domainUpDown1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            Close();
        }
    }
}
