using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.Main
{
    public partial class MapExportForm : Form
    {
        private List<Building> buildings;

        public MapExportForm()
        {
            InitializeComponent();
        }

        public MapExportForm(List<Building> buildings)
        {
            InitializeComponent();
            this.buildings = buildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (!buildings[i].delete)
                {
                    checkedListBox1.Items.Add(buildings[i].Name);
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                groupBox1.Enabled = false;
            else
                groupBox1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                groupBox1.Enabled = false;
            else
                groupBox1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private List<bool> buildenabled = new List<bool>();

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                buildenabled.Add(checkedListBox1.GetItemChecked(i));
            }
            if (radioButton2.Checked)
            {
                MainForm.ImageExport(buildenabled);
                Close();
            }
            else
            {
                MainForm.ExportListNE(buildenabled);
                Close();
            }
        }
    }
}
