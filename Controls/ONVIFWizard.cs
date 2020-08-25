using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using iSpyApplication.Onvif;
using iSpyApplication.Utilities;
using iSpyPRO.DirectShow;
using System.IO;
using System.Drawing;

namespace iSpyApplication.Controls
{
    public partial class ONVIFWizard : UserControl
    {
        public CameraWindow CameraControl;
        public CameraWindow Frame_cam;
        public ONVIFWizard()
        {
            InitializeComponent();

            lblUsername.Text = LocRm.GetString("USername");
            lblPassword.Text = LocRm.GetString("Password");
            lblDeviceURL.Text = LocRm.GetString("NetworkAddress");
            lblTransport.Text = LocRm.GetString("json.transport");
            lblConnectWith.Text = LocRm.GetString("json.connectwith");
            btnConnect.Text = LocRm.GetString("Next");
            btnBack.Text = LocRm.GetString("Back");
            lblURL.Text = LocRm.GetString("URL");

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetPanel(pnlStep1);
    
        }

        private void SetPanel(Panel p)
        {
            HidePanel(pnlStep1);
            HidePanel(pnlStep2);

            p.Visible = true;
            p.Dock = DockStyle.Fill;
            lblStep.Text = "1.";
            if (p == pnlStep2)
                lblStep.Text = "2.";

        }

        private void HidePanel(Panel p)
        {
            p.Visible = false;
            p.Dock = DockStyle.None;
        }

        private void ONVIFWizard_Load(object sender, EventArgs e)
        {
            UISync.Init(this);
            SetPanel(pnlStep1);
            ddlTransport.SelectedIndex = 0;
            ddlConnectWith.Items.Add("FFMPEG");
            if (VlcHelper.VlcInstalled)
                ddlConnectWith.Items.Add("VLC");
            ddlConnectWith.SelectedIndex = 0;
            BindDevices();

            if (CameraControl != null)
            {
                txtOnvifUsername.Text = CameraControl.Camobject.settings.login;
                txtOnvifPassword.Text = CameraControl.Camobject.settings.password;
                numRTSP.Value = CameraControl.Camobject.settings.onvif.rtspport;
                ddlDeviceURL.Text = CameraControl.Camobject.settings.onvifident;
                string path = @"C:\SomeDir2";
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                string text = ddlDeviceURL.Text;
                using (FileStream Onvif_url = new FileStream("Onvif.bhg", FileMode.OpenOrCreate))
                {   
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    Onvif_url.Write(array, 0, array.Length);
                    MessageBox.Show("Onvif connect!");
                }




                // чтение из файла
            //using (FileStream fstream = File.OpenRead($"{path}\note.txt"))
              //  {
                    // преобразуем строку в байты
                  //  byte[] array = new byte[fstream.Length];
                    // считываем данные
                  //  fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                  //  string textFromFile = System.Text.Encoding.Default.GetString(array);
             //       Console.WriteLine($"Текст из файла: {textFromFile}");
               // }



                ddlTransport.SelectedIndex = CameraControl.Camobject.settings.rtspmode;

                string conn = CameraControl.Nv("use");
                if (!string.IsNullOrEmpty(conn) && VlcHelper.VlcInstalled)
                    ddlConnectWith.SelectedItem = conn;
                chkOverrideRTSPPort.Checked = numRTSP.Value != 0;
            }


        }
        #region Nested type: UISync

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        #endregion

        public void Deinit()
        {

        }

        private void BindDevices()
        {
            ddlDeviceURL.Items.Clear();
            foreach (var s in Discovery.DiscoveredDevices)
            {
                ddlDeviceURL.Items.Add(s);
            }
            if (ddlDeviceURL.Items.Count > 0)
                ddlDeviceURL.SelectedIndex = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            GoStep1();
            string writePath = "Requst/source.txt";
            StreamWriter info_source = new StreamWriter(writePath);
            info_source.WriteLine(lbOnvifURLs.Text);
            info_source.Close();
        }

        public void GoStep1()
        {
            lbOnvifURLs.Items.Clear();
            Uri uri;
            if (!Uri.TryCreate(ddlDeviceURL.Text, UriKind.Absolute, out uri))
            {
                MessageBox.Show("Invalid Address");
                return;
            }
            var url = uri.ToString();
            var paq = uri.PathAndQuery;
            if (string.IsNullOrEmpty(paq.Trim('/')))
            {
                url += "onvif/device_service";
            }


            var dev = url;
            var urls = new List<object>();
            int port = 0;
            if (chkOverrideRTSPPort.Checked)
                port = Convert.ToInt32(numRTSP.Value);
            var od = new ONVIFDevice(dev, txtOnvifUsername.Text, txtOnvifPassword.Text, port, 15);

            if (od.Profiles == null)
            {
                MessageBox.Show(this, LocRm.GetString("ConnectFailed"), LocRm.GetString("Failed"));
            }
            else
            {
                foreach (var murl in od.MediaEndpoints)
                {
                    urls.Add(murl);
                }

                if (urls.Count > 0)
                {
                    lbOnvifURLs.Items.Clear();
                    lbOnvifURLs.Items.AddRange(urls.ToArray());
                    lbOnvifURLs.SelectedIndex = 0;
                    SetPanel(pnlStep2);
                }
                else
                {
                    MessageBox.Show(this, "No media endpoints found", LocRm.GetString("Failed"));
                }
            }
        }

        private void chkOverrideRTSPPort_CheckedChanged(object sender, EventArgs e)
        {
            numRTSP.Enabled = chkOverrideRTSPPort.Checked;
            if (!numRTSP.Enabled)
            {
                numRTSP.Value = 0;
            }
        }

        private void ddlDeviceURL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numRTSP_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        
        }

        private void lbOnvifURLs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
