using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Data.SQLite;

namespace WWM
{
    public partial class Registration_Form : MaterialForm
    {
        public SQLiteConnection m_dbConn = new SQLiteConnection();
        public SQLiteCommand m_sqlCmd = new SQLiteCommand();

        public Registration_Form()
        {
            InitializeComponent();
        }

        private void Registration_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Authorization_Form auth_f = new Authorization_Form();
            auth_f.Show();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            //Ссылка на класс 2 формы для доступа к компонентам и переменным
            Authorization_Form auth = new Authorization_Form();
            string role = "Пользователь";
            //Отлов ошибок
            try
            {
                //Проверка вводимых данных
                if (login_box.Text.Length > 0 && password_box.Text.Length > 0 && email_box.Text.Length > 0 && name_box.Text.Length > 0 && sourname_box.Text.Length > 0 && thirdname_box.Text.Length > 0 && group_box.Text.Length > 0 && numbes_box.Text.Length > 0)
                {
                    //Проверка основных данных
                    if (login_box.Text.Length > 4 && password_box.Text.Length > 4)
                    {
                        //Создание курсора
                        using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
                        {

                            m_dbConn.Open();

                            //Определение базы данных
                            m_sqlCmd.Connection = m_dbConn;

                            //Проверка таблицы, чтобы задать роль администратора, если записи отсутствуют
                            DataTable dTable = new DataTable();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Users LIMIT 5", m_dbConn);
                            adapter.Fill(dTable);

                            //Процесс регистрации
                            m_sqlCmd.Connection = m_dbConn;

                            //Проверка на первую запись в таблице
                            if (dTable.Rows.Count == 0)
                            {
                                role = "Администратор";
                                MessageBox.Show("Вы являетесь первым пользователем системы.\nВам присвоена роль - 'Администратор'.", "Первая регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            //Создание записи
                            m_sqlCmd.CommandText = "INSERT INTO Users (user_login, user_password, user_email, user_name, user_surname, user_midname, user_group, user_journum, user_access) values ('" + login_box.Text + "','" + password_box.Text + "','" + email_box.Text + "','" + name_box.Text + "','" + sourname_box.Text + "','" + thirdname_box.Text + "','" + group_box.Text + "','" + Convert.ToInt32(numbes_box.Text) + "','" + role + "')";
                            m_sqlCmd.ExecuteNonQuery();
                        }

                        //Окно об успешном создании записи
                        MessageBox.Show("Создание записи прошло успешно!\nТеперь вы пользователь системы.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    else
                    {
                        //Проверка на краткость вводимых данных
                        MessageBox.Show("Логин или пароль должны быть длинее 4-х символов.", "Краткость логина или пароля", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                else
                {
                    //Проверка полей
                    MessageBox.Show("При регистрации все поля должны быть заполнены.\nЗаполните все поля и повторите попытку", "Введены не все значения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            //Предосторожность, чтобы систему точно не сломали
            catch
            {
                MessageBox.Show("При регистрации все поля должны быть заполнены.\nЗаполните все поля и повторите попытку", "Введены не все значения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}
