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
    public partial class UserPanel : MaterialForm
    {
        public System.Data.SQLite.SQLiteDataReader acc;
        public UserPanel(System.Data.SQLite.SQLiteDataReader account)
        {
            InitializeComponent();
            acc = account;
        }

        private void UserPanel_Load(object sender, EventArgs e)
        {
            Authorization_Form auth = new Authorization_Form();

        }
    }
}
