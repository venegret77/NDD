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
        /// <summary>
        /// Наименование устройства
        /// </summary>
        public string Name = "Новое устройство";
        /// <summary>
        /// Общее количество сетевых портов
        /// </summary>
        public int TotalPorts = 0;
        /// <summary>
        /// Количество занятых сетевых портов
        /// </summary>
        public int BusyPorts = 0;
        /// <summary>
        /// Пропускная способность
        /// </summary>
        public Int64 Throughput = 100000000;

        public NetworkSettings()
        {
        }

        public NetworkSettings(string name, int totalPorts)
        {
            Name = name;
            TotalPorts = totalPorts;
        }

        internal void RefreshID(int id)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i].ID > id)
                    Options[i].SetNewID();
            }
        }

        public bool CheckPorts()
        {
            if (TotalPorts - BusyPorts > 0)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Наменование параметра и значение
    /// </summary>
    public class NetworkParametr
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int ID;
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name;
        /// <summary>
        /// Значение
        /// </summary>
        public string Value;

        public NetworkParametr(int iD, string name, string value)
        {
            ID = iD;
            Name = name;
            Value = value;
        }

        public void SetNewID()
        {
            ID--;
            Name = MainForm.parametrs.Params[ID];
        }

        public bool isEmpty()
        {
            if (Value == "" || Value == " ")
                return true;
            else
                return false;
        }

        internal void SetNewName(int id, string name)
        {
            Name = MainForm.parametrs.Params[id];
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
