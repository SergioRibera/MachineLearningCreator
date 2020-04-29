using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Machine_Learning_Creator_Enginer_Main;
using Machine_Learning_Creator_Enginer_Main.Dialogos;

namespace Machine_Learning_Creator_Enginer_Main
{
    public partial class FormMain : Form
    {
        Settings settings;
        List<ProyectoMLC> proyectos = new List<ProyectoMLC>();
        Idioma idioma;

        public DialogWindow engineWindow = null;
        public static FormMain Singletone;

        public FormMain()
        {
            InitializeComponent();
            if (Singletone == null) Singletone = this;
        }

        private void Btn_minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void Btn_maximize_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
        }
        private void Btn_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            new FormLoaderInit().Show();
            Console.WriteLine(Application.StartupPath);
            if (Datos.Exist(UtilNames.DATA_SETTINGS, ExtensionFile.sets))
            {
                settings = Datos.Load<Settings>(UtilNames.DATA_SETTINGS, ExtensionFile.sets);
            }
            else
            {
                settings = new Settings();
                settings.Save<Settings>(UtilNames.DATA_SETTINGS, ExtensionFile.sets);
            }
            if (Datos.Exist(settings.Idioma.ToString(), ExtensionFile.lan))
            {
                idioma = Datos.Load<Idioma>(settings.Idioma.ToString(), ExtensionFile.lan);
            }
            LoadProyects();
        }
        public void LoadProyects()
        {
            proyectsList.Items.Clear();
            proyectsDeleteList.Items.Clear();
            proyectsList.TileSize = new Size(proyectsList.Size.Width - 30 , 50);
            Datos.GetProyects().Foreach((s) => {
                if (s != null)
                {
                    ProyectoMLC p = Datos.GetProyect(s);
                    ListViewItem itemName = new ListViewItem(new string[] { p.proyectName, p.versionMLC, p.currentDateOpenProyect }, null, Color.Black, Color.White, new Font("Arial", 10f, FontStyle.Regular));
                    ListViewItem btnDelte = new ListViewItem(new string[] { "Delete Project" }, null, Color.Black, Color.White, new Font("Arial", 10f, FontStyle.Regular));
                    proyectsList.Items.Add(itemName);
                    proyectsDeleteList.Items.Add(btnDelte);
                    proyectos.Add(p);
                    Datos.SaveProyect(p);
                }
            });
        }
        private void ProyectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.SelectedIndices.Count > 0)
            {
                proyectos[list.SelectedIndices[0]].Open();
                new FormLoaderInit(true).Show();
            }
            else
            {
                list.SelectedItems.Clear();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new CreateProyect().Show();
        }
        void ButonDelteProyect(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("¿ Really you want delete this project ?", "Delete Project", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Datos.DeleteProyect(proyectos[list.SelectedIndices[0]].locationProyect);
                    LoadProyects();
                }
            }
            else
            {
                list.SelectedItems.Clear();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (engineWindow != null)
                engineWindow.Close();
        }
    }
}