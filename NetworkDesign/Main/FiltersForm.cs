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
            MainForm.filtres.Build = Build.Checked;
            MainForm.filtres.Ent = Ent.Checked;
            MainForm.filtres.IW = IW.Checked;
        }

        private void Ent_CheckedChanged(object sender, EventArgs e)
        {
            if (!Build.Checked)
                Ent.Checked = false;
            MainForm.filtres.Ent = Ent.Checked;
        }

        private void IW_CheckedChanged(object sender, EventArgs e)
        {
            if (!Build.Checked)
                IW.Checked = false;
            MainForm.filtres.IW = IW.Checked;
        }

        private void Text_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.Text = _Text.Checked;
        }

        private void NE_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.NE = NE.Checked;
        }

        private void NW_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.NW = NW.Checked;
        }

        private void Line_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.Line = Line.Checked;
        }

        private void Rect_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.Rect = Rect.Checked;
        }

        private void Poly_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.Poly = Poly.Checked;
        }

        private void Circ_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.filtres.Circ = Circ.Checked;
        }

        private void FiltersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
