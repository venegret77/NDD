using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace NetworkDesign
{
    public partial class AutorisationForm : Form
    {
        DirectoryEntry CurrentDomain;

        public AutorisationForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!label1.Visible)
            {
                label1.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = Environment.UserName;
            }
            else
            {
                if (textBox1.Text != "")
                {
                    MainForm mf = new MainForm();
                    MainForm.user = textBox1.Text;
                    mf.Show();
                    Hide();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryEntry CurrentDomain = new DirectoryEntry();
                MessageBox.Show(CurrentDomain.Name);
                CurrentDomain.Close();
            }
            catch
            {
                MessageBox.Show("Нет подключения к AD");
            }
        }
    }
}
