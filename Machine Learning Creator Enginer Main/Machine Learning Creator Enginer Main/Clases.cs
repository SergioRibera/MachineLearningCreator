using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Machine_Learning_Creator_Enginer_Main
{
    [Serializable]
    public class ProyectoMLC
    {
        public string proyectName;
        public string dateCreatedProyect;
        public string currentDateOpenProyect;
        public string versionMLC;
        public string locationProyect;
        public Settings settings;
        public List<ObjectMachineLearning> objetosDeMachineLearning;

        public ProyectoMLC()
        {
            proyectName = "New Proyect";
            dateCreatedProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            currentDateOpenProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            versionMLC = UtilNames.VERSION_MLC;
            settings = new Settings();
            settings.Idioma = Languaje.es;
            objetosDeMachineLearning = new List<ObjectMachineLearning>();
        }
        public ProyectoMLC(string ProyectName)
        {
            proyectName = ProyectName;
            dateCreatedProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            currentDateOpenProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            versionMLC = UtilNames.VERSION_MLC;
            settings = new Settings();
            settings.Idioma = Languaje.es;
            objetosDeMachineLearning = new List<ObjectMachineLearning>();
        }
        public ProyectoMLC(string ProyectName, string location)
        {
            proyectName = ProyectName;
            dateCreatedProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            currentDateOpenProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            versionMLC = UtilNames.VERSION_MLC;
            settings = new Settings();
            settings.Idioma = Languaje.es;
            locationProyect = location;
            objetosDeMachineLearning = new List<ObjectMachineLearning>();
        }
        public void Open() {
            currentDateOpenProyect = DateTime.Now.ToString("dd/MM/yyyy  h:mm tt");
            versionMLC = UtilNames.VERSION_MLC;
            Datos.CreateTempFile(locationProyect);
        }
        public override string ToString()
        {
            return string.Format("Nombre de proyecto : {0}\nProyecto Creado en : {1}\nUltima vez abierto en : {2}\nLa version de MachineLearningCreator usada en este proyect es : {3}", proyectName, dateCreatedProyect, currentDateOpenProyect, versionMLC);
        }
    }

    public enum ExtensionFile { sets, lan, mlcpd, ls }
    public enum Languaje { en, es }
    [Serializable]
    public class Settings
    {
        public Languaje Idioma;
        public Settings()
        {
            Idioma = Languaje.es;
        }
    }
    [Serializable]
    public partial class Idioma
    {
        public string LanguajeName;
        public StringMatrix Contenido;
    }
    [Serializable]
    public class IndexToDownload
    {
        //  Elemento    Download Link
        public StringMatrix Elementos;
    }
    [Serializable]
    public class StringMatrix
    {
        public List<string> Keys;
        public List<string> Values;

        public StringMatrix()
        {
            if (Keys == null)
                Keys = new List<string>();
            if (Values == null)
                Values = new List<string>();
        }
        public StringMatrix(Dictionary<string, string> content)
        {
            if (Keys == null)
                Keys = new List<string>();
            if (Values == null)
                Values = new List<string>();
            foreach (var c in content)
            {
                Keys.Add(c.Key);
                Values.Add(c.Value);
            }
        }
        public StringMatrix(List<string> keys, List<string> values)
        {
            if (Keys == null)
                Keys = new List<string>();
            if (Values == null)
                Values = new List<string>();
            foreach (var k in keys)
            {
                Keys.Add(k);
            }
            foreach (var v in values)
            {
                Values.Add(v);
            }
        }

        public int Count { get { return (Keys.Count == Values.Count) ? Keys.Count : throw new Exception("El Tamaño De Las Listas No Son Iguales"); } }

        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
        }

        public void Remove(int index)
        {
            Keys.Remove(Keys[index]);
            Values.Remove(Values[index]);
        }
        public void Remove(string key)
        {
            int i = Keys.IndexOf(key);
            Keys.Remove(Keys[i]);
            Values.Remove(Values[i]);
        }

        public void Add(string key, string content)
        {
            Keys.Add(key);
            Values.Add(content);
        }
        public void Add(Dictionary<string, string> content)
        {
            foreach (var c in content)
            {
                Keys.Add(c.Key);
                Values.Add(c.Value);
            }

        }

        public string GetKey(string content)
        {
            int index = Values.IndexOf(content);
            return Keys[index];
        }
        public string GetKey(int content)
        {
            return Keys[content];
        }
        public string GetValue(string key)
        {
            int index = Keys.IndexOf(key);
            return Values[index];
        }
        public string GetValue(int key)
        {
            return Values[key];
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < Count; i++)
            {
                s += GetKey(i) + " : " + GetValue(i) + "\n";
            }
            return s;
        }

        public string this[string Key]
        {
            get
            {
                return Values[Keys.IndexOf(Key)];
            }
            set
            {
                if (Keys.IndexOf(Key) < 0)
                    Keys.Add(value);
                else
                    Keys[Keys.IndexOf(Key)] = value;

                if (Values.IndexOf(Key) < 0)
                    Values.Add(value);
                else
                    Values[Values.IndexOf(Key)] = value;
            }
        }
        public string this[int K, bool key]
        {
            get
            {
                if (key)
                {
                    return Keys[K];
                }
                return Values[K];
            }
            set
            {
                if (key)
                {
                    if (Keys.Count < K)
                        Keys.Add(value);
                    else
                        Keys[K] = value;
                }
                else
                {
                    if (Values.Count < K)
                        Values.Add(value);
                    else
                        Values[K] = value;
                }
            }
        }
        public static implicit operator StringMatrix(Dictionary<string, string> s)
        {
            return new StringMatrix(s);
        }

        public static implicit operator Dictionary<string, string>(StringMatrix sm)
        {
            Dictionary<string, string> s = new Dictionary<string, string>();
            for (int i = 0; i < sm.Count; i++)
                s.Add(sm.GetKey(i), sm.GetValue(i));
            return s;
        }
        public static implicit operator string(StringMatrix sm)
        {
            return sm.ToString();
        }
    }
    public enum TypeDialogWindow { Loader, ShowMessage, OpenEngine }
    public delegate void OnExitEvent();
    public class DialogWindow
    {
        Process process;
        ProcessStartInfo info;
        public event OnExitEvent OnExit;

        public DialogWindow(TypeDialogWindow typeDialog, string loadedTextTittle = "Loading...", string loadedTextMessage = "Loading...", string messageTextTittle = "Alert!", string messageTextMessage = "MensajeDeAlerta...", string btnAcept = "Acept")
        {
            info = new ProcessStartInfo();

            string args = "";

            switch (typeDialog)
            {
                case TypeDialogWindow.Loader:
                    info.FileName = Application.StartupPath + "/dialogs/DialogsWindowsLoader.exe";
                    args = "tittle=" + loadedTextTittle.Compatibilizate() + " " + "msg=" + loadedTextMessage.Compatibilizate() + " ";
                    break;
                case TypeDialogWindow.ShowMessage:
                    info.FileName = Application.StartupPath + "/dialogs/DialogsWindowsMessage.exe";
                    args = "tittle=" + messageTextTittle.Compatibilizate() + " " + "msg=" + messageTextMessage.Compatibilizate() + " " + "btnAcept=" + btnAcept.Compatibilizate() + " ";
                    break;
                case TypeDialogWindow.OpenEngine:
                    info.FileName = Application.StartupPath + "/MLCEnginer.exe";
                    break;
            }

            info.Arguments = args;
            info.UseShellExecute = true;
            info.CreateNoWindow = true;
            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = false;
            info.RedirectStandardError = false;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            OnExit?.Invoke();
        }

        public void Open()
        {
            process = Process.Start(info);
            process.Exited += Process_Exited;
        }
        public void Close()
        {
            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                    OnExit?.Invoke();
                }
            }
        }
        public bool Wait(int milliseconds = 0)
        {
            if(milliseconds != 0)
                return process.WaitForInputIdle(milliseconds);
            return process.WaitForInputIdle();
        }
    }
}