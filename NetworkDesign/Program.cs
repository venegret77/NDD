using NetworkDesign.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (CheckLib())
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new StartForm());
            }
            else
            {
                MessageBox.Show("Запуск невозможен, т.к. не найдена одна или несколько библиотек. Проверьте файл readme.txt");
            }
        }

        static private bool CheckLib()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Lib"))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\DevIL.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\freeglut.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Microsoft.CSharp.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.DevIl.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.DevIl.xml")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.FreeGlut.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.FreeGlut.xml")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.OpenGl.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.OpenGl.xml")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.Platform.Windows.dll")))
                return false;
            if (!File.Exists((Application.StartupPath + @"\Lib\Tao.Platform.Windows.xml")))
                return false;
            return true;
        }
    }
}
