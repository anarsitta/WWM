using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using MaterialSkin;
using MaterialSkin.Controls;

namespace WWM
{
    public partial class Authorization_Form : MaterialForm
    {
        //Глобальные переменные БД
        public String dbFileName = "MainDataBase.sqlite";
        public SQLiteCommand m_sqlCmd = new SQLiteCommand();
        public SQLiteDataReader account;

        //Переменная темы
        public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

        public Authorization_Form()
        {
            InitializeComponent();

            //Установка значений темы
            #region Установка_темы
            Settings s = new Settings("bruh", "bruh", "0", this);

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            if (s.themeToggle.Checked)
            {
                ThemeManager.Theme = MaterialSkinManager.Themes.DARK;
            }
            else
            {
                ThemeManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }

            if (s.materialRadioButton1.Checked == true)
            {
                ThemeManager.ColorScheme = new ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green400, TextShade.WHITE);
            }
            else if (s.materialRadioButton2.Checked == true)
            {
                ThemeManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue500, Accent.Blue400, TextShade.WHITE);
            }
            else if (s.materialRadioButton3.Checked == true)
            {
                ThemeManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber500, Accent.Amber400, TextShade.WHITE);
            }
            #endregion
        }

        //Прогрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            //Создание курсора и обращение к БД
            using (var m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"))
            {
                //Создание БД в случае ее отсутствия
                if (!File.Exists(dbFileName))
                {
                    SQLiteConnection.CreateFile(dbFileName);
                }

                //Определение базы данных
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                //Создание таблицы
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Users (user_id INTEGER PRIMARY KEY AUTOINCREMENT, user_login TEXT NOT NULL, user_password TEXT NOT NULL, user_email TEXT NOT NULL, user_name TEXT NOT NULL, user_surname TEXT NOT NULL, user_midname TEXT NOT NULL, user_group TEXT NOT NULL, user_journum INTEGER, user_access TEXT NOT NULL DEFAULT ('Пользователь'), UNIQUE (\"user_login\") ON CONFLICT IGNORE)";
                m_sqlCmd.ExecuteNonQuery();
            }
        }

        //Кнопка входа в систему
        public void EnterButton_Click(object sender, EventArgs e)
        {
            //Вывод ошибки в случае отсутствия записи
            try
            {
                //Проверка количества символов
                if (materialTextBox2.Text.Length > 4 && materialTextBox1.Text.Length > 4)
                {
                    //Создание курсора и обращение к БД
                    using (var m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"))
                    {
                        m_dbConn.Open();
                        m_sqlCmd.Connection = m_dbConn;

                        SQLiteCommand command = new SQLiteCommand(@"SELECT user_id, user_login, user_email, user_name, user_surname, user_midname, user_group, user_journum, user_access FROM Users WHERE user_login = '" + materialTextBox2.Text + "' AND user_password = '" + materialTextBox1.Text + "';", m_dbConn);
                        account = command.ExecuteReader();

                        account.Read();
                        
                        //Проверка прав аккаунта
                        if (Convert.ToString(account[8]) == "Администратор")
                        {
                            //Открытие формы администратора
                            AdminPanel ap = new AdminPanel(account);
                            ap.Show();
                            Hide();
                            account.Close();
                        }
                        else
                        {
                            //Открытие формы пользователя
                            UserPanel up = new UserPanel(account);
                            up.Show();
                            Hide();
                            account.Close();
                        }
                    }
                }

                //Вывод ошибки количества символов
                else
                {
                    MaterialMessageBox.Show("Логин или пароль должны быть больше 4-х символов.\nВведите другие данные и повторите попытку.", "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Вывод ошибки отсутствия записи
            catch
            {
                MaterialMessageBox.Show("Введённые вами данные отсутствуют.\nВведите другие данные и повторите попытку.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Открытие формы регистрации
        private void RegistrationButton_Click(object sender, EventArgs e)
        {
            Hide();
            Registration_Form reg_f = new Registration_Form();
            reg_f.Show();
        }

        //Событие закрытия формы
        private void Authorization_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
