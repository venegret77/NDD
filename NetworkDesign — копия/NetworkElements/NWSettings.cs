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

        public NWSettings(long throughput)
        {
            InitializeComponent();
            Throughput = throughput;
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
        }

        private void NWSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (kr != 0)
                Throughput = (Int64)(numericUpDown1.Value * kr);
            else
                Throughput = (Int64)(numericUpDown1.Value);
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
    }
}
