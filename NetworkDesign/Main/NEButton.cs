using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NetworkDesign.Main
{
    public class ID_TEXT
    {
        public List<int> ID = new List<int>();
        public List<string> TEXT = new List<string>();

        public ID_TEXT()
        {
        }

        public void ADD(int id, string text)
        {
            ID.Add(id);
            TEXT.Add(text);
        }
    }

    public class NEButton
    {
        public ToolStripButton toolStripButton;
        public int id;
        public string textname;
        public int tsid;

        public NEButton(ToolStripButton toolStripButton, int id, string textname, int tsid)
        {
            this.toolStripButton = toolStripButton;
            this.id = id;
            this.textname = textname;
            this.tsid = tsid;
            toolStripButton.Click += ToolStripButton_Click;
        }

        public NEButton()
        {
        }

        public void ToolStripButton_Click(object sender, EventArgs e) => MainForm.MyMap.SetInstrument(MainForm.nebutnscount + id);

        static public void Save(ID_TEXT _cs)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ID_TEXT));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NEButtons", FileMode.Create))
            {
                formatter.Serialize(fs, _cs);
            }
        }

        static public ID_TEXT Open()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(new ID_TEXT());
                return new ID_TEXT();
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\NEButtons"))
            {
                Save(new ID_TEXT());
                return new ID_TEXT();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(ID_TEXT));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NEButtons", FileMode.Open))
            {
                return (ID_TEXT)formatter.Deserialize(fs);
            }
        }

        /*static public ID_TEXT _Open()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(new ID_TEXT());
                return new ID_TEXT();
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\NEButtons"))
            {
                Save(new ID_TEXT());
                return new ID_TEXT();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(ID_TEXT));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NEButtons", FileMode.Open))
            {
                return (ID_TEXT)formatter.Deserialize(fs);
            }
        }*/
    }
}
