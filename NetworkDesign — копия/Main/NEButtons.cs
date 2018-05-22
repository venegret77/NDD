using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.Main
{
    public class NEButton
    {
        public ToolStripButton toolStripButton;
        public int id;

        public NEButton(ToolStripButton toolStripButton, int id)
        {
            this.toolStripButton = toolStripButton;
            this.id = id;
            toolStripButton.Click += ToolStripButton_Click;
        }

        public void ToolStripButton_Click(object sender, EventArgs e) => MainForm.MyMap.SetInstrument(MainForm.nebutnscount + id);
    }
}
