﻿using System;
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
        public string Name = "";
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
        /// Обновление идентификаторов
        /// </summary>
        /// <param name="id">Идентификатор</param>
        internal void RefreshID(int id)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i].ID > id)
                    Options[i].SetNewID();
            }
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
                options.Add(new NetworkParametr(p.ID, p.Name, p.Value));
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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="iD">Идентификатор</param>
        /// <param name="name">Наименование</param>
        /// <param name="value">Значение</param>
        public NetworkParametr(int iD, string name, string value)
        {
            ID = iD;
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
        /// Установить новый идентификатор
        /// </summary>
        public void SetNewID()
        {
            ID--;
            Name = MainForm.parametrs.Params[ID];
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
    /// <summary>
    /// Общий набор параметров для сетевых элементов
    /// </summary>
    public class Parametrs : ICloneable
    {
        /// <summary>
        /// Список параметров в виде строк
        /// </summary>
        public List<string> Params = new List<string>();
        /// <summary>
        /// Конструктор
        /// </summary>
        public Parametrs()
        {
        }
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="_name">Наименование</param>
        public void Add(string _name)
        {
            Params.Add(_name);
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public void Remove(int id)
        {
            Params.RemoveAt(id);
        }
        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="_name">Новое наименование</param>
        public void Edit(int id, string _name)
        {
            Params[id] = _name;
        }
        /// <summary>
        /// Сохранение списка параметров
        /// </summary>
        /// <param name="_params">Список параметров</param>
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
        /// <summary>
        /// Загрузить параметры
        /// </summary>
        /// <returns>Возвращает список параментров</returns>
        static public Parametrs Open()
        {
            Parametrs parametrs = new Parametrs();
            //parametrs.Add("IP");
            //parametrs.Add("MAC");
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
        /// <summary>
        /// Загрузка параметров при загрузке карты сети
        /// </summary>
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
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<string> _params = new List<string>();
            foreach (var p in Params)
            {
                _params.Add(p);
            }
            return new Parametrs
            {
                Params = _params,
            };
        }
        /// <summary>
        /// Загрузка параметров при импорте здания
        /// </summary>
        /// <param name="NE">Группа сетевых элементов</param>
        internal static void _OpenFromBuild(ref GroupOfNE NE)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Parametrs));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings", FileMode.Open))
            {
                Parametrs _parametrs = (Parametrs)formatter.Deserialize(fs);
                for (int i = 0; i < NE.NetworkElements.Count; i++)
                {
                    for (int opt = 0; opt < NE.NetworkElements[i].Options.Options.Count; opt++)
                    {
                        bool isLoadParam = false;
                        for (int j = 0; j < MainForm.parametrs.Params.Count; j++)
                        {
                            if (NE.NetworkElements[i].Options.Options[opt].Name == MainForm.parametrs.Params[j])
                            {
                                NE.NetworkElements[i].Options.Options[opt].ID = j;
                                isLoadParam = true;
                                break;
                            }
                        }
                        if (!isLoadParam)
                        {
                            MainForm.parametrs.Add(NE.NetworkElements[i].Options.Options[opt].Name);
                            NE.NetworkElements[i].Options.Options[opt].ID = MainForm.parametrs.Params.Count - 1;
                            int lastindex = MainForm.parametrs.Params.Count - 1;
                            Element elem = new Element(13, lastindex, "", -1);
                            Element _elem = new Element(13, lastindex, MainForm.parametrs.Params[lastindex], -1);
                            MainForm.MyMap.log.Add(new LogMessage("Добавил параметр", elem, _elem));
                        }
                    }
                }
            }
        }
    }
}
