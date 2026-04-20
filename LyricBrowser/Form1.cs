using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace LyricBrowser
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser? browser;
        private TextBox? addressBar;
        private Button? goButton;
        private Button? backButton;
        private Button? forwardButton;
        private Button? reloadButton;
        private Panel? topPanel;

        public Form1()
        {
            InitializeComponent();
            SetupLayout();
            InitializeBrowser();
        }

        private void SetupLayout()
        {
            this.Text = "Lyric Browser - Alpha";
            this.Size = new Size(1024, 768);

            topPanel = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(5), BackColor = Color.WhiteSmoke };
            
            backButton = new Button { Text = "◁", Dock = DockStyle.Left, Width = 35, Cursor = Cursors.Hand };
            forwardButton = new Button { Text = "▷", Dock = DockStyle.Left, Width = 35, Cursor = Cursors.Hand };
            reloadButton = new Button { Text = "↻", Dock = DockStyle.Left, Width = 35, Cursor = Cursors.Hand };

            backButton.Click += (s, e) => { if (browser != null && browser.CanGoBack) browser.Back(); };
            forwardButton.Click += (s, e) => { if (browser != null && browser.CanGoForward) browser.Forward(); };
            reloadButton.Click += (s, e) => { browser?.Reload(); };

            goButton = new Button { Text = "Ir", Dock = DockStyle.Right, Width = 60, Cursor = Cursors.Hand };
            goButton.Click += (s, e) => NavigateToUrl();

            addressBar = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11) };
            addressBar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) NavigateToUrl(); };

            topPanel.Controls.Add(addressBar);
            topPanel.Controls.Add(goButton);
            topPanel.Controls.Add(reloadButton);
            topPanel.Controls.Add(forwardButton);
            topPanel.Controls.Add(backButton);
            
            this.Controls.Add(topPanel);
        }

        private void InitializeBrowser()
        {
            var settings = new CefSettings();
            if (!CefSharp.Cef.IsInitialized) CefSharp.Cef.Initialize(settings);

            // Obtenemos la ruta local de nuestro home.html
            string homePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "home.html");

            // Iniciamos el navegador con nuestra página principal
            browser = new ChromiumWebBrowser(homePath);
            browser.Dock = DockStyle.Fill;
            
            // EVENTO NUEVO: Actualiza la barra superior cuando cambia la web
            browser.AddressChanged += OnBrowserAddressChanged;

            this.Controls.Add(browser);
            browser.BringToFront();
        }

        // Esta función atrapa los cambios de URL y actualiza el TextBox superior
        private void OnBrowserAddressChanged(object? sender, AddressChangedEventArgs e)
        {
            if (addressBar != null && !addressBar.IsDisposed)
            {
                this.Invoke(new Action(() => {
                    // CORRECCIÓN: Filtramos la ruta local para que se vea profesional
                    if (e.Address != null && e.Address.Contains("home.html"))
                    {
                        addressBar.Text = ""; // Dejamos la barra limpia (o puedes poner "lyric://inicio")
                    }
                    else
                    {
                        addressBar.Text = e.Address;
                    }
                }));
            }
        }

        private void NavigateToUrl()
        {
            if (browser != null && addressBar != null && !string.IsNullOrWhiteSpace(addressBar.Text))
            {
                browser.LoadUrl(addressBar.Text);
            }
        }
    }
}