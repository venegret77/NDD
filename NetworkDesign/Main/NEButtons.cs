using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign.Main
{
    public class NEButtons
    {
        List<NEButton> neButtons = new List<NEButton>();

    }

    public struct NEButton
    {
        ToolStripButton toolStripButton;
        int id;
    }
}
