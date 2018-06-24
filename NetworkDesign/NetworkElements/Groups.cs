using NetworkDesign;
using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NetworkDesign.NetworkElements
{
    public class Groups
    {
        public List<Group> GroupsOfNE = new List<Group>();

        public void Add(Group g)
        {
            string empty = "";
            GroupsOfNE.Add(g);
            Element _elem = new Element(17, GroupsOfNE.Count - 1, empty, GroupsOfNE[GroupsOfNE.Count - 1], -1);
            Element elem = new Element(17, GroupsOfNE.Count - 1, empty, -1);
            MainForm.MyMap.log.Add(new LogMessage("Добавил группу элементов '" + g.name + "'", elem, _elem));
        }

        public void AddTemp(Group g)
        {
            GroupsOfNE.Add(g);
        }

        public bool Delete(int id, ref ListOfTextures NE)
        {
            bool isUsed = false;
            for (int i = 0; i < NE.Textures.Count; i++)
            {
                if (NE.Textures[i].Type == id)
                {
                    isUsed = true;
                    break;
                }
            }
            if (isUsed)
            {
                MessageBox.Show("Невозможно удалить группу, т.к. она используется");
                return false;
            }
            else
            {
                Element elem = new Element(17, id, "", GroupsOfNE[id], -3);
                Element _elem = new Element(17, id, "", -3);
                MainForm.MyMap.log.Add(new LogMessage("Удалил группу элементов '" + GroupsOfNE[id].name + "'", elem, _elem));
                NE.giddown(id);
                GroupsOfNE.RemoveAt(id);
                return true;
            }
        }

        public void Edit(int id, string name)
        {
            GroupsOfNE[id].Edit(name);
        }
        /// <summary>
        /// Сохранение списка параметров
        /// </summary>
        /// <param name="_params">Список параметров</param>
        static public void Save(Groups _params)
        {
            if (_params.GroupsOfNE.Count <= 0)
                _params.GroupsOfNE = null;
            XmlSerializer formatter = new XmlSerializer(typeof(Groups));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\Groups", FileMode.Create))
            {
                formatter.Serialize(fs, _params);
            }
        }
        /// <summary>
        /// Загрузить параметры
        /// </summary>
        /// <returns>Возвращает список параментров</returns>
        static public Groups Open()
        {
            Groups parametrs = new Groups();
            parametrs.AddTemp(new Group(new List<string> { "MAC" , "DNS", "Маска подсети", "Основной шлюз"}, "Персональные компьютеры", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Маршрутизаторы", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Сервера", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Сетевые принтеры", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string>(), "Хабы", false, false, true, true));
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(parametrs);
                return parametrs;
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\Groups"))
            {
                Save(parametrs);
                return parametrs;
            }
            XmlSerializer formatter = new XmlSerializer(typeof(Groups));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\Groups", FileMode.Open))
            {
                return (Groups)formatter.Deserialize(fs);
            }
        }
        /// <summary>
        /// Загрузка параметров при загрузке карты сети
        /// </summary>
        static public void _Open()
        {
            Groups parametrs = new Groups();
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Персональные компьютеры", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Маршрутизаторы", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Сервера", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string> { "MAC", "DNS", "Маска подсети", "Основной шлюз" }, "Сетевые принтеры", true, true, true, true));
            parametrs.AddTemp(new Group(new List<string>(), "Хабы", false, false, true, true));
            if (!Directory.Exists(Application.StartupPath + @"\Configurations"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Configurations");
                Save(parametrs);
                MainForm.groups = parametrs;
            }
            if (!File.Exists(Application.StartupPath + @"\Configurations\Groups"))
            {
                Save(parametrs);
                MainForm.groups = parametrs;
            }
            XmlSerializer formatter = new XmlSerializer(typeof(Groups));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\Groups", FileMode.Open))
            {
                Groups _parametrs = (Groups)formatter.Deserialize(fs);
                Groups __parametres = (Groups)MainForm.groups.Clone();
                //MainForm.parametrs = new Parametrs();
                MainForm.groups = (Groups)_parametrs.Clone();
                int id = -1;
                for (int i = 0; i < __parametres.GroupsOfNE.Count; i++)
                {
                    id = -1;
                    for (int j = 0; j < MainForm.groups.GroupsOfNE.Count; j++)
                    {
                        if (MainForm.groups.GroupsOfNE[j].name == __parametres.GroupsOfNE[i].name)
                        {
                            id = i;
                            break;
                        }
                    }
                    if (id != -1)
                    {
                        __parametres.GroupsOfNE.RemoveAt(id);
                        i--;
                    }
                    else
                        MainForm.groups.AddTemp(__parametres.GroupsOfNE[i]);
                }
            }
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<Group> _params = new List<Group>();
            foreach (var p in GroupsOfNE)
            {
                _params.Add(p);
            }
            return new Groups
            {
                GroupsOfNE = _params
            };
        }
        /// <summary>
        /// Загрузка параметров при импорте здания
        /// </summary>
        /// <param name="NE">Группа сетевых элементов</param>
        internal static void _OpenFromBuild(ref GroupOfNE NE)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Groups));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\Groups", FileMode.Open))
            {
                Groups groups = (Groups)formatter.Deserialize(fs);
                for (int i = 0; i < NE.NetworkElements.Count; i++)
                {
                    int id = -1;
                    for (int j = 0; j < MainForm.groups.GroupsOfNE.Count; j++)
                    {
                        if (MainForm.groups.GroupsOfNE[j].name == NE.NetworkElements[i].groupname)
                        {
                            id = j;
                            break;
                        }
                    }
                    if (id != -1)
                    {
                        var group = groups.GroupsOfNE[NE.NetworkElements[i].type];
                        if (group.isHostName)
                            MainForm.groups.GroupsOfNE[id].isHostName = true;
                        if (group.isTrouthp)
                            MainForm.groups.GroupsOfNE[id].isTrouthp = true;
                        if (group.isIP)
                            MainForm.groups.GroupsOfNE[id].isIP = true;
                        if (group.isPorts)
                            MainForm.groups.GroupsOfNE[id].isPorts = true;
                        foreach (var p in group.Parametres)
                        {
                            if (!MainForm.groups.GroupsOfNE[id].Parametres.Contains(p))
                                MainForm.parametrs.Add(p);
                        }
                        NE.NetworkElements[i].type = id;
                        NE.NetworkElements[i].groupname = MainForm.groups.GroupsOfNE[id].name;
                    }
                    else
                    {
                        for (int j = 0; j < groups.GroupsOfNE.Count; j++)
                        {
                            if (groups.GroupsOfNE[j].name == NE.NetworkElements[i].groupname)
                            {
                                id = j;
                                break;
                            }
                        }
                        MainForm.groups.Add(groups.GroupsOfNE[id]);
                        NE.NetworkElements[i].groupname = MainForm.groups.GroupsOfNE[MainForm.groups.GroupsOfNE.Count - 1].name;
                        NE.NetworkElements[i].type = MainForm.groups.GroupsOfNE.Count - 1;
                    }
                }
            }
            for (int i = 0; i < MainForm.ImagesURL.Textures.Count; i++)
            {
                for (int j = 0; j < MainForm.groups.GroupsOfNE.Count; j++)
                    if (MainForm.ImagesURL.Textures[i].namegroup == MainForm.groups.GroupsOfNE[j].name)
                        MainForm.ImagesURL.Textures[i].Type = j;
            }
        }
    }

    public class Group: ICloneable
    {
        public bool isHostName;
        public bool isIP;
        public bool isPorts;
        public bool isTrouthp;
        public List<string> Parametres;
        public string name;

        public Group()
        {
        }

        public Group(List<string> Parametres, string name, bool isHostName, bool isIP, bool isPorts, bool isTrouthp)
        {
            this.name = name;
            this.isPorts = isPorts;
            this.isIP = isIP;
            this.isTrouthp = isTrouthp;
            this.isHostName = isHostName;
            this.Parametres = new List<string>();
            foreach (var p in Parametres)
            {
                this.Parametres.Add(p);
            }
        }

        public object Clone()
        {
            List<string> param = new List<string>();
            foreach (var p in Parametres)
                param.Add(p);
            return new Group
            {
                name = this.name,
                isHostName = this.isHostName,
                isIP = this.isIP,
                isPorts = this.isPorts,
                isTrouthp = this.isTrouthp,
                Parametres = param
            };
        }

        internal void Edit(string name)
        {
            this.name = name;
        }
    }
}
	/*

        private void DeleteGroupFromLog(int id, ref List<URL_ID> NE)
        {
            for(int i = 0; i < NE.Count; i++)
			{
				if (NE[i].Type > id)
					NE[i].Type--;
			}
			GroupsOfNE.RemoveAt(id);
            CheckButtons(false);
        }

        private void AddGroupFromLog(int id, Group group)
        {
            Groups.Insert(id, group);
            foreach (var item in ImagesURL)
            {
                if (item.Type >= id)
                    item.Type++;
            }
            CheckButtons(false);
        }*/