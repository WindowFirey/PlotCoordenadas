using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Web.WebView2.WinForms;

namespace PlotCoordenadas
{
    public partial class Form1 : Form
    {
        private const string GOOGLE_MAPS_API_KEY = "AIzaSyBsZzIKvEatNuT-uPjIPp_-eptj_16AvcE";

        string connectionString;
        List<List<double>> coordenadas = new List<List<double>>();
        private WebView2 webView;


        private void EnableDisableObject(object objeto, bool status)
        {
            objeto.GetType().GetProperty("Enabled").SetValue(objeto, status);
            objeto.GetType().GetProperty("Visible").SetValue(objeto, status);

        }

        private async void SelectDB(string table)
        {
            string query = $"SELECT TOP 1 WKT FROM {table}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    coordenadas.Clear();
                    string coordenada = reader["WKT"].ToString();

                    if (coordenada.Contains(","))
                    {
                        string[] coordenadasSplit = coordenada.Split(',');
                        
                        foreach (string coordenadasStr in coordenadasSplit)
                        {
                            List<double> valores = coordenadasStr.Split(' ')
                                                                 .Where(s => !string.IsNullOrWhiteSpace(s))
                                                                 .Select(double.Parse)
                                                                 .ToList();
                            coordenadas.Add(valores);
                        }

                        //string formato = "[" + string.Join(", ", coordenadas.Select(inner =>
                        //    "[" + string.Join(", ", inner) + "]")) + "]";
                        //MessageBox.Show($"Virgula   {formato}");

                    }
                    else
                    {
                        List<double> valores = coordenada.Split(' ')
                                     .Where(s => !string.IsNullOrWhiteSpace(s))
                                     .Select(double.Parse)
                                     .ToList();

                        coordenadas.Add(valores);

                        //// Adiciona todos os marcadores
                        foreach (var coord in coordenadas)
                        {
                            await AdicionarMarcador(coordenadas[1], coordenadas[0], "Pontos");
                        }
                        //string formato = "[" + string.Join(", ", coordenadas.Select(inner =>
                        //    "[" + string.Join(", ", inner) + "]")) + "]";
                        //MessageBox.Show($"Espaco   {formato}");
                    }
                }
            }
        }
        private async System.Threading.Tasks.Task AdicionarMarcador(double lat, double lng, string titulo)
        {
            string script = $"addMarker({lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                           $"{lng.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                           $"'{titulo}'); fitBounds();";

            await webView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async(null);

            string html = CriarHtmlComMapa();
            webView.NavigateToString(html);

            // Aguarda o mapa carregar
            await System.Threading.Tasks.Task.Delay(2000);


        }


        private string CriarHtmlComMapa()
        {

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Mapa</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            height: 100%;
        }}
        #map {{
            height: 100%;
            width: 100%;
        }}
    </style>
</head>
<body>
    <div id='map'></div>
    
    <script>
        let map;
        let markers = [];
        
        function initMap() {{
            // Centro do mapa (Brasil)
            map = new google.maps.Map(document.getElementById('map'), {{
                center: {{ lat: -22.3438, lng: -44.5730 }}, 
                zoom: 4
            }});
        }}
        
        function addMarker(lat, lng, title) {{
            const marker = new google.maps.Marker({{
                position: {{ lat: lat, lng: lng }},
                map: map,
                title: title,
                animation: google.maps.Animation.DROP
            }});
            
            // InfoWindow ao clicar
            const infoWindow = new google.maps.InfoWindow({{
                content: `<h3>${{title}}</h3><p>Lat: ${{lat}}<br>Lng: ${{lng}}</p>`
            }});
            
            marker.addListener('click', () => {{
                infoWindow.open(map, marker);
            }});
            
            markers.push(marker);
            return marker;
        }}
        
        function clearMarkers() {{
            markers.forEach(marker => marker.setMap(null));
            markers = [];
        }}
        
        function fitBounds() {{
            if (markers.length > 0) {{
                const bounds = new google.maps.LatLngBounds();
                markers.forEach(marker => bounds.extend(marker.getPosition()));
                map.fitBounds(bounds);
            }}
        }}
    </script>
    
    <script async defer
        src='https://maps.googleapis.com/maps/api/js?key={GOOGLE_MAPS_API_KEY}&callback=initMap'>
    </script>
</body>
</html>";
        }

        public Form1()
        {
            InitializeComponent();
            connectionString = "Data Source=localhost;Initial Catalog=dbCoordenadasPontos;Integrated Security=True;";

            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            InitializeWebView();
        }


        //btn Areas de Interesse
        private void button8_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("AreasDeInteresse");
        }

        //btn Clientes
        private void button2_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("Clientes");
        }
        
        //btn Hidrografia
        private void button9_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("Hidrografia");
        }

        //btn Lotes
        private void button3_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("Lotes");
        }

        //btn mapa de setores
        private void button10_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("MapaDeSetores");
        }

        //btn Dados Clientes
        private void button4_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("PontosColetadosClientes");
        }

        //btn Rede Esgoto
        private void button5_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("RedeDeEsgoto");
        }

        //btn Rede Fluvial
        private void button6_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("RedePluvial");

        }

        //btn Registros
        private void button7_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, false);
            SelectDB("Registros");
        }



        //btn Obter Informacoes
        private void button1_Click(object sender, EventArgs e)
        {
            EnableDisableObject(panel1, true);
        }






        private void webView21_Click(object sender, EventArgs e)
        {

        }
    }
}
