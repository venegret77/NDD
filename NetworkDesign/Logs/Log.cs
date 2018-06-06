using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign
{
    public class Log
    {
        public List<LogMessage> Back = new List<LogMessage>();
        public List<LogMessage> Forward = new List<LogMessage>();

        public Log()
        {

        }

        public void Add(LogMessage logMessage)
        {
            if (logMessage._elem.type == 13 & logMessage._elem.transform == -4)
            {
                LogMessage lastelem = Back.Last();
                if (lastelem._elem.transform == -4 & lastelem._elem.type == logMessage._elem.type & lastelem._elem.index == logMessage._elem.index)
                {
                    logMessage._elem = (Element)lastelem._elem.Clone();
                    Back.RemoveAt(Back.Count - 1);
                }
            }
            else if (logMessage._elem.transform == -2 & logMessage._elem.type != 4 & logMessage._elem.type != 13)
            {
                LogMessage lastelem = Back.Last();
                if (lastelem._elem.transform == -2 & lastelem._elem.type == logMessage._elem.type & lastelem._elem.index == logMessage._elem.index)
                {
                    logMessage._elem = (Element)lastelem._elem.Clone();
                    Back.RemoveAt(Back.Count - 1);
                }
            }
            Back.Add(logMessage);
        }

        /*public void CheckIWandEnt(int id, Building build)
        {
            foreach (var elem in Back)
            {
                if (elem.buildid == id)
                {
                    int id1 = elem.elem.index;
                    int id2 = elem._elem.index;
                }
            }
        }*/

        public Element DeleteLastBack(out Element el)
        {
            LogMessage temp = Back[Back.Count - 1];
            Back.RemoveAt(Back.Count - 1);
            Forward.Add(temp);
            el = temp._elem;
            return temp.elem;
        }

        public Element DeleteLastForward(out Element el)
        {
            LogMessage temp = Forward[Forward.Count - 1];
            Forward.RemoveAt(Forward.Count - 1);
            Back.Add(temp);
            el = temp.elem;
            return temp._elem;
        }

        public Element DeleteLastBack(out Element el, out int buildid)
        {
            LogMessage temp = Back[Back.Count - 1];
            Back.RemoveAt(Back.Count - 1);
            Forward.Add(temp);
            el = temp._elem;
            buildid = temp.buildid;
            return temp.elem;
        }

        public Element DeleteLastForward(out Element el, out int buildid)
        {
            LogMessage temp = Forward[Forward.Count - 1];
            Forward.RemoveAt(Forward.Count - 1);
            Back.Add(temp);
            el = temp.elem;
            buildid = temp.buildid;
            return temp._elem;
        }

        public void ClearForward()
        {
            Forward.Clear();
        }

        public bool NotNullBack()
        {
            if (Back.Count != 0)
                return true;
            else
                return false;
        }

        public bool NotNullForward()
        {
            if (Forward.Count != 0)
                return true;
            else
                return false;
        }

    }
}
