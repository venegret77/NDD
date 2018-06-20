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
    public partial class ParametresForm : Form
    {
        const int requiredparameters = 0;
        GroupOfNE NetworkElements;

        public ParametresForm(ref GroupOfNE NetworkElements)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            this.NetworkElements = NetworkElements;
            for (int i = 0; i < MainForm.parametrs.Params.Count; i++)
            {
                listBox2.Items.Add(MainForm.parametrs.Params[i]);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (listBox2.SelectedIndex == -1)
            {
                listBox2.SelectedIndex = -1;
            }
            else
            {
                if (listBox2.SelectedIndex >= requiredparameters)
                {
                    button6.Enabled = true;
                    button7.Enabled = true;
                }
                textBox1.Text = listBox2.SelectedItem.ToString();
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" & textBox1.Text != " " & !MainForm.parametrs.Params.Contains(textBox1.Text))
                AddParam();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 & textBox1.Text != "" & textBox1.Text != " " & !MainForm.parametrs.Params.Contains(textBox1.Text))
                EditParam(listBox2.SelectedIndex);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Check(listBox2.SelectedIndex, listBox2.SelectedItem.ToString()))
            {
                if (MessageBox.Show("Вы действительно хотите удалить параметр?", "Удаление параметра", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int id = listBox2.SelectedIndex;
                    textBox1.Clear();
                    Element elem = new Element(13, id, MainForm.parametrs.Params[id], -3);
                    Element _elem = new Element(13, id, "", -3);
                    MainForm.MyMap.log.Add(new LogMessage("Удалил параметр", elem, _elem));
                    MainForm.parametrs.Remove(id);
                    NetworkElements.UpdateOptions(id);
                    listBox2.Items.RemoveAt(id);
                    listBox2.SelectedIndex = -1;
                    textBox1.Text = "";
                    listBox2.Focus();
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
        private bool Check(int id, string name)
        {
            foreach (var ne in NetworkElements.NetworkElements)
            {
                foreach (var param in ne.Options.Options)
                {
                    if (param.ID == id & param.Name == name & !param.isEmpty())
                        return false;
                }
            }
            return true;
            /*foreach (var item in NetworkElements.NetworkElements)
            {
                item.Options.RefreshID(id);
            }
            return true;*/
        }

        private void AddParam()
        {
            MainForm.parametrs.Add(textBox1.Text);
            int lastindex = MainForm.parametrs.Params.Count - 1;
            Element elem = new Element(13, lastindex, "", -1);
            Element _elem = new Element(13, lastindex, MainForm.parametrs.Params[lastindex], -1);
            MainForm.MyMap.log.Add(new LogMessage("Добавил параметр", elem, _elem));
            listBox2.Items.Add(textBox1.Text);
            listBox2.SelectedIndex = -1;
            textBox1.Text = "";
            listBox2.Focus();
        }

        private void EditParam(int id)
        {
            Element elem = new Element(13, id, MainForm.parametrs.Params[id], -2);
            MainForm.parametrs.Edit(id, textBox1.Text);
            Element _elem = new Element(13, id, MainForm.parametrs.Params[id], -2);
            MainForm.MyMap.log.Add(new LogMessage("Изменил параметр", elem, _elem));
            listBox2.Items[id] = textBox1.Text;
            listBox2.SelectedIndex = -1;
            textBox1.Text = "";
            listBox2.Focus();
            NetworkElements.UpdateOptions(id, "");
            int lastindex = MainForm.parametrs.Params.Count;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ParametresForm_Load(object sender, EventArgs e)
        {

        }
    }
}
