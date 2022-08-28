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
        public string user_name;
        public string user_midname;
		public MaterialForm form;
		public MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

		public Settings(string name, string midname, MaterialForm mf)
        {
            user_name = name;
            user_midname = midname;
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

			form = mf;
			
		}

        private void Settings_Load(object sender, EventArgs e)
        {
            this.Text = "WWM/" + user_name + " " + user_midname + "/Настройки";
        }

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

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
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
    }
}
