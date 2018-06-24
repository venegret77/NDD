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
    public partial class AddGroupForm : Form
    {
        public Group g = new Group();
        public DialogResult dialogResult = DialogResult.No;

        public AddGroupForm(List<string> parametres)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            foreach (var p in parametres)
                checkedListBox1.Items.Add(p);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> groups = new List<string>();
            foreach (var g in MainForm.groups.GroupsOfNE)
                groups.Add(g.name);
            if (textBox1.Text != "" & textBox1.Text != " " & !groups.Contains(textBox1.Text))
            {
                List<string> Parametres = new List<string>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                        Parametres.Add(checkedListBox1.Items[i].ToString());
                }
                g = new Group(Parametres, textBox1.Text, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked);
                dialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.No;
            Close();
        }
    }
}
