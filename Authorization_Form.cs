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
        //Создание курсора и подключения
        public String dbFileName = "MainDataBase.sqlite";
        public SQLiteConnection m_dbConn = new SQLiteConnection();
        public SQLiteCommand m_sqlCmd = new SQLiteCommand();

        public Authorization_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Users (user_id INTEGER PRIMARY KEY AUTOINCREMENT, user_login TEXT NOT NULL, user_password TEXT NOT NULL, user_email TEXT NOT NULL, user_name TEXT NOT NULL, user_surname TEXT NOT NULL, user_midname TEXT NOT NULL, user_group TEXT NOT NULL, user_journum INTEGER, user_access TEXT NOT NULL DEFAULT ('Пользователь'), UNIQUE (\"user_login\") ON CONFLICT IGNORE)";
                m_sqlCmd.ExecuteNonQuery();
            }
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            using (var m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;"))
            {
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "SELECT";
                m_sqlCmd.ExecuteReader();

            }
        }

        private void RegistrationButton_Click(object sender, EventArgs e)
        {
            Hide();
            Registration_Form reg_f = new Registration_Form();
            reg_f.Show();
        }

        private void Authorization_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
