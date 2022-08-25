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
        Authorization_Form auth = new Authorization_Form();
        public System.Data.SQLite.SQLiteDataReader acc;
        public AdminPanel(System.Data.SQLite.SQLiteDataReader account)
        {

            acc = account;
            InitializeComponent();
            
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            DataTable dTable = new DataTable();
            String sqlQuery;

            this.Text = "WWM/" + Convert.ToString(acc[2]) + " " + Convert.ToString(acc[4]);

            using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
            {
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
            String sqlQuery;

            using (var m_dbConn = new SQLiteConnection("Data Source=" + auth.dbFileName + ";Version=3;"))
            {
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
}
