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
    public partial class AdminPanel : MaterialForm
    {
        //Переменные БД
        public String sqlQuery;
        Authorization_Form auth = new Authorization_Form();
        public System.Data.SQLite.SQLiteDataReader acc;

        //Переменные для разделения объекта (напиши класс ленивый)
        public string user_id;
        public string user_login;
        public string user_email;
        public string user_name;
        public string user_surname;
        public string user_midname;
        public string user_group;
        public string user_access;

        //Переменная для темы
        public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

        //Конструктор формы
        public AdminPanel(System.Data.SQLite.SQLiteDataReader account)
        {
            //Да, да, да классы....
            user_id = Convert.ToString(account[0]);
            user_login = (string)account[1];
            user_email = (string)account[2];
            user_name = (string)account[3];
            user_surname = (string)account[4];
            user_midname = (string)account[5];
            user_group = (string)account[6];
            user_access = (string)account[8];

            InitializeComponent();

            //Установка значений темы
            #region Установка_темы
            Settings s = new Settings("bruh", "bruh", user_id, this);

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

            //Установка шрифтов для датагрида
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        //Загрузка формы
        private void AdminPanel_Load(object sender, EventArgs e)
        {
            this.Text = "WWM/" + user_name + " " + user_midname;

            materialButton4_Click(sender, e);
        }

        //Обработка выхода
        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        //Обработка выхода
        private void materialButton5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Отслеживание смены в комбобоксе для отображения нужного текст бокса
        private void materialComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Проверка
            if (materialComboBox2.SelectedIndex == 3)
            {
                //Заполнение комбо бокса
                using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
                {
                    groupTextBox.Items.Clear();
                    SQLiteCommand command = new SQLiteCommand("SELECT DISTINCT user_group FROM Users", m_dbConn);
                    m_dbConn.Open();
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        groupTextBox.Items.Add((string)reader["user_group"]);
                    reader.Close();
                }

                //Установка первого значения группы
                groupTextBox.SelectedIndex = 0;

                //Отображение компонентов
                groupTextBox.Visible = true;
                name_famTextBox.Visible = false;
                numberTextBox.Visible = false;

            }
            else if (materialComboBox2.SelectedIndex == 4)
            {
                //Отображение компонентов
                groupTextBox.Visible = false;
                numberTextBox.Visible = true;
                name_famTextBox.Visible = false;
            }
            else
            {
                //Отображение компонентов
                groupTextBox.Visible = false;
                numberTextBox.Visible = false;
                name_famTextBox.Visible = true;
            }
        }

        //Запред ввод символа
        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        //Поиск
        private void materialButton4_Click(object sender, EventArgs e)
        {
            //Переменные таблицы
            DataTable dTable = new DataTable();
            DataTable combBox = new DataTable();

            //Формирование запроса
            #region Формирование_запроса
            switch (materialComboBox1.SelectedIndex)
            {
                case 0: sqlQuery = "SELECT user_group, user_journum, user_name, user_surname, user_midname, user_login FROM Users "; break;
                case 1: sqlQuery = "DELETE FROM Users "; break;
            }

            switch (materialComboBox2.SelectedIndex)
            { 
                case 0: sqlQuery = sqlQuery + "WHERE user_name LIKE '%" + name_famTextBox.Text + "%' AND user_access = 'Пользователь' "; break;
                case 1: sqlQuery = sqlQuery + "WHERE user_surname LIKE '%" + name_famTextBox.Text + "%' AND user_access = 'Пользователь' "; break;
                case 2: sqlQuery = sqlQuery + "WHERE user_midname LIKE '%" + name_famTextBox.Text + "%' AND user_access = 'Пользователь' "; break;
                case 3: sqlQuery = sqlQuery + "WHERE user_group LIKE '" + groupTextBox.SelectedItem + "' AND user_access = 'Пользователь' ";  break;
                case 4: sqlQuery = sqlQuery + "WHERE user_journum LIKE '" + numberTextBox.Text + "' AND user_access = 'Пользователь' "; break;
                case 5: sqlQuery = sqlQuery + "WHERE user_login LIKE '" + name_famTextBox.Text + "' AND user_access = 'Пользователь' "; break;
            }

            switch (materialComboBox3.SelectedIndex)
            {
                case 0: break;
                case 1: sqlQuery = sqlQuery + "LIMIT 1"; break;
                case 2: sqlQuery = sqlQuery + "LIMIT 5"; break;
                case 3: sqlQuery = sqlQuery + "LIMIT 10"; break;
                case 4: sqlQuery = sqlQuery + "LIMIT 15"; break;
            }
            #endregion

            //Запрос в БД
            using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
            {
                //Простая выборка
                if (materialComboBox1.SelectedIndex == 0)
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter.Fill(dTable);
                    
                    //Обработка
                    if (dTable.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();

                        for (int i = 0; i < dTable.Rows.Count; i++)
                            dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                    //Ошибка
                    else
                    {
                        dataGridView1.Rows.Clear();
                        MaterialMessageBox.Show("Значения, задаваемого вами запроса отсутствуют.", "Значения отсутствуют", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                //Удаление записи
                else
                {
                    m_dbConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlQuery, m_dbConn);
                    cmd.ExecuteNonQuery();

                    sqlQuery = "SELECT user_group, user_journum, user_name, user_surname, user_midname, user_login FROM Users";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter.Fill(dTable);

                    if (dTable.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();

                        for (int i = 0; i < dTable.Rows.Count; i++)
                            dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                }
            }
        }

        //Отображение формы настройки
        private void EnterButton_Click(object sender, EventArgs e)
        {
            Settings set = new Settings(user_name, user_midname, user_id, this);
            set.Show();
            this.Hide();
        }
    }
}
