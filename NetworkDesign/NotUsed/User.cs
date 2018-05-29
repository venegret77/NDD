using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDesign.Main
{
    /// <summary>
    /// Пользователь
    /// </summary>
    /*public struct User : ICloneable
    {
        /// <summary>
        /// ФИО
        /// </summary>
        public string name;
        /// <summary>
        /// Логин
        /// </summary>
        public string login;
        /// <summary>
        /// Уровень доступа
        /// </summary>
        public AccessLevel accessLevel;

        public User(string name, string login) : this()
        {
            this.name = name;
            this.login = login;
        }

        public User(string name, string login, AccessLevel accessLevel)
        {
            this.name = name;
            this.login = login;
            this.accessLevel = accessLevel;
        }

        public static bool operator == (User u1, User u2)
        {
            if (u1.login == u2.login & u1.name == u2.name)
                return true;
            else
                return false;
        }

        public static bool operator !=(User u1, User u2)
        {
            if (u1.login == u2.login & u1.name == u2.name)
                return false;
            else
                return true;
        }

        public object Clone()
        {
            return new User
            {
                name = this.name,
                login = this.login,
                accessLevel = (AccessLevel)this.accessLevel.Clone()
            };
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// Уровень доступа
    /// </summary>
    public struct AccessLevel : ICloneable
    {
        /// <summary>
        /// Просмотр карты
        /// </summary>
        public bool ReadMap;
        /// <summary>
        /// Редактирование карты
        /// </summary>
        public bool EditMap;
        /// <summary>
        /// Редактирование прав доступа для пользователей
        /// </summary>
        public bool EditAccessLevel;

        public AccessLevel(bool readMap, bool editMap, bool editAccessLevel)
        {
            ReadMap = readMap;
            EditMap = editMap;
            EditAccessLevel = editAccessLevel;
        }

        public object Clone()
        {
            return new AccessLevel
            {
                ReadMap = this.ReadMap,
                EditMap = this.EditMap,
                EditAccessLevel = this.EditAccessLevel
            };
        }
    }*/
}
