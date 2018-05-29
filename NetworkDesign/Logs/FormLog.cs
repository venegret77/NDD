using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkDesign
{
    public partial class FormLog : Form
    {
        public FormLog(List<LogMessage> log)
        {
            InitializeComponent();

            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Пользователь"; //текст в шапке
            column1.ReadOnly = true; //значение в этой колонке нельзя править
            column1.Name = "name"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Дата";
            column2.Name = "data";
            column2.ReadOnly = true; //значение в этой колонке нельзя править.
            column2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column2.CellTemplate = new DataGridViewTextBoxCell();

            var column3 = new DataGridViewColumn();
            column3.HeaderText = "Сообщение";
            column3.Name = "message";
            column3.ReadOnly = true; //значение в этой колонке нельзя править.
            column3.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column3.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки

            foreach (var mess in log)
            {
                dataGridView1.Rows.Add(mess.username,mess.dateTime,mess.Text);
            }
            StartPosition = FormStartPosition.CenterParent;
        }

        private void FormLog_Load(object sender, EventArgs e)
        {

        }
    }
}
