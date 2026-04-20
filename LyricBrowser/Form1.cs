using System;
using System.Drawing;
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

            topPanel = new Panel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(5), BackColor = Color.LightGray };
            
            goButton = new Button { Text = "Ir", Dock = DockStyle.Right, Width = 60 };
            goButton.Click += (s, e) => NavigateToUrl();

            addressBar = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12) };
            addressBar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) NavigateToUrl(); };

            topPanel.Controls.Add(addressBar);
            topPanel.Controls.Add(goButton);
            this.Controls.Add(topPanel);
        }

        private void InitializeBrowser()
        {
            var settings = new CefSettings();
            // Referencia explícita para evitar confusiones
            if (!CefSharp.Cef.IsInitialized) CefSharp.Cef.Initialize(settings);

            browser = new ChromiumWebBrowser("https://www.google.com");
            browser.Dock = DockStyle.Fill;
            
            this.Controls.Add(browser);
            browser.BringToFront();
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