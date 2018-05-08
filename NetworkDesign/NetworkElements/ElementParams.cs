using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.NetworkElements
{
    public partial class ElementParams : Form
    {
        GroupOfNE NetworkElements;
        const int requiredparameters = 2;
        public NetworkSettings Options;
        bool add = false;
        bool edit = false;
        int id = -1;

        /*public ElementParams()
        {
            InitializeComponent();
        }*/

        public ElementParams(NetworkSettings Options, ref GroupOfNE NetworkElements)
        {
            InitializeComponent();
            this.Options = Options;
            this.NetworkElements = NetworkElements;
            PharseOptions();
        }

        private void PharseOptions()
        {
            foreach (var param in MainForm.parametrs.Params)
            {
                listBox2.Items.Add(param);
                listBox1.Items.Add("");
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                foreach (var item in Options.Options)
                {
                    if (item.ID == i)
                    {
                        listBox1.Items[i] = item.Value;
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            add = true;
            textBox1.Enabled = true;
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = textBox1.Text;
                if (add)
                {
                    MainForm.parametrs.Add(text);
                    listBox2.Items.Add(text);
                    listBox1.Items.Add("");
                    add = false;
                }
                else if (edit)
                {
                    MainForm.parametrs.Edit(id, text);
                    listBox2.Items[id] = text;
                    edit = false;
                }
                else
                {
                    listBox1.Items[id] = text;
                }
                textBox1.Clear();
                textBox1.Enabled = false;
                listBox2.Focus();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            textBox1.Clear();
            if (listBox1.SelectedIndex == -1)
            {
                listBox1.SelectedIndex = -1;
            }
            else
            {
                if (listBox1.SelectedIndex >= requiredparameters)
                {
                    toolStripButton2.Enabled = true;
                    toolStripButton3.Enabled = true;
                }
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                id = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[id].ToString();
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            edit = true;
            id = listBox2.SelectedIndex;
            textBox1.Text = listBox2.Items[id].ToString();
            textBox1.Enabled = true;
            textBox1.Focus();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("", "Удалить?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Check())
                {
                    textBox1.Clear();
                    MainForm.parametrs.Remove(id);
                    listBox1.Items.RemoveAt(id);
                    listBox2.Items.RemoveAt(id);
                    listBox1.SelectedIndex = -1;
                    listBox2.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Невозможно удалить параметр, т.к. он используется в других устройствах");
                }
            }
        }

        /// <summary>
        /// Проверка на то, используется ли данный параметр в других устройствах
        /// </summary>
        /// <returns>true - параметр не найден среди устройств; false - параметр найден среди устройств</returns>
        private bool Check()
        {
            foreach (var ne in NetworkElements.NetworkElements)
            {
                foreach (var param in ne.Options.Options)
                {
                    if (param.ID == id & !param.isEmpty())
                        return false;
                }
            }
            foreach (var item in NetworkElements.NetworkElements)
            {
                item.Options.RefreshID(id);
            }
            return true;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Очистка листбокс1
        }

        private void ElementParams_FormClosed(object sender, FormClosedEventArgs e) => SaveParams();

        private void SaveParams()
        {
            Options = new NetworkSettings();
            Options.Options.Add(new NetworkParametr(0, listBox1.Items[0].ToString()));
            Options.Options.Add(new NetworkParametr(1, listBox1.Items[1].ToString()));
            for (int i = 2; i < listBox1.Items.Count; i++)
            {
                if (!isEmpty(listBox1.Items[i].ToString()))
                {
                    Options.Options.Add(new NetworkParametr(i, listBox1.Items[i].ToString()));
                }
            }
        }

        private bool isEmpty(string Value)
        {
            if (Value == "" || Value == " ")
                return true;
            else
                return false;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            textBox1.Clear();
            if (listBox2.SelectedIndex == -1)
            {
                listBox2.SelectedIndex = -1;
            }
            else
            {
                if (listBox2.SelectedIndex >= requiredparameters)
                {
                    toolStripButton2.Enabled = true;
                    toolStripButton3.Enabled = true;
                }
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                id = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[id].ToString();
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }
    }
}
