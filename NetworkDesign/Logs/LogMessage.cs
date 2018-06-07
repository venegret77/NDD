using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class LogMessage
    {
        public int userid;
        public string username;
        public DateTime dateTime = new DateTime();
        public string Text = "";
        public Element elem;
        public Element _elem;
        public int buildid;

        public LogMessage()
        {

        }

        public LogMessage(string _Text, Element elem_, Element _elem_)
        {
            username = MainForm.user.DisplayName;
            userid = MainForm.user.GetHashCode();
            dateTime = DateTime.Now;
            Text = _Text;
            elem = elem_;
            _elem = _elem_;
        }

        public LogMessage(string _Text, Element elem_, Element _elem_, int _buildid)
        {
            username = MainForm.user.DisplayName;
            userid = MainForm.user.GetHashCode();
            dateTime = DateTime.Now;
            Text = _Text;
            elem = elem_;
            _elem = _elem_;
            buildid = _buildid;
        }

        /*public override string ToString()
        {
            return User + ": " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString() + ": " + Text;
        }*/
    }
}
