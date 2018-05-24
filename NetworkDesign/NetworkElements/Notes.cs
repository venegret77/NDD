using NetworkDesign.Main;
using System;
using System.Collections.Generic;
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
        public User user;

        public Note()
        {
        }

        public Note(string note, User user)
        {
            this.note = note;
            this.user = user;
        }

        public object Clone()
        {
            return new Note
            {
                note = this.note,
                user = (User)this.user.Clone()
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

        public void Edit(int id, string text)
        {
            if (MainForm.user == notes[id].user | MainForm.user.accessLevel.EditAccessLevel)
                notes[id].note = text;
        }

        public void Remove(int id)
        {
            if (MainForm.user == notes[id].user | MainForm.user.accessLevel.EditAccessLevel)
                notes.RemoveAt(id);
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
