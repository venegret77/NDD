using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    /// <summary>
    /// Сообщение лога
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string userlogin;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string username;
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime dateTime = new DateTime();
        /// <summary>
        /// Текст
        /// </summary>
        public string Text = "";
        /// <summary>
        /// Элемент 1
        /// </summary>
        public Element elem;
        /// <summary>
        /// Элемент 2
        /// </summary>
        public Element _elem;
        /// <summary>
        /// Идентификатор здания
        /// </summary>
        public int buildid;
        /// <summary>
        /// Конструктор
        /// </summary>
        public LogMessage()
        {

        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_Text">Текст</param>
        /// <param name="elem_">Элемент 1</param>
        /// <param name="_elem_">Элемент 2</param>
        public LogMessage(string _Text, Element elem_, Element _elem_)
        {
            string name = "";
            string login = "";
            if (MainForm.user == null)
            {
                name = MainForm.usertemp;
                login = MainForm.usertemp;
            }
            else
            {
                name = MainForm.user.DisplayName;
                login = MainForm.user.SamAccountName;
            }
            username = name;
            userlogin = login;
            dateTime = DateTime.Now;
            Text = _Text;
            elem = elem_;
            _elem = _elem_;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_Text">Текст</param>
        /// <param name="elem_">Элемент 1</param>
        /// <param name="_elem_">Элемент 2</param>
        /// <param name="_buildid">Идентификатор здания</param>
        public LogMessage(string _Text, Element elem_, Element _elem_, int _buildid)
        {
            string name = "";
            string login = "";
            if (MainForm.user == null)
            {
                name = MainForm.usertemp;
                login = MainForm.usertemp;
            }
            else
            {
                name = MainForm.user.DisplayName;
                login = MainForm.user.SamAccountName;
            }
            username = name;
            userlogin = login;
            dateTime = DateTime.Now;
            Text = _Text;
            elem = elem_;
            _elem = _elem_;
            buildid = _buildid;
        }
    }
}
