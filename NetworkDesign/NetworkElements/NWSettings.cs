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

        public NWSettings(long throughput, ref Notes notes)
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
                listBox1.Items.Add(note.note);
            }
        }

        private void NWSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (kr != 0)
                Throughput = (Int64)(numericUpDown1.Value * kr);
            else
                Throughput = (Int64)(numericUpDown1.Value);
            /*Notes _notes = new Notes();
            for (int i = 0; i < notes.notes.Count; i++)
            {
                notes.notes[i].note = listBox1.Items[i].ToString();
                _notes.Add(new Note(listBox1.Items[i].ToString(), notes.notes[i].user));
            }
            notes = _notes.Copy();*/
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
                textBox1.Text = listBox1.SelectedItem.ToString().Split(new char[] { ':' }).Last();
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
            if (notes.Remove(listBox1.SelectedIndex))
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = MainForm.user.DisplayName.ToString() + ":" + textBox1.Text;
            if (notes.Edit(listBox1.SelectedIndex, text))
                listBox1.Items[listBox1.SelectedIndex] = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" & textBox1.Text != " ")
            {
                listBox1.Items.Add(MainForm.user.DisplayName.ToString() + ":" + textBox1.Text);
                notes.Add(new Note(MainForm.user.DisplayName.ToString() + ":" + textBox1.Text, MainForm.user));
            }
            textBox1.Text = "";
            listBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 58)
                e.Handled = true;
        }
    }
}
