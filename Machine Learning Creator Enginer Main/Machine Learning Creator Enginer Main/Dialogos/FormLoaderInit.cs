using System;
using System.Threading;
using System.Windows.Forms;

namespace Machine_Learning_Creator_Enginer_Main.Dialogos
{
    public partial class FormLoaderInit : Form
    {
        bool openEngine = false;
        public FormLoaderInit(bool _openEngine = false)
        {
            InitializeComponent();
            openEngine = _openEngine;
        }

        int tiempo;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            tiempo += 4;
            if (tiempo <= 100)
                progressBar1.Value = tiempo;
            if(tiempo > 120)
            {
                timer1.Stop();
                UseWaitCursor = true;
                Thread.Sleep(5000);
                UseWaitCursor = false;
                FormMain.Singletone.ShowInTaskbar = true;
                if (openEngine)
                {
                    FormMain.Singletone.engineWindow = new DialogWindow(TypeDialogWindow.OpenEngine);
                    FormMain.Singletone.engineWindow.Open();
                    FormMain.Singletone.engineWindow.OnExit += () => {
                        if (Datos.Exist(UtilNames.TEMP_FILE))
                        {
                            DialogWindow message = new DialogWindow(TypeDialogWindow.ShowMessage, messageTextTittle: "Se a Producido un Error", messageTextMessage: "El Enginer De Machine Learning Se a Cerrado de Pronto.", btnAcept: "Aceptar");
                            message.Open();
                            Datos.DeleteTempFile();
                        }
                    };
                }
                Close();
            }
        }
        private void FormLoaderInit_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}