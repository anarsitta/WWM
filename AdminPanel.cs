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
        public String sqlQuery;
        Authorization_Form auth = new Authorization_Form();
        public System.Data.SQLite.SQLiteDataReader acc;

        public long user_id;
        public string user_login;
        public string user_email;
        public string user_name;
        public string user_surname;
        public string user_midname;
        public string user_group;
        public string user_access;
        public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

        public AdminPanel(System.Data.SQLite.SQLiteDataReader account)
        {
            user_id = Convert.ToInt64(account[0]);
            user_login = (string)account[1];
            user_email = (string)account[2];
            user_name = (string)account[3];
            user_surname = (string)account[4];
            user_midname = (string)account[5];
            user_group = (string)account[6];
            user_access = (string)account[8];

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

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            this.Text = "WWM/" + user_name + " " + user_midname;

            materialButton4_Click(sender, e);
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void materialComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialComboBox2.SelectedIndex == 3)
            {
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
                groupTextBox.SelectedIndex = 0;

                groupTextBox.Visible = true;
                name_famTextBox.Visible = false;
                numberTextBox.Visible = false;

            }
            else if (materialComboBox2.SelectedIndex == 4)
            {
                groupTextBox.Visible = false;
                numberTextBox.Visible = true;
                name_famTextBox.Visible = false;
            }
            else
            {
                groupTextBox.Visible = false;
                numberTextBox.Visible = false;
                name_famTextBox.Visible = true;
            }
        }

        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            DataTable dTable = new DataTable();
            DataTable combBox = new DataTable();

            switch (materialComboBox1.SelectedIndex)
            {
                case 0: sqlQuery = "SELECT user_group, user_journum, user_name, user_surname, user_midname, user_login FROM Users "; break;
                case 1: sqlQuery = "DELETE FROM Users "; break;
            }

            switch (materialComboBox2.SelectedIndex)
            { 
                case 0: sqlQuery = sqlQuery + "WHERE user_name LIKE '%" + name_famTextBox.Text + "%' "; break;
                case 1: sqlQuery = sqlQuery + "WHERE user_surname LIKE '%" + name_famTextBox.Text + "%' "; break;
                case 2: sqlQuery = sqlQuery + "WHERE user_midname LIKE '%" + name_famTextBox.Text + "%' "; break;
                case 3: sqlQuery = sqlQuery + "WHERE user_group LIKE '" + groupTextBox.SelectedItem + "' ";  break;
                case 4: sqlQuery = sqlQuery + "WHERE user_journum LIKE '" + numberTextBox.Text + "' "; break;
                case 5: sqlQuery = sqlQuery + "WHERE user_login LIKE '" + name_famTextBox.Text + "' "; break;
            }

            switch (materialComboBox3.SelectedIndex)
            {
                case 0: break;
                case 1: sqlQuery = sqlQuery + "LIMIT 1"; break;
                case 2: sqlQuery = sqlQuery + "LIMIT 5"; break;
                case 3: sqlQuery = sqlQuery + "LIMIT 10"; break;
                case 4: sqlQuery = sqlQuery + "LIMIT 15"; break;
            }

            using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
            {
                if (materialComboBox1.SelectedIndex == 0)
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter.Fill(dTable);
                    if (dTable.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();

                        for (int i = 0; i < dTable.Rows.Count; i++)
                            dataGridView1.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                    else
                    {
                        dataGridView1.Rows.Clear();
                        MaterialMessageBox.Show("Значения, задаваемого вами запроса отсутствуют.", "Значения отсутствуют", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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

        private void EnterButton_Click(object sender, EventArgs e)
        {
            Settings set = new Settings(user_name, user_midname, this);
            set.Show();
            this.Hide();
        }
    }
}
