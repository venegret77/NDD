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
    /// <summary>
    /// Класс для хранения кнопок
    /// </summary>
    public class ID_TEXT
    {
        /// <summary>
        /// Список идентификаторов
        /// </summary>
        public List<int> ID = new List<int>();
        /// <summary>
        /// Список текстур
        /// </summary>
        public List<string> TEXT = new List<string>();
        /// <summary>
        /// Конструктор
        /// </summary>
        public ID_TEXT()
        {
        }
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="text">Текстура</param>
        public void ADD(int id, string text)
        {
            ID.Add(id);
            TEXT.Add(text);
        }
    }

    /// <summary>
    /// Кнопка для панели элементов
    /// </summary>
    public class NEButton
    {
        /// <summary>
        /// Кнопка
        /// </summary>
        public ToolStripButton toolStripButton;
        /// <summary>
        /// Идентификатор текстуры
        /// </summary>
        public int id;
        /// <summary>
        /// Наименование текстуры
        /// </summary>
        public string textname;
        /// <summary>
        /// Идентификатор кнопки
        /// </summary>
        public int tsid;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="toolStripButton">Кнопка</param>
        /// <param name="id">Идентификатор текстуры</param>
        /// <param name="textname">Наименование текстуры</param>
        /// <param name="tsid">Идентификатор кнопки</param>
        public NEButton(ToolStripButton toolStripButton, int id, string textname, int tsid)
        {
            this.toolStripButton = toolStripButton;
            this.id = id;
            this.textname = textname;
            this.tsid = tsid;
            toolStripButton.Click += ToolStripButton_Click;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public NEButton()
        {
        }
        /// <summary>
        /// Обработчик события клика по кнопке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ToolStripButton_Click(object sender, EventArgs e) => MainForm.MyMap.SetInstrument(MainForm.nebutnscount + id);
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="_cs">Кнопки</param>
        static public void Save(ID_TEXT _cs)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ID_TEXT));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Configurations\NEButtons", FileMode.Create))
            {
                formatter.Serialize(fs, _cs);
            }
        }
        /// <summary>
        /// Загрузка
        /// </summary>
        /// <returns>Возвращает кнопки для панели элементов</returns>
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
    }
}
