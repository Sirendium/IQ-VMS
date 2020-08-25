using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Cloud;
using iSpyApplication.Server;
using Microsoft.Win32;
using NAudio.Wave;
using iSpyApplication.Controls;
using iSpyApplication.Joystick;
using iSpyApplication.Utilities;
using Encoder = System.Drawing.Imaging.Encoder;

namespace iSpyApplication
{
    public partial class Settings : Form
    {
        public static readonly object[] StartupModes = 
            {
                "Normal","Minimised","Maximised","FullScreen"
            };

        public static readonly object[] PlaybackModes =
        {
            "Website", "iSpy", "Default"
        };
        public static readonly object[] CloudProviders = 
            {
                "Drive","Dropbox","Flickr","OneDrive","Box"
            };

        public static readonly object[] Priorities =
        {
            "Normal","AboveNormal","High","RealTime"
        };
        private const int Rgbmax = 255;
        private JoystickDevice _jst;
        public int InitialTab;
        public bool ReloadResources;
        readonly string _noDevices = LocRm.GetString("NoAudioDevices");
        private RegistryKey _rkApp;
        private string[] _sticks;
        private static readonly object Jslock = new object();
        private bool _loaded;
        public MainForm MainClass;
        public FolderSelectDialog Fsd = new FolderSelectDialog();

        public Settings()
        {
            InitializeComponent();
            RenderResources();            
        }

        private void Button1Click(object sender, EventArgs e)
        {
            string err = "";

         
            
            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                return;
            }

           
         
          
            SaveSMTPSettings();
            
            if (!string.IsNullOrEmpty(MainForm.Conf.ArchiveNew))
            {
                if (!MainForm.Conf.ArchiveNew.EndsWith(@"\"))
                    MainForm.Conf.ArchiveNew += @"\";
            }

            MainForm.Iconfont = new Font(FontFamily.GenericSansSerif, MainForm.Conf.BigButtons ? 22 : 15, FontStyle.Bold, GraphicsUnit.Pixel);
            
            MainForm.Conf.TalkMic = "";
         
            

            MainForm.SetPriority();

           
         
            LocalServer.ReloadAllowedIPs();

           
           
            LocalServer.ReloadAllowedReferrers();

            


           
          
            
           

            //SetStorageOptions();

            MainForm.ReloadColors();

           
          
            DialogResult = DialogResult.OK;
            Close();
        }

        private jbutton _curButton;
        private jaxis _curAxis;

        void JbuttonGetInput(object sender, EventArgs e)
        {
          

            if (sender!=null)
                _curButton = (jbutton) sender;
            else
            {
                _curButton = null;
            }
        }

        void JaxisGetInput(object sender, EventArgs e)
        {
            
            if (sender!=null)
                _curAxis = (jaxis)sender;
            else
            {
                _curAxis = null;
            }
        }

        

        private void Button2Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsLoad(object sender, EventArgs e)
        {
            if (!Helper.HasFeature(Enums.Features.Settings))
            {
                using (var cp = new CheckPassword())
                {
                    cp.ShowDialog(this);
                }
            }

            if (!Helper.HasFeature(Enums.Features.Settings))
            {
                MessageBox.Show(this, LocRm.GetString("AccessDenied"));
                Close();
                return;
            }

            UISync.Init(this);
            tcTabs.SelectedIndex = InitialTab;

            _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);

            SetColors();



            int i = 0, selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {

                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }

            SetSSLText();



      



        
          
         
      

 

            _jst = new JoystickDevice();
            var ij = 0;
            _sticks = _jst.FindJoysticks();
            i = 1;
            foreach(string js in _sticks)
            {
                var nameid = js.Split('|');
            
                if (nameid[1] == MainForm.Conf.Joystick.id)
                    ij = i;
                i++;
            }




      


            HideTab(tabPage11, Helper.HasFeature(Enums.Features.Plugins));

            //important leave here:

            if (Helper.HasFeature(Enums.Features.Plugins))
                ListPlugins();

      
            
            _loaded = true;
        }

        private void HideTab(TabPage t, bool show)
        {
            if (!show)
            {
                tcTabs.TabPages.Remove(t);
            }
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("settings");
           
            button1.Text = LocRm.GetString("Ok");
            button2.Text = LocRm.GetString("Cancel");
           
        }


        private void SetColors()
        {
           
        }

        private static Color InverseColor(Color colorIn)
        {
            return Color.FromArgb(Rgbmax - colorIn.R,
                                  Rgbmax - colorIn.G, Rgbmax - colorIn.B);
        }

        private void chkStartup_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void BtnBrowseVideoClick(object sender, EventArgs e)
        {
            
        }

        private void Button3Click(object sender, EventArgs e)
        {
           
        }

        private void BtnDetectColorClick(object sender, EventArgs e)
        {
          

            if (cdColorChooser.ShowDialog(this) == DialogResult.OK)
            {
                
                SetColors();
            }
        }

        private void BtnColorTrackingClick(object sender, EventArgs e)
        {
           
        }

        private void BtnColorVolumeClick(object sender, EventArgs e)
        {
           
        }

        private void BtnColorMainClick(object sender, EventArgs e)
        {

        }

        private void BtnColorBackClick(object sender, EventArgs e)
        {
            
        }

        private void BtnColorAreaClick(object sender, EventArgs e)
        {
            
        }

        private void chkPasswordProtect_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void TbOpacityScroll(object sender, EventArgs e)
        {
           
        }

        private void chkErrorReporting_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkShowGettingStarted_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            _jst?.ReleaseJoystick();
        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LinkLabel1LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var d = new downloader
            {
                Url = MainForm.ContentSource + "/getcontent.aspx?name=translations3&encoding=utf-8",
                SaveLocation = Program.AppDataPath + @"XML\Translations.xml"
            };
            d.ShowDialog(this);
            if (d.DialogResult == DialogResult.OK)
            {
                LocRm.Reset();
                UISync.Execute(ReloadLanguages);
            }
            d.Dispose();
        }

        private void ReloadLanguages()
        {           
            
        
           
        }

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try { _sync.BeginInvoke(action, null); }
                catch { }
            }
        }

        private void LinkLabel2LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/yaf/forum.aspx?g=posts&m=678&#post678#post678");
        }

        #region Nested type: ListItem

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string[] Value;

            public ListItem(string name, string[] value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        #endregion

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-settings.aspx");
        }


        private void chkMonitor_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnBorderHighlight_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void ddlJoystick_SelectedIndexChanged(object sender, EventArgs e)
        {
            



            



            

            
            
        }

        private int[] _axisLast;
        private int[] _dPadsLast;
        private bool[] _buttonsLast;

        private void tmrJSUpdate_Tick(object sender, EventArgs e)
        {
            if (_jst != null && _axisLast!=null)
            {
                lock (Jslock)
                {
                    _jst.UpdateStatus();
                    for (int i = 0; i < _jst.Axis.Length; i++)
                    {
                        if (_jst.Axis[i] != _axisLast[i])
                        {
                            if (_curAxis != null)
                            {
                                _curAxis.ID = (i + 1);
                            }
                        }
                        _axisLast[i] = _jst.Axis[i];

                    }

                    for (int i = 0; i < _jst.Buttons.Length; i++)
                    {
                         
                        if (_jst.Buttons[i] != _buttonsLast[i])
                        {
                            if (_curButton!=null)
                            {
                                _curButton.ID = (i + 1);
                            }
                        }

                        _buttonsLast[i] = _jst.Buttons[i];

                    }

                    for (int i = 0; i < _jst.Dpads.Length; i++)
                    {
                        if (_jst.Dpads[i] != _dPadsLast[i])
                        {
                           
                        }

                        _dPadsLast[i] = _jst.Dpads[i];
                        
                    }
                }

            }
        }

        private void btnCenterAxes_Click(object sender, EventArgs e)
        {
            CenterAxes();
            MessageBox.Show(this, LocRm.GetString("AxesCentered"));
        }

        private void CenterAxes()
        {
            
        }

        private void jaxis1_Load(object sender, EventArgs e)
        {

        }

        private void btnFeatureSet_Click(object sender, EventArgs e)
        {
            if (MainForm.Group != "Admin")
            {
                var ap = EncDec.DecryptData(MainForm.Conf.Permissions.First(q => q.name == "Admin").password,MainForm.Conf.EncryptCode);

                var p = new Prompt(LocRm.GetString("AdminPassword"), "", true);
                p.ShowDialog(this);
                string pwd = p.Val;
                p.Dispose();
                if (pwd != ap)
                {
                    MessageBox.Show(this, LocRm.GetString("PasswordIncorrect"));
                    return;
                }
                
               
            }
            var f = new Features();
            f.ShowDialog(this);
            f.Dispose();
            MainClass.RenderResources();
        }

        private void chkCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ddlTalkMic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string _lastPath = "";
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "All Files (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                       
                    }
                }
            }
        }

        private void btnChooseFile2_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "All Files (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                      
                    }
                }
            }
        }

        private void jbutton4_Load(object sender, EventArgs e)
        {

        }

        private void jbutton1_Load(object sender, EventArgs e)
        {

        }

        private void chkEnableIPv6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/plugins.aspx");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.LoadPlugins();
            ListPlugins();
        }

        private void ListPlugins()
        {
            lbPlugins.Items.Clear();
            
            foreach (String plugin in MainForm.Plugins)
            {
                string name = plugin.Substring(plugin.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                name = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal));
                lbPlugins.Items.Add(name);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            string f = GetFolder(MainForm.Conf.Archive);
            if (f != "")
            {
            }
        }

        private string GetFolder(string initialPath)
        {
            string f = "";
            if (!string.IsNullOrEmpty(initialPath))
            {
                try
                {
                    Fsd.InitialDirectory = initialPath;
                }
                catch
                {

                }
            }


            if (Fsd.ShowDialog(Handle))
            {
                f = Fsd.FileName;
                if (!f.EndsWith(@"\"))
                    f += @"\";
            }
            return f;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void chkUseiSpy_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void btnTestSMTP_Click(object sender, EventArgs e)
        {
            SaveSMTPSettings();
             var p = new Prompt(LocRm.GetString("TestMailTo"), MainForm.Conf.SMTPFromAddress);
            if (p.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show(this, Mailer.Send(p.Val, LocRm.GetString("test"),
                    LocRm.GetString("BYSEC MessageTest"))
                    ? LocRm.GetString("MessageSent")
                    : LocRm.GetString("FailedCheckLog"));
            }
        }

        private void SaveSMTPSettings()
        {


            
        }

        private void chkPasswordProtectSettings_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void btnCert_Click(object sender, EventArgs e)
        {
            var c = MainForm.Conf.SSLEnabled;
            var ssl = new SSLConfig();
            ssl.ShowDialog(this);
            ssl.Dispose();
            SetSSLText();
            if (MainForm.Conf.SSLEnabled != c)
            {
                MainClass.ConnectServices(false);

            }
            
        }

        private void SetSSLText() 
        {
            

        }

        private void ddlPlayback_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mediaDirectoryEditor1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm.InstanceReference.RunStorageManagement(true);
        }

        private void ddlStartupMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}