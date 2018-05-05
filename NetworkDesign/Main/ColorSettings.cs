using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NetworkDesign
{
    public class ColorSettings
    {
        [XmlIgnore()]
        public Color LinesColor;
        [XmlIgnore()]
        public Color PolygonColor;
        [XmlIgnore()]
        public Color RectColor;
        [XmlIgnore()]
        public Color BuildColor;
        [XmlIgnore()]
        public Color ActiveElemColor;
        [XmlIgnore()]
        public Color CircleColor;
        [XmlIgnore()]
        public Color EntranceColor;
        [XmlIgnore()]
        public Color InputWireColor;
        public float LineWidth;
        public int EntranceRadius;
        public int InputWireRadius;

        [XmlElement("LinesColor")]
        public int argbl
        {
            get { return LinesColor.ToArgb(); }
            set { LinesColor = Color.FromArgb(value); }
        }

        [XmlElement("PolygonColor")]
        public int argbp
        {
            get { return PolygonColor.ToArgb(); }
            set { PolygonColor = Color.FromArgb(value); }
        }

        [XmlElement("RectColor")]
        public int argbr
        {
            get { return RectColor.ToArgb(); }
            set { RectColor = Color.FromArgb(value); }
        }

        [XmlElement("BuildColor")]
        public int argbb
        {
            get { return BuildColor.ToArgb(); }
            set { BuildColor = Color.FromArgb(value); }
        }

        [XmlElement("ActiveElemColor")]
        public int argba
        {
            get { return ActiveElemColor.ToArgb(); }
            set { ActiveElemColor = Color.FromArgb(value); }
        }

        [XmlElement("CircleColor")]
        public int argbc
        {
            get { return CircleColor.ToArgb(); }
            set { CircleColor = Color.FromArgb(value); }
        }

        [XmlElement("EntranceColor")]
        public int argbe
        {
            get { return EntranceColor.ToArgb(); }
            set { EntranceColor = Color.FromArgb(value); }
        }

        [XmlElement("InputWireColor")]
        public int argbiw
        {
            get { return InputWireColor.ToArgb(); }
            set { InputWireColor = Color.FromArgb(value); }
        }

        public ColorSettings(string todefault)
        {
            LinesColor = Color.Black;
            PolygonColor = Color.Black;
            RectColor = Color.Black;
            ActiveElemColor = Color.Black;
            BuildColor = Color.Black;
            CircleColor = Color.Black;
            EntranceColor = Color.Black;
            InputWireColor = Color.Black;
            EntranceRadius = 5;
            InputWireRadius = 5;
            LineWidth = 1;
        }

        public ColorSettings()
        {
        }

        static public void Save(ColorSettings _cs)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ColorSettings));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\ColorSettings", FileMode.Create))
            {
                formatter.Serialize(fs, _cs);
            }
        }

        static public ColorSettings Open()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(new ColorSettings(""));
                return new ColorSettings("");
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\ColorSettings"))
            {
                Save(new ColorSettings(""));
                return new ColorSettings("");
            }
            XmlSerializer formatter = new XmlSerializer(typeof(ColorSettings));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\ColorSettings", FileMode.Open))
            {
                return (ColorSettings)formatter.Deserialize(fs);
            }
        }
    }
}
