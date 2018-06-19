using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.NetworkElements
{
    public partial class NESettings : Form
    {
        public GroupOfNE NetworkElements;
        const int requiredparameters = 0;
        public NetworkSettings Options;
        int id = -1;
        string _Name = "";
        Int64 kr = 1000000;
        public Notes notes;

        public NESettings()
        {
            InitializeComponent();
        }

        public NESettings(NetworkSettings Options, GroupOfNE NetworkElements, ref Notes note)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.Options = Options;
            this.NetworkElements = NetworkElements;
            this.notes = note;
            PharseOptions();
        }

        private void RefreshLable()
        {
            switch (kr)
            {
                case 0:
                    label4.Text = "(б/с)";
                    break;
                case 1000:
                    label4.Text = "(Кб/с)";
                    break;
                case 1000000:
                    label4.Text = "(Мб/с)";
                    break;
                case 1000000000:
                    label4.Text = "(Гб/с)";
                    break;
                case 1000000000000:
                    label4.Text = "(Тб/с)";
                    break;
            }
        }

        private void PharseOptions()
        {
            if (Options.Throughput == 0)
            {
                kr = 1000000;
                numericUpDown1.Value = 100;
            }
            else
            {
                kr = 0;
                if (Options.Throughput >= 1000 & Options.Throughput < 1000000)
                    kr = 1000;
                else if (Options.Throughput >= 1000000 & Options.Throughput < 1000000000)
                    kr = 1000000;
                else if (Options.Throughput >= 1000000000 & Options.Throughput < 1000000000000)
                    kr = 1000000000;
                else if (Options.Throughput >= 1000000000000)
                    kr = 1000000000000;
                if (kr != 0)
                    numericUpDown1.Value = (decimal)(Options.Throughput / (double)kr);
                else
                    numericUpDown1.Value = (decimal)Options.Throughput;
            }
            RefreshLable();
            textBox4.Text = Options.Name;
            textBox2.Text = Options.HostName;
            if (isEmpty(textBox4.Text))
                textBox4.Text = "Новое устройство";
            numericUpDown2.Value = Options.TotalPorts;
            numericUpDown2.Minimum = Options.BusyPorts;
            foreach (var ip in Options.IPs)
                comboBox1.Items.Add(ip.ToString());
            if (Options.isPing)
                radioButton1.Checked = true;
            if (comboBox1.Items.Count >= 1)
                comboBox1.SelectedIndex = 0;
            RefreshPorts();
            for (int i = 0; i < MainForm.parametrs.Params.Count; i++)
            {
                listBox2.Items.Add(MainForm.parametrs.Params[i]);
                listBox1.Items.Add("");
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                foreach (var item in Options.Options)
                {
                    if (item.Name == listBox2.Items[i].ToString())
                    {
                        listBox1.Items[i] = item.Value;
                    }
                }
            }
            foreach (var note in notes.notes)
            {
                listBox3.Items.Add(note.note);
            }
        }

        public void RefreshPorts()
        {
            int ports = Options.TotalPorts - Options.BusyPorts;
            label5.Text = "Свободных портов: " + ports;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = textBox1.Text;
                listBox1.Items[id] = text;
                textBox1.Clear();
                textBox1.Enabled = false;
                listBox1.Focus();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button7.Enabled = false;
            textBox1.Clear();
            if (listBox1.SelectedIndex == -1)
            {
                listBox1.SelectedIndex = -1;
            }
            else
            {
                if (listBox1.SelectedIndex >= requiredparameters)
                {
                    button6.Enabled = true;
                    button7.Enabled = true;
                }
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                id = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[id].ToString();
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }

        private void ElementParams_FormClosed(object sender, FormClosedEventArgs e) => SaveParams();

        private void SaveParams()
        {
            Options.Options.Clear();
            Options.Name = textBox4.Text;
            Options.HostName = textBox2.Text;
            Options.IPs.Clear();
            foreach (var ip in comboBox1.Items)
                Options.IPs.Add(ip.ToString());
            if (radioButton1.Checked)
                Options.isPing = true;
            Options.TotalPorts = (int)numericUpDown2.Value;
            if (kr != 0)
                Options.Throughput = (Int64)(numericUpDown1.Value * kr);
            else
                Options.Throughput = (Int64)(numericUpDown1.Value);
            for (int i = 0; i < requiredparameters; i++)
            {
                Options.Options.Add(new NetworkParametr(i, listBox2.Items[i].ToString(), listBox1.Items[i].ToString()));
            }
            for (int i = requiredparameters; i < listBox1.Items.Count; i++)
            {
                if (!isEmpty(listBox1.Items[i].ToString()))
                {
                    Options.Options.Add(new NetworkParametr(i, listBox2.Items[i].ToString(), listBox1.Items[i].ToString()));
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
            button6.Enabled = false;
            button7.Enabled = false;
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
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                id = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[id].ToString();
                textBox1.Enabled = true;
                textBox1.Focus();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
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

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Options.TotalPorts = (int)numericUpDown2.Value;
            numericUpDown2.Minimum = Options.BusyPorts;
            RefreshPorts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    IPHostEntry ips = Dns.GetHostByName(textBox2.Text);
                    comboBox1.Items.Clear();
                    foreach (var ip in ips.AddressList)
                    {
                        comboBox1.Items.Add(ip.ToString());
                    }
                    if (comboBox1.Items.Count >= 1)
                        comboBox1.SelectedIndex = 0;
                    Options.isPing = true;
                    radioButton1.Checked = true;
                }
                else
                {
                    GetName();
                }
            }
            catch
            {
                GetName();
            }
        }

        private void GetName()
        {
            if (comboBox1.Items.Count != 0)
            {
                if (_GetName())
                {
                    Options.isPing = true;
                    radioButton1.Checked = true;
                }
                else
                {
                    Options.isPing = false;
                    radioButton1.Checked = false;
                }
            }
        }

        int ip = 0;

        private bool _GetName()
        {
            try
            {
                IPHostEntry ips = Dns.GetHostByAddress(comboBox1.Items[ip].ToString());
                textBox2.Text = ips.HostName;
                comboBox1.Items.Clear();
                foreach (var ip in ips.AddressList)
                    comboBox1.Items.Add(ip.ToString());
                if (comboBox1.Items.Count >= 1)
                    comboBox1.SelectedIndex = 0;
                ip = 0;
                return true;
            }
            catch
            {
                if (ip != comboBox1.Items.Count - 1)
                {
                    ip++;
                    GetName();
                }
                ip = 0;
                return false;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            if (listBox3.SelectedIndex != -1)
            {
                textBox3.Text = listBox3.SelectedItem.ToString().Split(new char[] { ':' }).Last();
                textBox3.Focus();
                button3.Enabled = true;
                button4.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 58)
                e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" & textBox3.Text != " ")
            {
                listBox3.Items.Add(MainForm.user.DisplayName.ToString() + ":" + textBox3.Text);
                notes.Add(new Note(MainForm.user.DisplayName.ToString() + ":" + textBox3.Text, MainForm.user));
            }
            textBox3.Text = "";
            listBox3.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = MainForm.user.DisplayName.ToString() + ":" + textBox3.Text;
            if (notes.Edit(listBox3.SelectedIndex, text))
                listBox3.Items[listBox3.SelectedIndex] = text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (notes.Remove(listBox3.SelectedIndex))
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
            textBox1.Clear();
            listBox1.SelectedIndex = -1;
            listBox2.SelectedIndex = -1;
            listBox1.Focus();
            textBox1.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items[listBox1.SelectedIndex] = "";
            textBox1.Clear();
            listBox1.SelectedIndex = -1;
            listBox2.SelectedIndex = -1;
            listBox1.Focus();
            textBox1.Enabled = false;
            /**/
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & comboBox1.Text != "")
            {
                foreach (var item in comboBox1.Items)
                    if (comboBox1.Text == item.ToString())
                        e.Handled = true;
                if (!e.Handled)
                    comboBox1.Items.Add(comboBox1.Text);
                e.Handled = true;
                comboBox1.Text = "";
            }
        }
    }
}
