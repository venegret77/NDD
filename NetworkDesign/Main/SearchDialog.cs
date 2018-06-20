using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign
{
    public partial class SearchDialog : Form
    {
        GroupOfMT GOMT;
        GroupOfBuildings GOB;
        GroupOfNE GONE;
        GroupOfNW GONW;
        public int _type = -1;
        public int _item = -1;

        public SearchDialog(GroupOfMT GOMT, GroupOfBuildings GOB, GroupOfNE GONE, GroupOfNW GONW)
        {
            InitializeComponent();
            this.GOMT = GOMT;
            this.GOB = GOB;
            this.GONE = GONE;
            this.GONW = GONW;
            FormClosed += SearchDialog_FormClosed;
        }

        private void PharseElems()
        {
            
        }

        private void SearchDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void SearchDialog_Load(object sender, EventArgs e)
        {

        }

        List<int> items = new List<int>();
        //здание - 1, сетевой элемент - 2, надпись - 3
        List<int> type = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (textBox1.Text != "")
            {
                string text = textBox1.Text.ToLower();
                for (int i = 0; i < GOB.Buildings.Count; i++)
                {
                    if (!GOB.Buildings[i].delete)
                    {
                        if (GOB.Buildings[i].Name.ToLower().Contains(text))
                        {
                            listBox1.Items.Add("Здание '" + GOB.Buildings[i].Name + "'");
                            items.Add(i);
                            type.Add(1);
                        }
                    }
                }
                for (int i = 0; i < GONE.NetworkElements.Count; i++)
                {
                    if (!GONE.NetworkElements[i].delete)
                    {
                        if (GONE.NetworkElements[i].Options.Name.ToLower().Contains(text))
                        {
                            listBox1.Items.Add("Сетевой элемент '" + GONE.NetworkElements[i].Options.Name + "'");
                            items.Add(i);
                            type.Add(2);
                        }
                        if (GONE.NetworkElements[i].Options.HostName.ToLower().Contains(text))
                        {
                            listBox1.Items.Add("Сетевой элемент '/" + GONE.NetworkElements[i].Options.Name + "'");
                            items.Add(i);
                            type.Add(2);
                        }
                        foreach (var ip in GONE.NetworkElements[i].Options.IPs)
                        {
                            if (ip.ToLower().Contains(text))
                            {
                                listBox1.Items.Add("Сетевой элемент '/" + GONE.NetworkElements[i].Options.HostName + "'");
                                items.Add(i);
                                type.Add(2);
                            }
                        }
                        foreach (var note in GONE.NetworkElements[i].notes.notes)
                        {
                            if (note.note.ToLower().Contains(text))
                            {
                                listBox1.Items.Add("Заметка '/" + note + "'");
                                items.Add(i);
                                type.Add(2);
                            }
                        }
                    }
                }
                for (int i = 0; i < GOMT.MyTexts.Count; i++)
                {
                    if (!GOMT.MyTexts[i].delete)
                    {
                        if (GOMT.MyTexts[i].text.ToLower().Contains(text))
                        {
                            listBox1.Items.Add("Надпись '" + GOMT.MyTexts[i].text + "'");
                            items.Add(i);
                            type.Add(3);
                        }
                    }
                }
                for (int i = 0; i < GONW.NetworkWires.Count; i++)
                {
                    if (!GONW.NetworkWires[i].delete)
                    {
                        foreach (var note in GONW.NetworkWires[i].notes.notes)
                        {
                            if (note.note.ToLower().Contains(text))
                            {
                                listBox1.Items.Add("Заметка '/" + note.note + "'");
                                items.Add(i);
                                type.Add(4);
                            }
                        }
                    }
                }
            }
        }

        private void Search()
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                button2.Enabled = true;
            else
                button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _type = type[listBox1.SelectedIndex];
            _item = items[listBox1.SelectedIndex];
            Close();
        }
    }
}
