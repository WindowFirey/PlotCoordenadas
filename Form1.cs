using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlotCoordenadas
{
    public partial class Form1 : Form
    {
        private void enableDisanbleObject(object objeto, bool status)
        {
            objeto.GetType().GetProperty("Enabled").SetValue(objeto, status);
            objeto.GetType().GetProperty("Visible").SetValue(objeto, status);
        }
        public Form1()
        {
            InitializeComponent();
        }

        //btn Obter Informacoes
        private void button1_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, true);
        }



        //btn Clientes
        private void button2_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);

        }

        //btn Lotes
        private void button3_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);
        }

        //btn Dados Clientes
        private void button4_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);
        }

        //btn Rede Esgoto
        private void button5_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);
        }

        //btn Rede Fluvial
        private void button6_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);
        }

        //btn Registros
        private void button7_Click(object sender, EventArgs e)
        {
            enableDisanbleObject(panel1, false);
        }
    }
}
