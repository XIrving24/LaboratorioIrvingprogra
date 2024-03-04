using System.IO;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.LinkLabel;
using System.Reflection;
using System.Security.Cryptography;

namespace Laboratorio__2
{
    public partial class Form1 : Form
    {
        List<URL> urls = new List<URL>();
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView.NavigationStarting += EnsureHttps;
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            comboBox1.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            comboBox1.Width = goButton.Left - comboBox1.Left;
        }

        private void Guardar(string fileName, string texto)
        {
            FileStream flujo = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter escritor = new StreamWriter(flujo);
            escritor.WriteLine(texto);
            escritor.Close();
        }

        private void Leer()
        {
            string fileName = "Historial.txt";

            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            while (reader.Peek() > -1)
            {
                //Una url para ir guardando cada 3 lineas que se leen del archivo
                URL url = new URL();
                url.Pagina = reader.ReadLine();
                url.Veces = Convert.ToInt32(reader.ReadLine());
                url.Fecha = Convert.ToDateTime(reader.ReadLine());

                //La url con sus datos se guarda en la lista
                urls.Add(url);
            }
            reader.Close();

            comboBox1.DisplayMember = "Pagina";
            comboBox1.DataSource = urls;
            comboBox1.Refresh();
        }

        private void Grabar(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            foreach (var u in urls)
            {
                writer.WriteLine(u.Pagina);
                writer.WriteLine(u.Veces);
                writer.WriteLine(u.Fecha);
            }
            writer.Close();
        }

        private void BotonIr_Click(object sender, EventArgs e)
        {
            //La URL que voy a analizar
            string urlIngresada = comboBox1.Text.ToString();
            URL urlExiste = urls.Find(u => u.Pagina == urlIngresada);

            if (urlIngresada.Contains(".") || urlIngresada.Contains("/") || urlIngresada.Contains(":")) 
            {
                if (urlIngresada.Contains("https"))
                {
                    webView.CoreWebView2.Navigate(urlIngresada);
                }
                else
                {
                    urlIngresada = "https//" + urlIngresada;
                }
            }    
            else
                {
                    if (!string.IsNullOrEmpty(urlIngresada))
                    {
                        urlIngresada = "https://www.google.com/search?q=" + urlIngresada;
                        webView.CoreWebView2.Navigate(urlIngresada);
                    }
                }
            


            string link = "";
            if (!(comboBox1.Text.Contains(".")))
            {
                link = "https://www.google.com/search?q=" + comboBox1.Text;
            }

            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(link);
            }

            if (urlExiste == null)
            {
                URL urlNueva = new URL();
                urlNueva.Pagina = urlIngresada;
                urlNueva.Veces = 1;
                urlNueva.Fecha = DateTime.Now;
                urls.Add(urlNueva );
                Grabar("C:\\Users\\cifir\\Historial.txt");
                webView.CoreWebView2.Navigate(urlIngresada);
            }
            else
            {
                urlExiste.Veces++;
                urlExiste.Fecha = DateTime.Now;
                Grabar("C:\\Users\\cifir\\Historial.txt");
                webView.CoreWebView2.Navigate(urlExiste.Pagina);
            }
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void haciaDelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Leer();
        }

        private void webView_Click(object sender, EventArgs e)
        {

        }
    }
}
