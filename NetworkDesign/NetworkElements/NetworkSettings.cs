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
        List<NetworkParametr> Options = new List<NetworkParametr>();

        public NetworkSettings()
        {
        }

        public NetworkSettings(List<NetworkParametr> _Options)
        {
            Options = _Options;
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
        /// Наименование
        /// </summary>
        public string Name;
        /// <summary>
        /// Значение
        /// </summary>
        public string Value;

        public override string ToString()
        {
            return Name + ": " + Value; 
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
            XmlSerializer formatter = new XmlSerializer(typeof(Parametrs));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NetworkSettings", FileMode.Open))
            {
                return (Parametrs)formatter.Deserialize(fs);
            }
        }
    }
}
