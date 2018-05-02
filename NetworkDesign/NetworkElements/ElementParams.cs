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
        List<NetworkParametr> Options = new List<NetworkParametr>();
        bool add = false;
        bool edit = false;
        int id = -1;

        public ElementParams()
        {
            InitializeComponent();
            foreach(var item in MainForm.parametrs.Params)
            {
                checkedListBox1.Items.Add(item);
                listBox1.Items.Add("");
            }
        }

        //Добавить новый конструктор с передачей списка сетевых устройств

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
                    checkedListBox1.Items.Add(text);
                    listBox1.Items.Add("");
                    add = false;
                }
                else if (edit)
                {
                    MainForm.parametrs.Edit(id, text);
                    checkedListBox1.Items[id] = text;
                    edit = false;
                }
                else
                {
                    listBox1.Items[id] = text;
                }
                textBox1.Clear();
                textBox1.Enabled = false;
                checkedListBox1.Focus();
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (checkedListBox1.SelectedIndex == -1)
            {
                checkedListBox1.SelectedIndex = -1;
            }
            else
            {
                listBox1.SelectedIndex = checkedListBox1.SelectedIndex;
                id = checkedListBox1.SelectedIndex;
                if (checkedListBox1.GetItemChecked(id))
                {
                    textBox1.Text = listBox1.Items[id].ToString();
                    textBox1.Enabled = true;
                    textBox1.Focus();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (listBox1.SelectedIndex == -1)
            {
                listBox1.SelectedIndex = -1;
            }
            else
            {
                checkedListBox1.SelectedIndex = listBox1.SelectedIndex;
                id = checkedListBox1.SelectedIndex;
                if (checkedListBox1.GetItemChecked(id))
                {
                    textBox1.Text = listBox1.Items[id].ToString();
                    textBox1.Enabled = true;
                    textBox1.Focus();
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            edit = true;
            id = checkedListBox1.SelectedIndex;
            textBox1.Text = checkedListBox1.Items[id].ToString();
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
                    checkedListBox1.Items.RemoveAt(id);
                    listBox1.SelectedIndex = -1;
                    checkedListBox1.SelectedIndex = -1;
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
        /// <returns>true - параметр найден среди устройств; false - параметр не найден среди устройств</returns>
        private bool Check()
        {
            return true;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Очистка листбокс1
        }

        private void ElementParams_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Сохранение
        }
    }
}
