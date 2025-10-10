using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;

namespace PlotCoordenadas
{
    public partial class Form1 : Form
    {
        string connectionString;
        private void EnableDisableObject(object objeto, bool status)
        {
            objeto.GetType().GetProperty("Enabled").SetValue(objeto, status);
            objeto.GetType().GetProperty("Visible").SetValue(objeto, status);

            SelectDB("aa");
        }

        private void SelectDB(string table)
        {
            string query = $"SELECT WKT FROM {table}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Nome: {reader["Nome"]}, Idade: {reader["Idade"]}");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            connectionString = "Data Source=localhost;Initial Catalog=dbCoordenadasPontos;Integrated Security=True;";

        }

        //btn Obter Informacoes
        private void button1_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, true);
        }



        //btn Clientes
        private void button2_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);

        }

        //btn Lotes
        private void button3_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
        }

        //btn Dados Clientes
        private void button4_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
        }

        //btn Rede Esgoto
        private void button5_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
        }

        //btn Rede Fluvial
        private void button6_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
        }
    
        //btn Registros
        private void button7_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
        }
    }
}
