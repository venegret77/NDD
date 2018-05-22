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
    public partial class FiltersForm : Form
    {
        public FiltersForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            LoadSettings();
        }

        private void LoadSettings()
        {
            Line.Checked = MainForm.filtres.Line;
            Rect.Checked = MainForm.filtres.Rect;
            Poly.Checked = MainForm.filtres.Poly;
            Circ.Checked = MainForm.filtres.Circ;
            Build.Checked = MainForm.filtres.Build;
            Ent.Checked = MainForm.filtres.Ent;
            IW.Checked = MainForm.filtres.IW;
            NE.Checked = MainForm.filtres.NE;
            NW.Checked = MainForm.filtres.NW;
            _Text.Checked = MainForm.filtres.Text;
        }

        private void Build_CheckedChanged(object sender, EventArgs e)
        {
            if (!Build.Checked)
            {
                Ent.Checked = false;
                IW.Checked = false;
            }
            else
            {
                Ent.Checked = true;
                IW.Checked = true;
            }
        }

        private void Ent_CheckedChanged(object sender, EventArgs e)
        {
            if (!Build.Checked)
                Ent.Checked = false;
        }

        private void IW_CheckedChanged(object sender, EventArgs e)
        {
            if (!Build.Checked)
                IW.Checked = false;
        }

        private void Text_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void NE_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void NW_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Line_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Rect_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Poly_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Circ_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FiltersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.filtres.Line = Line.Checked;
            MainForm.filtres.Rect = Rect.Checked;
            MainForm.filtres.Poly = Poly.Checked;
            MainForm.filtres.Circ = Circ.Checked;
            MainForm.filtres.Build = Build.Checked;
            MainForm.filtres.Ent = Ent.Checked;
            MainForm.filtres.IW = IW.Checked;
            MainForm.filtres.NE = NE.Checked;
            MainForm.filtres.NW = NW.Checked;
            MainForm.filtres.Text = _Text.Checked;
        }
    }
}
