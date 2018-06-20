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
        SizeRenderingArea DefaultSettings = new SizeRenderingArea("DefaultMap", 1000, 1000);
        MainForm mf;

        public StartForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
            try
            {
                MainForm.user = MainForm.Autorisation(out MainForm.edit);
                if (MainForm.user.DisplayName.Length < 2)
                {
                    DisplayedNameForm displayedNameForm = new DisplayedNameForm();
                    while (displayedNameForm.dialogResult != DialogResult.OK)
                    {
                        displayedNameForm.ShowDialog();
                    }
                }
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
            //if (!MainForm.isInitMap)
                //mf = new MainForm(DefaultSettings);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "" & richTextBox1.Text != " ")
            {
                SizeRenderingArea mapSettings = new SizeRenderingArea(richTextBox1.Text, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                //MainForm.MyMap.MapLoad(new Map(mapSettings));
                if (!MainForm.isInitMap)
                {
                    mf = new MainForm(mapSettings);
                    mf.Show();
                    MainForm.isInitMap = true;
                }
                else
                {
                    MainForm.MyMap.MapLoad(mapSettings);
                    mf.Show();
                }
                Hide();
            }
            else
            {
                MessageBox.Show("Пожалуйста введите название");
            }
        }

        private void StartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!MainForm.isInitMap)
                Application.Exit();
            else
                Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!MainForm.isInitMap)
            {
                mf = new MainForm(DefaultSettings);
                MainForm.isInitMap = true;
            }
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Network Design Map File|*.ndm|Network Design Map File (Template)|*.ndmt"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileName.Contains(".ndm") & !openFileDialog.FileName.Contains(".ndmt"))
                    {
                        if (MainForm.OpenMap(openFileDialog.FileName))
                        {
                            mf.Show();
                            mf.CheckButtons(true);
                            mf.Text = MainForm.MyMap.sizeRenderingArea.Name;
                            Hide();
                        }
                    }
                    else if (openFileDialog.FileName.Contains(".ndmt"))
                    {
                        if (MainForm.OpenTemplateMap(openFileDialog.FileName))
                        {
                            mf.Show();
                            mf.CheckButtons(true);
                            mf.Text = MainForm.MyMap.sizeRenderingArea.Name;
                            Hide();
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
