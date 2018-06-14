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
    public partial class BuildExportImport : Form
    {
        private List<Building> buildings;

        public BuildExportImport()
        {
            InitializeComponent();
        }

        private List<int> build = new List<int>();

        public BuildExportImport(List<Building> buildings)
        {
            InitializeComponent();
            this.buildings = buildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (!buildings[i].delete)
                {
                    build.Add(i);
                    listBox1.Items.Add(buildings[i].Name);
                }
            }
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
            MainForm.SaveBuild(".build", "Building File", build[listBox1.SelectedIndex]);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MainForm.OpenBuild(".build", "Building File"))
                Close();
        }
    }
}
