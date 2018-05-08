using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NetworkDesign.NetworkElements
{
    /// <summary>
    /// Сетевые параметры
    /// </summary>
    public class NetworkSettings
    {
        /// <summary>
        /// Список параметров сетевых устройств
        /// </summary>
        public List<NetworkParametr> Options = new List<NetworkParametr>();

        public NetworkSettings()
        {
        }

        public NetworkSettings(string todefault)
        {
            Options.Add(new NetworkParametr(0, ""));
            Options.Add(new NetworkParametr(1, ""));
        }

        internal void RefreshID(int id)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i].ID > id)
                    Options[i].SetNewID();
            }
        }
    }

    /// <summary>
    /// Наменование параметра и значение
    /// </summary>
    public struct NetworkParametr
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int ID;
        /// <summary>
        /// Значение
        /// </summary>
        public string Value;

        public void SetNewID()
        {
            ID--;
        }

        public bool isEmpty()
        {
            if (Value == "" || Value == " ")
                return true;
            else
                return false;
        }

        public NetworkParametr(int iD, string value)
        {
            ID = iD;
            Value = value;
        }
    }

    public class Parametrs
    {
        public List<string> Params = new List<string>();

        public Parametrs()
        {
        }

        public void Add(string _name)
        {
            Params.Add(_name);
        }

        public void Remove(int id)
        {
            Params.RemoveAt(id);
        }

        public void Edit(int id, string _name)
        {
            Params[id] = _name;
        }

        static public void Save(Parametrs _params)
        {
            if (_params.Params.Count <= 0)
                _params.Params = null;
            XmlSerializer formatter = new XmlSerializer(typeof(Parametrs));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NetworkSettings", FileMode.Create))
            {
                formatter.Serialize(fs, _params);
            }
        }

        static public Parametrs Open()
        {
            Parametrs parametrs = new Parametrs();
            parametrs.Add("Наименование элемента");
            parametrs.Add("Количество сетевых портов");
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(parametrs);
                return parametrs;
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\NetworkSettings"))
            {
                Save(parametrs);
                return parametrs;
            }
            XmlSerializer formatter = new XmlSerializer(typeof(Parametrs));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NetworkSettings", FileMode.Open))
            {
                return (Parametrs)formatter.Deserialize(fs);
            }
        }
    }
}
