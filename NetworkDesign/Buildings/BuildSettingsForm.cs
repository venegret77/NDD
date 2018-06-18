using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.Buildings
{
    public partial class BuildSettingsForm : Form
    {
        public string BuildName = "";
        public DialogResult dialogResult;
        List<bool> emptyfloors = new List<bool>();
        public List<bool> floors = new List<bool>();
        private Building temp;

        public BuildSettingsForm(Building building, List<bool> emptyfloors)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            temp = building;
            this.emptyfloors = emptyfloors;
            BuildName = textBox1.Text = building.Name;
            for (int i = 0; i < building.floors_name.Count; i++)
            {
                checkedListBox1.Items.Add(building.floors_name[i]);
                checkedListBox1.SetItemCheckState(i, CheckState.Checked);
            }
            if (!building.basement)
            {
                checkedListBox1.Items.Insert(0, "Подвал");
                emptyfloors.Insert(0, true);
                checkedListBox1.SetItemCheckState(0, CheckState.Unchecked);
            }
            if (!building.loft)
            {
                emptyfloors.Add(true);
                checkedListBox1.Items.Add("Чердак");
                checkedListBox1.SetItemCheckState(checkedListBox1.Items.Count - 1, CheckState.Unchecked);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                floors.Add(checkedListBox1.GetItemChecked(i));
            dialogResult = DialogResult.Yes;
            BuildName = textBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.No;
            Close();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!emptyfloors[e.Index])
                e.NewValue = CheckState.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool b = true;
            foreach (var iw in temp.InputWires.InputWires.Circles)
            {
                if (!iw.side & !iw.delete)
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                int pos = checkedListBox1.Items.Count - 1;
                checkedListBox1.Items.Insert(pos, "Этаж " + pos.ToString());
                emptyfloors.Insert(pos, true);
                checkedListBox1.SetItemCheckState(pos, CheckState.Checked);
            }
            else
            {
                MessageBox.Show("Невозможно добавить этаж. Удалите входы проводов на крыше здания");
            }
        }
    }
}
