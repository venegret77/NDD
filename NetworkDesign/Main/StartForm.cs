using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NetworkDesign.Main
{
    public partial class StartForm : Form
    {
        public static bool isMainFormStart = false;

        public StartForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            try
            {
                MainForm.user = MainForm.Autorisation(out MainForm.edit);
                Text = "Авторизация успешна";
                label4.Text = MainForm.user.DisplayName + " (";
                if (MainForm.edit)
                    label4.Text += "администратор)";
                else
                    label4.Text += "пользователь)";
                tabControl1.Enabled = true;
            }
            catch
            {
                Name = "Авторизация не удалась";
                label4.Text = "DefaultUser (пользоваель)";
                tabControl1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isMainFormStart)
            {
                SizeRenderingArea mapSettings = new SizeRenderingArea(richTextBox1.Text, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                MainForm mainForm = new MainForm(mapSettings);
                mainForm.Show();
                isMainFormStart = true;
                Hide();
            }
            else
            {

            }
        }

        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isMainFormStart)
                Application.Exit();
            else
                Hide();
        }
    }
}
