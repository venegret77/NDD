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
        const int requiredparameters = 4;
        const int requiredgroups = 5;
        public bool isEdit = false;
        Groups groups;
        GroupOfNE NE;

        public ParametresForm(ref Groups groups, ref GroupOfNE NE)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            this.groups = groups;
            this.NE = NE;
            for (int i = 0; i < MainForm.parametrs.Parametres.Count; i++)
            {
                listBox2.Items.Add(MainForm.parametrs.Parametres[i]);
            }
            foreach (var g in groups.GroupsOfNE)
                listBox1.Items.Add(g.name);
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
            if (textBox1.Text != "" & textBox1.Text != " ")
                AddParam();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 & textBox1.Text != "" & textBox1.Text != " ")
                EditParam(listBox2.SelectedIndex);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить параметр?", "Удаление параметра", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = listBox2.SelectedIndex;
                if (MainForm.parametrs.Delete(id, listBox2.SelectedItem.ToString(), ref groups))
                {
                    listBox2.Items.RemoveAt(id);
                    isEdit = true;
                }
                textBox1.Clear();
                listBox2.SelectedIndex = -1;
            }
        }

        private void AddParam()
        {
            isEdit = true;
            MainForm.parametrs.Add(textBox1.Text);
            listBox2.Items.Add(textBox1.Text);
            listBox2.SelectedIndex = -1;
            textBox1.Text = "";
            listBox2.Focus();
        }

        private void EditParam(int id)
        {
            isEdit = true;
            MainForm.parametrs.Edit(id, listBox2.Items[id].ToString(), textBox1.Text, ref groups, ref NE, true);
            listBox2.Items[id] = textBox1.Text;
            listBox2.SelectedIndex = -1;
            textBox1.Text = "";
            listBox2.Focus();
        }

        private void ParametresForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGroup();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить группу?", "Удаление группы", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = listBox1.SelectedIndex;
                if (MainForm.groups.Delete(id, ref MainForm.ImagesURL))
                {
                    listBox1.Items.RemoveAt(id);
                    isEdit = true;
                }
                listBox1.SelectedIndex = -1;
            }
        }

        private void AddGroup()
        {
            AddGroupForm addGroupForm = new AddGroupForm(MainForm.parametrs.Parametres);
            addGroupForm.ShowDialog();
            if (addGroupForm.dialogResult == DialogResult.Yes)
            {
                isEdit = true;
                MainForm.groups.Add(addGroupForm.g);
                listBox1.Items.Add(addGroupForm.g.name);
                listBox1.SelectedIndex = -1;
                listBox1.Focus();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                listBox1.SelectedIndex = -1;
                button3.Enabled = false;
            }
            else
            {
                if (listBox1.SelectedIndex >= requiredgroups)
                {
                    button3.Enabled = true;
                }
                else
                    button3.Enabled = false;
            }
        }
    }
}
