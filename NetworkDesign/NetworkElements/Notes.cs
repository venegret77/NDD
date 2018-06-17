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
        /// <summary>
        /// Текст заметки
        /// </summary>
        public string note;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string userlogin;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Note()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="note">Текст заметки</param>
        /// <param name="user">Пользователь</param>
        public Note(string note, UserPrincipal user)
        {
            this.note = note;
            this.userlogin = user.SamAccountName;
        }

        public override string ToString()
        {
            return note;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            return new Note
            {
                note = this.note,
                userlogin = this.userlogin
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
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="text">Текст заметки</param>
        public void Add(string text)
        {
            notes.Add(new Note(text, MainForm.user));
        }
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="note">Заметка</param>
        public void Add(Note note)
        {
            notes.Add(note);
        }
        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        public bool Edit(int id, string text)
        {
            if (MainForm.user.SamAccountName == notes[id].userlogin | MainForm.edit)
            {
                notes[id].note = text;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            if (MainForm.user.SamAccountName == notes[id].userlogin | MainForm.edit)
            {
                notes.RemoveAt(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
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
