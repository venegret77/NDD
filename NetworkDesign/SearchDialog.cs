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
        public SearchDialog()
        {
            InitializeComponent();
        }

        private void SearchDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int a = rnd.Next(0, 2);
            if (a == 1)
            {
                MessageBox.Show("Данное помещение находится в здании " + rnd.Next(1, 10) + " на этаже " + rnd.Next(1, 10));
            }
            else
            {
                MessageBox.Show("Помещений не найдено");
            }
            Close();
        }
    }
}
