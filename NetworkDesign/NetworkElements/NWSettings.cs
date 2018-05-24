using NetworkDesign.Main;
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
    public partial class NWSettings : Form
    {
        Int64 kr = 1000000;
        public Int64 Throughput = 100000000;
        bool edit = false;
        public Notes notes;

        public NWSettings(long throughput, Notes notes)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            Throughput = throughput;
            this.notes = notes;
            PharseOptions();
        }

        private void RefreshLable()
        {
            switch (kr)
            {
                case 0:
                    label2.Text = "(б/с)";
                    break;
                case 1000:
                    label2.Text = "(Кб/с)";
                    break;
                case 1000000:
                    label2.Text = "(Мб/с)";
                    break;
                case 1000000000:
                    label2.Text = "(Гб/с)";
                    break;
                case 1000000000000:
                    label2.Text = "(Тб/с)";
                    break;
            }
        }

        private void PharseOptions()
        {
            if (Throughput == 0)
            {
                kr = 1000000;
                numericUpDown1.Value = 100;
            }
            else
            {
                kr = 0;
                if (Throughput >= 1000 & Throughput < 1000000)
                    kr = 1000;
                else if (Throughput >= 1000000 & Throughput < 1000000000)
                    kr = 1000000;
                else if (Throughput >= 1000000000 & Throughput < 1000000000000)
                    kr = 1000000000;
                else if (Throughput >= 1000000000000)
                    kr = 1000000000000;
                if (kr != 0)
                    numericUpDown1.Value = (decimal)(Throughput / (double)kr);
                else
                    numericUpDown1.Value = (decimal)Throughput;
            }
            RefreshLable();
            foreach (var note in notes.notes)
            {
                listBox2.Items.Add(note.user.login + ": ");
                listBox1.Items.Add(note.note);
            }
        }

        private void NWSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (kr != 0)
                Throughput = (Int64)(numericUpDown1.Value * kr);
            else
                Throughput = (Int64)(numericUpDown1.Value);
            Notes _notes = new Notes();
            for (int i = 0; i < notes.notes.Count; i++)
            {
                notes.notes[i].note = listBox1.Items[i].ToString();
                _notes.Add(new Note(listBox1.Items[i].ToString(), (User)notes.notes[i].user.Clone()));
            }
            notes = _notes.Copy();
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 1000 & kr != 1000000000000)
            {
                numericUpDown1.Value /= 1000;
                kr *= 1000;
                if (kr == 0)
                    kr = 1000;
                RefreshLable();
            }
            else if (numericUpDown1.Value < 1 & kr != 0)
            {
                numericUpDown1.Value *= 1000;
                kr /= 1000;
                RefreshLable();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            edit = false;
            textBox1.Text = "";
            if (listBox1.SelectedIndex != -1)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
                textBox1.Focus();
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            edit = true;
            listBox1.SelectedItem = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" & textBox1.Text != " ")
            {
                listBox1.Items.Add(textBox1.Text);
                listBox2.Items.Add(MainForm.user.login + ":");
                notes.Add(new Note("", MainForm.user));
            }
            textBox1.Text = "";
            listBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (edit)
                    listBox1.SelectedItem = textBox1.Text;
                else
                {
                    listBox1.Items.Add(textBox1.Text);
                    listBox2.Items.Add(MainForm.user.login + ":");
                    notes.Add(new Note("", MainForm.user));
                }
                textBox1.Text = "";
                listBox1.Focus();
            }
        }
    }
}
