using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.NetworkElements
{
    public class Note : ICloneable
    {
        public string note;
        /// <summary>
        /// Пользователь
        /// </summary>
        public int userid;

        public Note()
        {
        }

        public Note(string note, UserPrincipal user)
        {
            this.note = note;
            this.userid = user.GetHashCode();
        }

        public override string ToString()
        {
            return note;
        }

        public object Clone()
        {
            return new Note
            {
                note = this.note,
                userid = this.userid
            };
        }
    }

    /// <summary>
    /// Заметки об элементе
    /// </summary>
    public class Notes
    {
        /// <summary>
        /// Список заметок
        /// </summary>
        public List<Note> notes = new List<Note>();

        public void Add(string text)
        {
            notes.Add(new Note(text, MainForm.user));
        }

        public void Add(Note note)
        {
            notes.Add(note);
        }

        public bool Edit(int id, string text)
        {
            if (MainForm.user.GetHashCode() == notes[id].userid | MainForm.edit)
            {
                notes[id].note = text;
                return true;
            }
            return false;
        }

        public bool Remove(int id)
        {
            if (MainForm.user.GetHashCode() == notes[id].userid | MainForm.edit)
            {
                notes.RemoveAt(id);
                return true;
            }
            return false;
        }

        internal Notes Copy()
        {
            List<Note> _notes = new List<Note>();
            foreach (var note in notes)
                _notes.Add((Note)note.Clone());
            return new Notes
            {
                notes = _notes
            };
        }
    }
}
