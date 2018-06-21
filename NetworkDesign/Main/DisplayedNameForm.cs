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
    public partial class DisplayedNameForm : Form
    {
        public DialogResult dialogResult = new DialogResult();

        public DisplayedNameForm()
        {
            InitializeComponent();
            dialogResult = DialogResult.Abort;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" & textBox1.Text != " ")
            {
                MainForm.usertemp = textBox1.Text;
                //MainForm.user.SamAccountName = textBox1.Text;
                dialogResult = DialogResult.OK;
                Close();
            }
        }

        private void DisplayedNameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
