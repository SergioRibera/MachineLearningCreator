using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Machine_Learning_Creator_Enginer_Main.Dialogos
{
    public partial class CreateProyect : Form
    {
        public CreateProyect()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nameProject = textBox1.Text;
            if(nameProject.Contains("|") || nameProject.Contains(".") || nameProject.Contains("&") || nameProject.Contains("%") | nameProject.Contains("$") || nameProject.Contains("#"))
            {
                MessageBox.Show("El nombre del proyecto no debe contener los siguientes caracteres: \n |  .  &  %  $  #");
            }else if(string.IsNullOrEmpty(nameProject) || string.IsNullOrWhiteSpace(nameProject))
            {
                MessageBox.Show("El nombre del proyecto no debe estar vacio");
            }
            else
            {
                Datos.CreateProyect(nameProject);
                FormMain.Singletone.LoadProyects();
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
