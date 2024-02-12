using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowerAvenue
{
    public partial class FormWebview : Form
    {

        string flveTempPath;
        public FormWebview()
        {
            InitializeComponent();
        }

        public void deleteDir(string PATH)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(PATH);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private async void FormWebview_Load(object sender, EventArgs e)
        {
            string tempPath = Path.GetTempPath();
            flveTempPath = Path.Combine(tempPath, "FloweraveResources");
            string wv2TempPath = Path.Combine(tempPath, "FloweraveWebView2");
            if (Directory.Exists(flveTempPath))
                deleteDir(flveTempPath);
            else if (File.Exists(flveTempPath))
                File.Delete(flveTempPath);


            CoreWebView2Environment corewv2Env = await CoreWebView2Environment.CreateAsync(userDataFolder: wv2TempPath);
            await this.webView.EnsureCoreWebView2Async(corewv2Env);
            
            MemoryStream flveStream = new MemoryStream(Properties.Resources.flowerave_resources);
            ZipArchive flveArchive = new ZipArchive(flveStream);
            
            flveArchive.ExtractToDirectory(flveTempPath);

            this.webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            this.webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.webView.Source = new System.Uri(Path.Combine(flveTempPath, "index.html"), System.UriKind.Absolute);
            
        }


        private void FormWebview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(flveTempPath))
                deleteDir(flveTempPath);
        }
    }
}
