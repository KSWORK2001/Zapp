using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Drawing;
using System.IO;

namespace Zapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Specify the cache directory in the user's documents folder
            string cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CefSharpCache");

            // Ensure the cache directory exists, or create it if it doesn't
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }

            // Initialize CefSharp with custom settings
            CefSettings cefSettings = new CefSettings
            {
                CachePath = cachePath, // Set the cache directory
            };

            Cef.Initialize(cefSettings);

            this.Size = new Size(1280, 720);

            // Create and configure the ChromiumWebBrowser control for Discord.com
            ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser("https://www.discord.com/login");
            chromiumWebBrowser.Dock = DockStyle.Fill;

            // Add the ChromiumWebBrowser control to the webBrowserPanel
            webBrowserPanel.Controls.Add(chromiumWebBrowser);

            // Calculate the width of the panel as 1/3 of the form's width
            int panelWidth = 1281 / 3;

            // Set the width of the panel
            webBrowserPanel.Size = new Size(panelWidth, this.Height);

            ChromiumWebBrowser chromiumWebBrowser2 = new ChromiumWebBrowser("https://www.dopebox.to");
            chromiumWebBrowser2.Dock = DockStyle.Fill;

            panel2.Controls.Add(chromiumWebBrowser2);

            // Set the initial zoom level using JavaScript
            chromiumWebBrowser.FrameLoadEnd += (sender, args) =>
            {
                string jsCode = "document.body.style.zoom = '100%';"; // Adjust the zoom level as needed
                chromiumWebBrowser.ExecuteScriptAsync(jsCode);
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Shutdown CefSharp when closing the form
            Cef.Shutdown();
            base.OnFormClosing(e);
        }

        private void Form1_SizeChanged(object sender, System.EventArgs e)
        {
            int panelWidth = this.Width / 3;

            // Set the width and position of the panel
            webBrowserPanel.Width = panelWidth;
            webBrowserPanel.Left = this.Width - panelWidth;
            int newPanel1Width = this.Width - webBrowserPanel.Width;
            panel1.Width = newPanel1Width;

            int newPanel2Width = webBrowserPanel.Left - panel2.Left;
            int newPanel2Height = panel1.Top - panel2.Top;
            panel2.Size = new Size(newPanel2Width, newPanel2Height);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    class CustomLifeSpanHandler : ILifeSpanHandler
    {

        bool ILifeSpanHandler.DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            // Implement the logic for the DoClose method here
            return false; // You can return true or false based on your requirements
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            // Check if the pop-up is triggered by a user gesture (e.g., clicking a link)
            if (userGesture)
            {
                // Block pop-ups by canceling the request
                newBrowser = null;
                return true;
            }

            // Allow pop-ups for other cases
            newBrowser = null;
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        // Implement other ILifeSpanHandler methods as needed
    }
}
