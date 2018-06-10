using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    public class ListOfTextures: ICloneable
    {
        public int Count;
        public List<URL_ID> Textures = new List<URL_ID>();

        public ListOfTextures()
        {
        }

        public void Add(string url, int id)
        {
            Textures.Add(new URL_ID(url, id));
            Count++;
        }

        public void Add(URL_ID url_id)
        {
            Textures.Add(url_id);
            Count++;
        }

        public object Clone()
        {
            List<URL_ID> _Textures = new List<URL_ID>();
            foreach (var t in Textures)
                _Textures.Add(t);
            return new ListOfTextures
            {
                Count = this.Count,
                Textures = _Textures
            };
        }

        internal void Clear()
        {
            Textures = new List<URL_ID>();
            Count = 0;
        }

        internal void RemoveAt(int id)
        {
            Textures.RemoveAt(id);
            Count--;
        }

        internal int IndexOf(string v)
        {
            for (int i = 0; i < Textures.Count; i++)
            {
                if (Textures[i].URL == v)
                    return i;
            }
            return -1;
        }
    }

    public struct URL_ID
    {
        public string URL;
        public int ID;
        public string name;
        public string description;

        public URL_ID(string uRL, int iD) : this()
        {
            URL = uRL;
            ID = iD;
        }

        public URL_ID(string uRL, int iD, string name, string description)
        {
            this.URL = uRL;
            this.ID = iD;
            this.name = name;
            this.description = description;
        }
    }
}
