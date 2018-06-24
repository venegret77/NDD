using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        /// Сетевое имя устройства
        /// </summary>
        public string HostName = "";
        public List<string> IPs = new List<string>();
        /// <summary>
        /// Общее количество сетевых портов
        /// </summary>
        public int TotalPorts = 0;
        /// <summary>
        /// Количество занятых сетевых портов
        /// </summary>
        public int BusyPorts = 0;
        //public IPHostEntry AddressList = null;
        /// <summary>
        /// Пропускная способность
        /// </summary>
        public Int64 Throughput = 100000000;
        /// <summary>
        /// Показывает, включено или нет устройство
        /// </summary>
        public bool isPing = false;
        /// <summary>
        /// Конструктор
        /// </summary>
        public NetworkSettings()
        {
        }
        /// <summary>
        /// Проверка доступности портов
        /// </summary>
        /// <returns>Возвращает значение, есть ли свободный порт или нет</returns>
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
            settings = "Устройство " + Name + " (сетевое имя: " + HostName + "):" + Environment.NewLine;
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
            foreach (var ip in IPs)
                settings += "IP: " + ip + Environment.NewLine;
            foreach (var opt in Options)
            {
                settings += opt.ToString() + Environment.NewLine;
            }
            return settings;
        }

        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<string> _IPs = new List<string>();
            foreach (var ip in IPs)
                _IPs.Add(ip);
            List<NetworkParametr> options = new List<NetworkParametr>();
            foreach (var p in Options)
                options.Add(new NetworkParametr(p.Name, p.Value));
            return new NetworkSettings
            {
                BusyPorts = this.BusyPorts,
                HostName = this.HostName,
                Throughput = this.Throughput,
                TotalPorts = this.TotalPorts,
                Options = options,
                Name = this.Name,
                isPing = this.isPing,
                IPs = _IPs
            };
        }
    }

    /// <summary>
    /// Наменование параметра, идентификатор и значение
    /// </summary>
    public class NetworkParametr
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name;
        /// <summary>
        /// Значение
        /// </summary>
        public string Value;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="value">Значение</param>
        public NetworkParametr(string name, string value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public NetworkParametr()
        {
        }
        /// <summary>
        /// Проверка пустое значение или нет
        /// </summary>
        /// <returns>Возвращает, пустое значение или нет</returns>
        public bool isEmpty()
        {
            if (Value == "" || Value == " ")
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }

    public class GroupsOfNE
    {

    }

    public class GroupOfParametres
    {
        public List<string> Parametres = new List<string>();

        public GroupOfParametres() { }

        public void Add(string name)
        {
            if (!Parametres.Contains(name))
            {
                Parametres.Add(name);
                int lastindex = Parametres.Count - 1;
                Element elem = new Element(13, lastindex, "", -1);
                Element _elem = new Element(13, lastindex, Parametres[lastindex], -1);
                MainForm.MyMap.log.Add(new LogMessage("Добавил параметр '" + Parametres.Last() + "'", elem, _elem));
            }
            else
                MessageBox.Show("Такой параметр уже существует");
        }

        public bool Delete(int id, string name, ref Groups groups)
        {
            bool isUsed = false;
            for (int i = 0; i < groups.GroupsOfNE.Count; i++)
            {
                if (groups.GroupsOfNE[i].Parametres.Contains(name))
                {
                    isUsed = true;
                    break;
                }
            }
            if (isUsed)
            {
                MessageBox.Show("Невозможно удалить параметр, т.к. он используется");
                return false;
            }
            else
            {
                Element elem = new Element(13, id, MainForm.parametrs.Parametres[id], -3);
                Element _elem = new Element(13, id, "", -3);
                MainForm.MyMap.log.Add(new LogMessage("Удалил параметр '" + MainForm.parametrs.Parametres[id] + "'", elem, _elem));
                Parametres.RemoveAt(id);
                return true;
            }
        }

        public void Edit(int id, string oldname, string newname, ref Groups groups, ref GroupOfNE NE, bool toLog)
        {
            if (!Parametres.Contains(newname))
            {
                for (int i = 0; i < groups.GroupsOfNE.Count; i++)
                {
                    for (int j = 0; j < groups.GroupsOfNE[i].Parametres.Count; j++)
                    {
                        if (groups.GroupsOfNE[i].Parametres[j] == oldname)
                            groups.GroupsOfNE[i].Parametres[j] = newname;
                    }
                }
                for (int i = 0; i < NE.NetworkElements.Count; i++)
                {
                    if (!NE.NetworkElements[i].delete)
                    {
                        for (int j = 0; j < NE.NetworkElements[i].Options.Options.Count; j++)
                            if (NE.NetworkElements[i].Options.Options[j].Name == oldname)
                                NE.NetworkElements[i].Options.Options[j].Name = newname;
                    }
                }
                if (toLog)
                {
                    Element elem = new Element(13, id, oldname, -2);
                    Element _elem = new Element(13, id, newname, -2);
                    MainForm.MyMap.log.Add(new LogMessage("Изменил параметр с '" + oldname + "' на '" + newname + "'", elem, _elem));
                }
                Parametres[id] = newname;
            }
            else
                MessageBox.Show("Такой параметр уже добавлен");
        }
        /// <summary>
        /// Сохранение списка параметров
        /// </summary>
        /// <param name="_params">Список параметров</param>
        static public void Save(GroupOfParametres _params)
        {
            if (_params.Parametres.Count <= 0)
                _params.Parametres = null;
            XmlSerializer formatter = new XmlSerializer(typeof(GroupOfParametres));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NetworkSettings", FileMode.Create))
            {
                formatter.Serialize(fs, _params);
            }
        }
        /// <summary>
        /// Загрузить параметры
        /// </summary>
        /// <returns>Возвращает список параментров</returns>
        static public GroupOfParametres Open()
        {
            GroupOfParametres parametrs = new GroupOfParametres();
            parametrs.Parametres.Add("MAC");
            parametrs.Parametres.Add("DNS");
            parametrs.Parametres.Add("Маска подсети");
            parametrs.Parametres.Add("Основной шлюз");
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
            XmlSerializer formatter = new XmlSerializer(typeof(GroupOfParametres));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NetworkSettings", FileMode.Open))
            {
                return (GroupOfParametres)formatter.Deserialize(fs);
            }
        }
        /// <summary>
        /// Загрузка параметров при загрузке карты сети
        /// </summary>
        static public void _Open()
        {
            GroupOfParametres parametrs = new GroupOfParametres();
            parametrs.Parametres.Add("MAC");
            parametrs.Parametres.Add("DNS");
            parametrs.Parametres.Add("Маска подсети");
            parametrs.Parametres.Add("Основной шлюз");
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
            XmlSerializer formatter = new XmlSerializer(typeof(GroupOfParametres));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings", FileMode.Open))
            {
                GroupOfParametres _parametrs = (GroupOfParametres)formatter.Deserialize(fs);
                GroupOfParametres __parametres = (GroupOfParametres)MainForm.parametrs.Clone();
                //MainForm.parametrs = new Parametrs();
                MainForm.parametrs = (GroupOfParametres)_parametrs.Clone();
                int id = -1;
                for (int i = 0; i < __parametres.Parametres.Count; i++)
                {
                    id = -1;
                    for (int j = 0; j < MainForm.parametrs.Parametres.Count; j++)
                    {
                        if (MainForm.parametrs.Parametres[j] == __parametres.Parametres[i])
                        {
                            id = i;
                            break;
                        }
                    }
                    if (id != -1)
                    {
                        __parametres.Parametres.RemoveAt(id);
                        i--;
                    }
                    else
                        MainForm.parametrs.Add(__parametres.Parametres[i]);
                }
            }
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<string> _params = new List<string>();
            foreach (var p in Parametres)
            {
                _params.Add(p);
            }
            return new GroupOfParametres
            {
                Parametres = _params,
            };
        }
        /// <summary>
        /// Загрузка параметров при импорте здания
        /// </summary>
        /// <param name="NE">Группа сетевых элементов</param>
        internal static void _OpenFromBuild(ref GroupOfNE NE)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(GroupOfParametres));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings", FileMode.Open))
            {
                GroupOfParametres _parametrs = (GroupOfParametres)formatter.Deserialize(fs);
                for (int i = 0; i < NE.NetworkElements.Count; i++)
                {
                    //MainForm.groups.GroupsOfNE.Contains(NE.NetworkElements[i].groupname)
                    /*
                    for (int opt = 0; opt < NE.NetworkElements[i].Options.Options.Count; opt++)
                    {
                        bool isLoadParam = false;
                        for (int j = 0; j < MainForm.parametrs.Parametres.Count; j++)
                        {
                            if (NE.NetworkElements[i].Options.Options[opt].Name == MainForm.parametrs.Parametres[j])
                            {
                                //NE.NetworkElements[i].Options.Options[opt].ID = j;
                                isLoadParam = true;
                                break;
                            }
                        }
                        if (!isLoadParam)
                        {
                            MainForm.parametrs.Add(NE.NetworkElements[i].Options.Options[opt].Name);
                            //NE.NetworkElements[i].Options.Options[opt].ID = MainForm.parametrs.Params.Count - 1;
                            int lastindex = MainForm.parametrs.Parametres.Count - 1;
                            Element elem = new Element(13, lastindex, "", -1);
                            Element _elem = new Element(13, lastindex, MainForm.parametrs.Parametres[lastindex], -1);
                            MainForm.MyMap.log.Add(new LogMessage("Добавил параметр", elem, _elem));
                        }*/
                   // }
                }
            }
        }
    }
}
