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

        public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

        public Registration_Form()
        {
            InitializeComponent();
            Settings s = new Settings("bruh", "bruh", this);

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
        }

        //Событие закрытия формы
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
                        //Проверка номера в журнале
                        if (Convert.ToInt32(numbes_box.Text) <= 30 && Convert.ToInt32(numbes_box.Text) > 0)
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
                                m_sqlCmd.CommandText = "INSERT INTO Users (user_login, user_password, user_email, user_name, user_surname, user_midname, user_group, user_journum, user_access) values ('" + login_box.Text + "','" + password_box.Text + "','" + email_box.Text + "','" + name_box.Text + "','" + sourname_box.Text + "','" + thirdname_box.Text + "','" + group_box.Text + "','" + numbes_box.Text + "','" + role + "')";
                                m_sqlCmd.ExecuteNonQuery();
                            }

                            //Окно об успешном создании записи
                            MaterialMessageBox.Show("Создание записи прошло успешно!\nТеперь вы пользователь системы.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }

                        else
                        {
                            MaterialMessageBox.Show("Номер в журнале не может превышать 30.\nПовторите попытку.", "Ошибка ввода номера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            
                        }
                        
                    }
                    else
                    {
                        //Проверка на краткость вводимых данных
                        MaterialMessageBox.Show("Логин или пароль должны быть длинее 4-х символов.", "Краткость логина или пароля", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        
                    }
                }

                else
                {
                    //Проверка полей
                    MaterialMessageBox.Show("При регистрации все поля должны быть заполнены.\nЗаполните все поля и повторите попытку", "Введены не все значения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                }

        }
            //Предосторожность, чтобы систему точно не сломали
            catch
            {
                MaterialMessageBox.Show("Значение поля \"№ в журнале\" принимает может принимать только цифры.\nЗаполните поле и повторите попытку.", "Введены не все значения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }

        }

        private void group_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') e.Handled = true;
        }

        private void email_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') e.Handled = true;
        }

        private void password_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') e.Handled = true;
        }

        private void login_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') e.Handled = true;
        }

        private void sourname_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') e.Handled = true;
        }
    }
}
