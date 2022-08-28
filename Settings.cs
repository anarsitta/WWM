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
    public partial class Settings : MaterialForm
    {
		//Переменные текста (пиши класс ленивый)
        public string user_name;
        public string user_midname;
		public string user_id1;

		//Переменные формы
		public MaterialForm form;
		public Authorization_Form af;

		//Переменная темы
		public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

		//Строка запроса
		public SQLiteCommand m_sqlCmd = new SQLiteCommand();

		//Конструктор формы
		public Settings(string name, string midname, string user_id, MaterialForm mf)
        {
			//Обозначение глобальных переменных
            user_name = name;
            user_midname = midname;
			user_id1 = user_id;

			//Установка значений темы
			#region Установка_темы
			InitializeComponent();

			var materialSkinManager = MaterialSkinManager.Instance;
			materialSkinManager.AddFormToManage(this);

			materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
			materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

			if (themeToggle.Checked)
			{
				ThemeManager.Theme = MaterialSkinManager.Themes.DARK;
			}
			else
			{
				ThemeManager.Theme = MaterialSkinManager.Themes.LIGHT;
			}

			if (materialRadioButton1.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green400, TextShade.WHITE);
			}
			else if (materialRadioButton2.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue500, Accent.Blue400, TextShade.WHITE);
			}
			else if (materialRadioButton3.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber500, Accent.Amber400, TextShade.WHITE);
			}
            #endregion

			//Установка глобального значения формы
            form = mf;
			
		}

		//Подгрузка формы
        private void Settings_Load(object sender, EventArgs e)
        {
            this.Text = "WWM/" + user_name + " " + user_midname + "/Настройки";
        }

		//Изменение темы в приложении
        private void themeToggle_CheckedChanged(object sender, EventArgs e)
        {
			//Проверка на нажатие
			if (themeToggle.Checked)
			{
				ThemeManager.Theme = MaterialSkinManager.Themes.DARK;
			}
			else
			{
				ThemeManager.Theme = MaterialSkinManager.Themes.LIGHT;
			}

		}

        //Изменение цвета
        #region Изменение_цвета
        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
			//Проверка на нажатие
			if (materialRadioButton1.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green400, TextShade.WHITE);
			}
			else
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
			}
		}

        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
			//Проверка на нажатие
			if (materialRadioButton2.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue500, Accent.Blue400, TextShade.WHITE);

			}
			else
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
			}
		}

        private void materialRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
			//Проверка на нажатие
			if (materialRadioButton3.Checked == true)
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber500, Accent.Amber400, TextShade.WHITE);

			}
			else
			{
				ThemeManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
			}
		}
        #endregion

		//Событие закрытия формы
        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
			//Определение с какой формы была открыта форма настроек и открытие нужной формы
			if(form.Name == "AdminPanel")
            {
				form.Refresh();
				form.Show();

            }
			else if(form.Name == "UserPanel")
			{
				form.Show();
            }
			Properties.Settings.Default.Save();
		}

		//Изменение логина
        private void materialButton3_Click(object sender, EventArgs e)
        {
			af = new Authorization_Form();
			try
            {
				if (materialTextBox1.Text.Length >= 4)
				{
					//Запрос в БД
					using (var m_dbConn = new SQLiteConnection("Data Source=" + af.dbFileName + ";Version=3;"))
					{
						m_dbConn.Open();

						m_sqlCmd.Connection = m_dbConn;

						SQLiteCommand command = new SQLiteCommand(@"UPDATE Users SET user_login = '" + materialTextBox1.Text + "' WHERE user_id = '" + user_id1 + "'", m_dbConn);
						command.ExecuteNonQuery();
					}

					MaterialMessageBox.Show("Логин успешно изменён.", "Изменение логина", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MaterialMessageBox.Show("Логин должен быть больше 4-х символов.", "Изменение логина", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch
            {
				MaterialMessageBox.Show("Возникла некоторая ошибка.\nЛогин не был изменён.", "Ошибка изменения логина", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}			
			
		}

		//Изменение пароля
        private void materialButton5_Click(object sender, EventArgs e)
        {
			af = new Authorization_Form();
			try
			{
				if (materialTextBox2.Text.Length >= 4)
				{
					//Запрос в БД
					using (var m_dbConn = new SQLiteConnection("Data Source=" + af.dbFileName + ";Version=3;"))
					{
						m_dbConn.Open();

						m_sqlCmd.Connection = m_dbConn;

						SQLiteCommand command = new SQLiteCommand(@"UPDATE Users SET user_password = '" + materialTextBox2.Text + "' WHERE user_id = '" + user_id1 + "'", m_dbConn);
						command.ExecuteNonQuery();
					}

					MaterialMessageBox.Show("Пароль успешно изменён.", "Изменение пароля", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MaterialMessageBox.Show("Пароль должен быть больше 4-х символов.", "Изменение пароля", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch
			{
				MaterialMessageBox.Show("Возникла некоторая ошибка.\nПароль не был изменён.", "Ошибка изменения пароля", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//Изменение почты
        private void materialButton11_Click(object sender, EventArgs e)
        {
			af = new Authorization_Form();
			try
			{
				if (materialTextBox3.Text.Length >= 4)
				{
					//Запрос в БД
					using (var m_dbConn = new SQLiteConnection("Data Source=" + af.dbFileName + ";Version=3;"))
					{
						m_dbConn.Open();

						m_sqlCmd.Connection = m_dbConn;

						SQLiteCommand command = new SQLiteCommand(@"UPDATE Users SET user_email = '" + materialTextBox3.Text + "' WHERE user_id = '" + user_id1 + "'", m_dbConn);
						command.ExecuteNonQuery();
					}

					MaterialMessageBox.Show("Почта успешно изменена.", "Изменение почты", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MaterialMessageBox.Show("Почта должна быть больше 4-х символов.", "Изменение почты", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch
			{
				MaterialMessageBox.Show("Возникла некоторая ошибка.\nПочта не была изменёна.", "Ошибка изменения почты", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
    }
}
