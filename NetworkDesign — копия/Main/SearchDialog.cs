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
    public partial class SearchDialog : Form
    {
        public string text = "";

        public SearchDialog()
        {
            InitializeComponent();
            FormClosed += SearchDialog_FormClosed;
        }

        private void SearchDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            text = textBox1.Text;
        }

        private void SearchDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
