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
    public class NetworkSettings : ICloneable
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

        public override string ToString()
        {
            string settings = "";
            settings = "Устройство " + Name + ":" + Environment.NewLine;
            settings += "Пропускная способность: ";
            long kr = 0;
            if (Throughput >= 1000 & Throughput < 1000000)
                kr = 1000;
            else if (Throughput >= 1000000 & Throughput < 1000000000)
                kr = 1000000;
            else if (Throughput >= 1000000000 & Throughput < 1000000000000)
                kr = 1000000000;
            else if (Throughput >= 1000000000000)
                kr = 1000000000000;
            switch (kr)
            {
                case 0:
                    settings += (Throughput).ToString() + "(б/с)";
                    break;
                case 1000:
                    settings += (Throughput / kr).ToString() + "(Кб/с)";
                    break;
                case 1000000:
                    settings += (Throughput / kr).ToString() + "(Мб/с)";
                    break;
                case 1000000000:
                    settings += (Throughput / kr).ToString() + "(Гб/с)";
                    break;
                case 1000000000000:
                    settings += (Throughput / kr).ToString() + "(Тб/с)";
                    break;
            }
            settings += Environment.NewLine + "Порты: " + TotalPorts.ToString() + " - общее количество портов; " + BusyPorts.ToString() + " - количество занятых портов" + Environment.NewLine;
            foreach (var opt in Options)
            {
                settings += opt.ToString() + Environment.NewLine;
            }
            return settings;
        }

        public object Clone()
        {
            List<NetworkParametr> options = new List<NetworkParametr>();
            foreach (var p in Options)
                options.Add(new NetworkParametr(p.ID, p.Name, p.Value));
            return new NetworkSettings
            {
                BusyPorts = this.BusyPorts,
                Name = this.Name,
                Throughput = this.Throughput,
                TotalPorts = this.TotalPorts,
                Options = options
            };
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

        public NetworkParametr()
        {
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

        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }

    public class Parametrs : ICloneable
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
            parametrs.Add("IP");
            parametrs.Add("MAC");
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

        static public void _Open()
        {
            Parametrs parametrs = new Parametrs();
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(parametrs);
                MainForm.parametrs = parametrs;
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\NetworkSettings"))
            {
                Save(parametrs);
                MainForm.parametrs = parametrs;
            }
            XmlSerializer formatter = new XmlSerializer(typeof(Parametrs));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings", FileMode.Open))
            {
                Parametrs _parametrs = (Parametrs)formatter.Deserialize(fs);
                Parametrs __parametres = (Parametrs)MainForm.parametrs.Clone();
                //MainForm.parametrs = new Parametrs();
                MainForm.parametrs = (Parametrs)_parametrs.Clone();
                int id = -1;
                for (int i = 0; i < __parametres.Params.Count; i++)
                {
                    id = -1;
                    for (int j = 0; j < MainForm.parametrs.Params.Count; j++)
                    {
                        if (MainForm.parametrs.Params[j] == __parametres.Params[i])
                        {
                            id = i;
                            break;
                        }
                    }
                    if (id != -1)
                    {
                        __parametres.Params.RemoveAt(id);
                        i--;
                    }
                    else
                        MainForm.parametrs.Add(__parametres.Params[i]);
                }
            }
        }

        public object Clone()
        {
            List<string> _params = new List<string>();
            foreach (var p in Params)
            {
                _params.Add(p);
            }
            return new Parametrs
            {
                Params = _params
            };
        }
    }
}
