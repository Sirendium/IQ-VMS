using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Antiufo.Controls;
using FFmpeg.AutoGen;
using iSpyApplication.Cloud;
using iSpyApplication.Controls;
using iSpyApplication.Joystick;
using iSpyApplication.Onvif;
using iSpyApplication.Properties;
using iSpyApplication.Server;
using iSpyApplication.Sources;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Audio.talk;
using iSpyApplication.Utilities;
using Microsoft.Win32;
using NATUPNPLib;
using NAudio.Wave;
using NETWORKLIST;
using SharpDX.DirectInput;
using PictureBox = iSpyApplication.Controls.PictureBox;
using Timer = System.Timers.Timer;

namespace iSpyApplication
{
    /// <summary>
    ///     Summary description for MainForm
    /// </summary>
    public partial class MainForm : Form, INetworkListManagerEvents
    {
        public const string VLCx86 = "http://www.videolan.org/vlc/download-windows.html";
        public const string VLCx64 = "http://download.videolan.org/pub/videolan/vlc/last/win64/";

        public const string Website = "http://www.ispyconnect.com";
        public const string ContentSource = Website;
        public static bool NeedsSync;
        private static DateTime _needsMediaRefresh = DateTime.MinValue;
        //private static Player _player = null;

        public static DateTime LastAlert = DateTime.MinValue;

        public static MainForm InstanceReference;

        public static bool VLCRepeatAll;
        public static bool NeedsMediaRebuild = false;
        public static int MediaPanelPage;
        public static bool LoopBack;
        public static string NL = Environment.NewLine;
        public static Font Drawfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font DrawfontBig = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font DrawfontMed = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font Iconfont = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Brush IconBrush = new SolidBrush(Color.White);
        public static Brush IconBrushOff = new SolidBrush(Color.FromArgb(64, 255, 255, 255));
        public static Brush IconBrushActive = new SolidBrush(Color.Red);
        public static Brush OverlayBrush = new SolidBrush(Color.White);
        public static int ThreadKillDelay = 10000;
        public static SolidBrush CameraDrawBrush = new SolidBrush(Color.White);
        public static Pen CameraLine = new Pen(Color.Green, 2);
        public static Pen CameraNav = new Pen(Color.White, 1);
        public static Brush RecordBrush = new SolidBrush(Color.Red);
        public static Brush OverlayBackgroundBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        
        public static string Identifier;
        public static DataTable IPTABLE;
        public static bool IPLISTED = true;
        public static bool IPRTSP = false, IPHTTP = true;
        public static string IPADDR = "";
        public static string IPCHANNEL = "0";
        public static string IPMODEL = "";
        public static string IPUN = "";
        public static string IPPORTS = "80,8080";
        public static int IPPORT = 80;
        public static string IPPASS = "";
        public static string IPTYPE = "";
        public static int Affiliateid = 0;
        public static string EmailAddress = "", MobileNumber = "";
        public static string Group="";
        public static float CpuUsage, CpuTotal;
        public static bool HighCPU;
        public static int RecordingThreads;
        public static List<string> Plugins = new List<string>();
        public static bool NeedsResourceUpdate;
        private static readonly List<FilePreview> Masterfilelist = new List<FilePreview>();

        public static EncoderParameters EncoderParams;
        public static bool ShuttingDown = false;
        public static string Webserver = Website;
        public static string WebserverSecure = Website.Replace("http:", "https:");


        public static Rectangle RPower = new Rectangle(94, 3, 16, 16);
        public static Rectangle RPowerOn = new Rectangle(94, 43, 16, 16);
        public static Rectangle RPowerOff = new Rectangle(94, 83, 16, 16);
        public static Rectangle RAdd = new Rectangle(127, 3, 16, 16);
        public static Rectangle RAddOff = new Rectangle(127, 83, 16, 16);
        public static Rectangle REdit = new Rectangle(3, 2, 16, 16);
        public static Rectangle REditOff = new Rectangle(3, 82, 16, 16);
        public static Rectangle RHold = new Rectangle(255, 2, 16, 16);
        public static Rectangle RHoldOn = new Rectangle(284, 42, 16, 16);
        public static Rectangle RHoldOff = new Rectangle(284, 82, 16, 16);
        public static Rectangle RRecord = new Rectangle(188,2,16,16);
        public static Rectangle RRecordOn = new Rectangle(188, 42, 16, 16);
        public static Rectangle RRecordOff = new Rectangle(188, 82, 16, 16);
        public static Rectangle RNext = new Rectangle(65, 3, 16, 16);
        public static Rectangle RNextOff = new Rectangle(65, 82, 16, 16);
        public static Rectangle RGrab = new Rectangle(157,2,16,16);
        public static Rectangle RGrabOff = new Rectangle(157, 82, 16, 16);
        public static Rectangle RTalk = new Rectangle(313, 2, 16,16);
        public static Rectangle RTalkOn = new Rectangle(313, 42, 16, 16);
        public static Rectangle RTalkOff = new Rectangle(313, 82, 16, 16);
        public static Rectangle RFiles = new Rectangle(223,3,16,16);
        public static Rectangle RFilesOff = new Rectangle(223, 83, 16, 16);
        public static Rectangle RListen = new Rectangle(347,2,16,16);
        public static Rectangle RListenOn = new Rectangle(380,43,16,16);
        public static Rectangle RListenOff = new Rectangle(347, 83, 16, 16);
        public static Rectangle RWeb = new Rectangle(411, 3, 16, 16);
        public static Rectangle RWebOff = new Rectangle(411, 83, 16, 16);
        public static Rectangle RText = new Rectangle(443, 3, 16, 16);
        public static Rectangle RTextOff = new Rectangle(443, 83, 16, 16);
        public static Rectangle RFolder = new Rectangle(473, 3, 16, 16);
        public static Rectangle RFolderOff = new Rectangle(473, 83, 16, 16);

        private static List<string> _tags;
        public static bool CustomWebserver;
        public static List<string> Tags
        {
            get
            {
                if (_tags != null)
                    return _tags;
                _tags = new List<string>();
                if (!string.IsNullOrEmpty(Conf.Tags))
                {
                    var l = Conf.Tags.Split(',').ToList();
                    foreach (var t in l)
                    {
                        if (!string.IsNullOrEmpty(t))
                        {
                            var s = t.Trim();
                            if (!s.StartsWith("{"))
                                s = "{" + s;
                            if (!s.EndsWith("}"))
                                s = s + "}";
                            _tags.Add(s.ToUpper(CultureInfo.InvariantCulture));
                        }
                    }
                }
                return _tags;
            }
            set { _tags = value; }
        } 
        public ISpyControl LastFocussedControl = null;

        internal static LocalServer MWS;

        public static string PurchaseLink = "http://www.ispyconnect.com/astore.aspx";
        private static int _storageCounter;
        private static Timer _rescanIPTimer, _tmrJoystick;
        
        private static string _counters = "";
        private static readonly Random Random = new Random();
        private static ViewController _vc;
        private static int _pingCounter;
        private static ImageCodecInfo _encoder;
        private static bool _needsDelete = false;
        public LayoutPanel _pnlCameras;
        private MediaPanelControl mediaPanelControl1;
        internal MediaPanel flowPreview;
        private SplitContainer splitContainer1;
        private Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton _toolStripDropDownButton2;
        private ToolStripMenuItem _localCameraToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4; 
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton5;
        private Panel panel3;
        private ToolStripStatusLabel tsslMonitor;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveLayoutToolStripMenuItem;
        private ToolStripMenuItem openLayoutToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem filesToolStripMenuItem;
        private ToolStripMenuItem fullScreenToolStripMenuItem1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem switchONToolStripMenuItem;
        private ToolStripMenuItem switchOFFToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem grid4XToolStripMenuItem;
        private ToolStripMenuItem grid8XToolStripMenuItem;
        private ToolStripMenuItem grid16XToolStripMenuItem;
        private ToolStripMenuItem aboutOurToolStripMenuItem;
        private ToolStripButton toolStripButton1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private ToolStripMenuItem recordListToolStripMenuItem;
        private ToolStripMenuItem oNToolStripMenuItem1;
        private ToolStripMenuItem oFFToolStripMenuItem1;
        private ToolStripMenuItem licenseToolStripMenuItem;
        private ToolStripMenuItem registerToolStripMenuItem;
        private ToolStripMenuItem buyLicenseToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private static string _browser = string.Empty;
        

        public static void AddObject(object o)
        {
            var c = o as objectsCamera;
            if (c != null)
            {
                c.settings.order = MaxOrderIndex;
                _cameras.Add(c);
                
                MaxOrderIndex++;
            }
            var m = o as objectsMicrophone;
            if (m != null)
            {
                m.settings.order = MaxOrderIndex;
                _microphones.Add(m);
                MaxOrderIndex++;
            }
            var f = o as objectsFloorplan;
            if (f != null)
            {
                f.order = MaxOrderIndex;
                _floorplans.Add(f);
                MaxOrderIndex++;

            }
            var a = o as objectsActionsEntry;
            if (a != null)
                _actions.Add(a);
            var oc = o as objectsCommand;
            if(oc!=null)
                _remotecommands.Add(oc);
            LayoutPanel.NeedsRedraw = true;
        }

        private static List<PTZSettings2Camera> _ptzs;
        private static List<ManufacturersManufacturer> _sources;
        private static IPAddress[] _ipv4Addresses, _ipv6Addresses;
        
        private readonly List<float> _cpuAverages = new List<float>();
        private readonly int _mCookie = -1;
        private readonly IConnectionPoint _mIcp;
        private static readonly object ThreadLock = new object();

        private readonly FolderSelectDialog _fbdSaveTo = new FolderSelectDialog
                                                        {
                                                            Title = "Select a folder to copy the file to"
                                                        };


        public object ContextTarget;
        //internal Player Player;
        internal PlayerVLC PlayerVLC;
        public McRemoteControlManager.RemoteControlDevice RemoteManager;
        public bool SilentStartup;
        
        internal CameraWindow TalkCamera;
        private ToolStripMenuItem _addCameraToolStripMenuItem;
        private bool _closing;
        private PerformanceCounter _cpuCounter, _cputotalCounter;
        private ToolStripMenuItem _deleteToolStripMenuItem;
        private ToolStripMenuItem _editToolStripMenuItem;
        private FileSystemWatcher _fsw;
        private Timer _houseKeepingTimer;
        private static string _lastPath = Program.AppDataPath;
        private static string _currentFileName = "";
        private PersistWindowState _mWindowState;
        private PerformanceCounter _pcMem;
        private FormWindowState _previousWindowState = FormWindowState.Normal;
        private PTZTool _ptzTool;
        private ToolStripMenuItem _recordNowToolStripMenuItem;
        private ToolStripMenuItem _showFilesToolStripMenuItem;
        private bool _shuttingDown;
        private string _startCommand = "";
        private Thread _storageThread;
        private ToolStripMenuItem _takePhotoToolStripMenuItem;
        private IAudioSource _talkSource;
        private ITalkTarget _talkTarget;
        private Thread _updateChecker;
        private Timer _updateTimer;
        private IContainer components;
        private ContextMenuStrip ctxtMainForm;
        private ContextMenuStrip ctxtMnu;
        private ToolStripMenuItem fullScreenToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolTip toolTip1;
        private ToolStripStatusLabel tsslMediaInfo;
        private ToolStripMenuItem switchToolStripMenuItem;
        private ToolStripMenuItem onToolStripMenuItem;
        private ToolStripMenuItem offToolStripMenuItem;

        public MainForm(bool silent, string command)
        {
            if (Conf.StartupForm != "iSpy")
            {
                SilentStartup = true;
            }

            SilentStartup = SilentStartup || silent || Conf.Enable_Password_Protect || Conf.StartupMode == 1;

            //need to wrap initialize component
            if (SilentStartup)
            {
                ShowInTaskbar = false;
                ShowIcon = false;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                switch (Conf.StartupMode)
                {
                    case 0:
                        _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
                        break;
                    case 2:
                        WindowState = FormWindowState.Maximized;
                        break;
                    case 3:
                        WindowState = FormWindowState.Maximized;
                        FormBorderStyle = FormBorderStyle.None;
                        break;
                }
            }

            InitializeComponent();

            if (!SilentStartup)
            {
                if (Conf.StartupMode == 0)
                    _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
            }


            RenderResources();

            _startCommand = command;

            Windows7Renderer r = Windows7Renderer.Instance;
            
            statusStrip1.Renderer = r;

            _pnlCameras.BackColor = Color.DimGray;

            RemoteManager = new McRemoteControlManager.RemoteControlDevice();
            RemoteManager.ButtonPressed += RemoteManagerButtonPressed;

            SetPriority();
            Arrange(false);


            _jst = new JoystickDevice();
            bool jsactive = false;
            string[] sticks = _jst.FindJoysticks();
            foreach (string js in sticks)
            {
                string[] nameid = js.Split('|');
                if (nameid[1] == Conf.Joystick.id)
                {
                    Guid g = Guid.Parse(nameid[1]);
                    jsactive = _jst.AcquireJoystick(g);
                }
            }

            if (!jsactive)
            {
                _jst.ReleaseJoystick();
                _jst = null;
            }
            else
            {
                _tmrJoystick = new Timer(100);
                _tmrJoystick.Elapsed += TmrJoystickElapsed;
                _tmrJoystick.Start();
            }
            try
            {
                INetworkListManager mNlm = new NetworkListManager();
                var icpc = (IConnectionPointContainer) mNlm;
                //similar event subscription can be used for INetworkEvents and INetworkConnectionEvents
                Guid tempGuid = typeof (INetworkListManagerEvents).GUID;
                icpc.FindConnectionPoint(ref tempGuid, out _mIcp);
                if (_mIcp != null)
                {
                    _mIcp.Advise(this, out _mCookie);
                }
            }
            catch (Exception)
            {
                _mIcp = null;
            }
            InstanceReference = this;

            try
            {
                Discovery.FindDevices();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static DateTime NeedsMediaRefresh
        {
            get { return _needsMediaRefresh; }
            set
            {
                //only store first recorded or reset value
                if (value == DateTime.MinValue)
                    _needsMediaRefresh = value;
                else
                {
                    if (_needsMediaRefresh == DateTime.MinValue)
                        _needsMediaRefresh = value;
                }
            }
        }

        public static ReadOnlyCollection<FilePreview> MasterFileList => Masterfilelist.AsReadOnly();


        public bool StorageThreadRunning
        {
            get
            {
                lock (ThreadLock)
                {
                    try
                    {
                        return _storageThread != null && !_storageThread.Join(TimeSpan.Zero);
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }

        public static int ProductID => Program.Platform != "x86" ? 19 : 11;

        private static string DefaultBrowser
        {
            get
            {
                if (!string.IsNullOrEmpty(_browser))
                    return _browser;

                _browser = string.Empty;
                RegistryKey key = null;
                try
                {
                    key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //trim off quotes
                    if (key != null) _browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!_browser.EndsWith(".exe"))
                    {
                        _browser = _browser.Substring(0, _browser.LastIndexOf(".exe", StringComparison.Ordinal) + 4);
                    }
                }
                finally
                {
                    key?.Close();
                }
                return _browser;
            }
        }

        public void ConnectivityChanged(NLM_CONNECTIVITY newConnectivity)
        {
            var i = (int) newConnectivity;
            if (!WsWrapper.WebsiteLive)
            {
                if (newConnectivity != NLM_CONNECTIVITY.NLM_CONNECTIVITY_DISCONNECTED)
                {
                    if ((i & (int) NLM_CONNECTIVITY.NLM_CONNECTIVITY_IPV4_INTERNET) != 0 ||
                        ((int) newConnectivity & (int) NLM_CONNECTIVITY.NLM_CONNECTIVITY_IPV6_INTERNET) != 0)
                    {
                        if (!WsWrapper.WebsiteLive)
                        {
                            WsWrapper.LastLiveCheck = Helper.Now.AddMinutes(-5);
                        }
                    }
                }
            }
        }

        public static void MasterFileAdd(FilePreview fp)
        {
            lock (ThreadLock)
            {
                Masterfilelist.Add(fp);
            }
            var wss = MWS.WebSocketServer;
            if (wss != null)
                wss.SendToAll("new events|" + fp.Name);
        }

        public static void MasterFileRemoveAll(int objecttypeid, int objectid)
        {
            lock (ThreadLock)
            {
                Masterfilelist.RemoveAll(p => p.ObjectTypeId == objecttypeid && p.ObjectId == objectid);
            }
        }

        public static void MasterFileRemove(string filename)
        {
            lock (ThreadLock)
            {
                Masterfilelist.RemoveAll(p => p.Filename == filename);
            }
        }

        private bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            var formTopLeft = new Point(form.Left, form.Top);
            //hack for maximised window
            if (form.WindowState == FormWindowState.Maximized)
            {
                formTopLeft.X += 8;
                formTopLeft.Y += 8;
            }

            return screens.Any(screen => screen.WorkingArea.Contains(formTopLeft));
        }

        protected override void WndProc(ref Message message)
        {
            RemoteManager.ProcessMessage(message);
            base.WndProc(ref message);
        }


        private void RemoteManagerButtonPressed(object sender, McRemoteControlManager.RemoteControlEventArgs e)
        {
            ProcessKey(e.Button.ToString().ToLower());
        }

        public static void SetPriority()
        {
            switch (Conf.Priority)
            {
                case 1:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case 2:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
                    break;
                case 3:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                    break;
                case 4:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                    break;
            }
        }


        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               

                components?.Dispose();
                _mWindowState?.Dispose();

                Drawfont.Dispose();
                _updateTimer?.Dispose();
                _houseKeepingTimer?.Dispose();
                //sometimes hangs??
                //if (_fsw != null)
                //    _fsw.Dispose();
            }
            base.Dispose(disposing);
        }

        // Close the main form
        private void ExitFileItemClick(object sender, EventArgs e)
        {
           
        }

        // On "Help->About"
        private void AboutHelpItemClick(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog(this);
            form.Dispose();
        }

        private void VolumeControlDoubleClick(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(sender);
        }

        private void FloorPlanDoubleClick(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(sender);
        }

        private static Enums.LayoutMode _layoutMode;
        public static Enums.LayoutMode LayoutMode
        {
            get
            {
                return _layoutMode;
            }
            set
            {
                _layoutMode = value;
                
                Conf.ArrangeMode = (_layoutMode == Enums.LayoutMode.AutoGrid ? 1 : 0);
                
            }
        }

        public static bool LockLayout => Conf.LockLayout || _layoutMode == Enums.LayoutMode.AutoGrid;

        private static void AddPlugin(FileInfo dll)
        {
            try
            {
                Assembly plugin = Assembly.LoadFrom(dll.FullName);
                object ins = null;
                try
                {
                    ins = plugin.CreateInstance("Plugins.Main", true);
                }
                catch
                {
                    // ignored
                }
                if (ins != null)
                {
                    Logger.LogMessage("Added: " + dll.FullName);
                    Plugins.Add(dll.FullName);
                }
            }
            catch // (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }


        public void Play(string filename, int objectId, string displayName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Play(filename,objectId,displayName)));
                return;
            }
            if (PlayerVLC == null)
            {
                try
                {
                    PlayerVLC = new PlayerVLC(displayName, this);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex,"Play with VLC");
                    Conf.PlaybackMode = (int) Enums.PlaybackMode.Default;
                    MessageBox.Show(
                        "Could not start VLC. Check you have the right version installed. Using default player instead");
                    return;
                }

                PlayerVLC.Show(this);
                PlayerVLC.Closed += PlayerClosed;
                PlayerVLC.Activate();
                PlayerVLC.BringToFront();
                PlayerVLC.Owner = this;
            }

            PlayerVLC.ObjectID = objectId;
            PlayerVLC.Play(filename, displayName);

        }

        private void PlayerClosed(object sender, EventArgs e)
        {
            //_player = null;
            PlayerVLC = null;
        }

       

        public void MainFormLoad(object sender, EventArgs e)
        {
            MainInit();
            splitContainer1.Panel2.Hide();
    
            Intro_Box f = new Intro_Box();
            f.Show();
          
         
        }

        private void MainInit()
        {
            UISync.Init(this);
            Logger.InitLogging();
            try
            {
                File.WriteAllText(Program.AppDataPath + "exit.txt", "RUNNING");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            try
            {
                ffmpeg.avdevice_register_all();
                Program.SetFfmpegLogging();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (!SilentStartup)
            {
                switch (Conf.StartupMode)
                {
                    case 0:
                        break;
                    case 2:
                        break;
                    case 3:
                        WinApi.SetWinFullScreen(Handle);
                        break;
                }
            }

            mediaPanelControl1.MainClass = this;
            EncoderParams = new EncoderParameters(1)
                            {
                                Param =
                                {
                                    [0] =
                                        new EncoderParameter(
                                        System.Drawing.Imaging.Encoder.Quality,
                                        Conf.JPEGQuality)
                                }
                            };

            //this initializes the port mapping collection
            IStaticPortMappingCollection map = NATControl.Mappings;
            if (string.IsNullOrEmpty(Conf.MediaDirectory) || Conf.MediaDirectory == "NotSet")
            {
                Conf.MediaDirectory = Program.AppDataPath + @"WebServerRoot\Media\";
            }

            if (Conf.MediaDirectories == null || Conf.MediaDirectories.Length == 0)
            {
                Conf.MediaDirectories = new[]
                                        {
                                            new configurationDirectory
                                            {
                                                Entry = Conf.MediaDirectory,
                                                DeleteFilesOlderThanDays =
                                                    Conf.DeleteFilesOlderThanDays,
                                                Enable_Storage_Management =
                                                    Conf.Enable_Storage_Management,
                                                MaxMediaFolderSizeMB = Conf.MaxMediaFolderSizeMB,
                                                StopSavingOnStorageLimit =
                                                    Conf.StopSavingOnStorageLimit,
                                                ID = 0
                                            }
                                        };
            }
            else
            {
                if (Conf.MediaDirectories.First().Entry == "NotSet")
                {
                    Conf.MediaDirectories = new[]
                                            {
                                                new configurationDirectory
                                                {
                                                    Entry = Conf.MediaDirectory,
                                                    DeleteFilesOlderThanDays =
                                                        Conf.DeleteFilesOlderThanDays,
                                                    Enable_Storage_Management =
                                                        Conf.Enable_Storage_Management,
                                                    MaxMediaFolderSizeMB =
                                                        Conf.MaxMediaFolderSizeMB,
                                                    StopSavingOnStorageLimit =
                                                        Conf.StopSavingOnStorageLimit,
                                                    ID = 0
                                                }
                                            };
                }
            }

            //reset stop saving flag
            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                d.StopSavingFlag = false;
            }

            if (!Directory.Exists(Conf.MediaDirectories[0].Entry))
            {
                string notfound = Conf.MediaDirectories[0].Entry;
                Logger.LogError("Media directory could not be found (" + notfound + ") - reset it to " +
                               Program.AppDataPath + @"WebServerRoot\Media\" + " in settings if it doesn't attach.");
            }

            if (!VlcHelper.VlcInstalled)
            {
                Logger.LogWarningToFile(
                    "VLC not installed - install VLC (" + Program.Platform + ") for additional connectivity.");
                if (Program.Platform == "x64")
                {
                    Logger.LogWarningToFile(
                        "VLC64  must be unzipped so the dll files and folders including libvlc.dll and the plugins folder are in " +
                        Program.AppPath + "VLC64\\");
                    Logger.LogWarningToFile("Download: <a href=\""+VLCx64+"\">"+VLCx64+"</a>");
                }
                else
                    Logger.LogWarningToFile("Download: <a href=\"" + VLCx86 + "\">" + VLCx86 + "</a>");
            }
            else
            {
                Version v = VlcHelper.VlcVersion;
                if (v.CompareTo(VlcHelper.VMin) < 0)
                {
                    Logger.LogWarningToFile(
                        "Old VLC installed - update VLC (" + Program.Platform + ") for additional connectivity.");
                }
                else
                {
                    if (v.CompareTo(new Version(2, 0, 2)) == 0)
                    {
                        Logger.LogWarningToFile(
                            "VLC v2.0.2 detected - there are known issues with this version of VLC (HTTP streaming is broken for a lot of cameras) - if you are having problems with VLC connectivity we recommend you install v2.0.1 ( http://download.videolan.org/pub/videolan/vlc/2.0.1/ ) or the latest (if available).");
                    }
                }
            }


            _fsw = new FileSystemWatcher
                   {
                       Path = Program.AppDataPath,
                       IncludeSubdirectories = false,
                       Filter = "external_command.txt",
                       NotifyFilter = NotifyFilters.LastWrite
                   };
            _fsw.Changed += FswChanged;
            _fsw.EnableRaisingEvents = true;

          


           
            Identifier = Guid.NewGuid().ToString();
            MWS = new LocalServer
                  {
                      ServerRoot = Program.AppDataPath + @"WebServerRoot\"
                  };

#if DEBUG
            MWS.ServerRoot = Program.AppPath + @"WebServerRoot\";
#endif

            if (Conf.Monitor)
            {
                Process[] w = Process.GetProcessesByName("ispymonitor");
                if (w.Length == 0)
                {
                    try
                    {
                        var si = new ProcessStartInfo(Program.AppPath + "/ispymonitor.exe", "ispy");
                        Process.Start(si);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            SetBackground();

            
            statusStrip1.Visible = Conf.ShowStatus && Helper.HasFeature(Enums.Features.View_Status_Bar);
        

            if (SilentStartup)
            {
                WindowState = FormWindowState.Minimized;
            }

            if (Conf.Password_Protect_Startup)
            {
                _locked = true;
                WindowState = FormWindowState.Minimized;
            }

            if (Conf.Fullscreen && !SilentStartup && !_locked)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                WinApi.SetWinFullScreen(Handle);
            }
            

           
         
          
           
            TopMost = Conf.AlwaysOnTop;

            Iconfont = new Font(FontFamily.GenericSansSerif, Conf.BigButtons ? 22 : 15, FontStyle.Bold,
                GraphicsUnit.Pixel);
            double dOpacity;
            Double.TryParse(Conf.Opacity.ToString(CultureInfo.InvariantCulture), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dOpacity);
            Opacity = dOpacity/100.0;


            if (Conf.ServerName == "NotSet")
            {
                Conf.ServerName = SystemInformation.ComputerName;
            }

         

            _updateTimer = new Timer(200);
            _updateTimer.Elapsed += UpdateTimerElapsed;
            _updateTimer.AutoReset = true;
            _updateTimer.SynchronizingObject = this;
            //GC.KeepAlive(_updateTimer);

            _houseKeepingTimer = new Timer(1000);
            _houseKeepingTimer.Elapsed += HouseKeepingTimerElapsed;
            _houseKeepingTimer.AutoReset = true;
            _houseKeepingTimer.SynchronizingObject = this;
            //GC.KeepAlive(_houseKeepingTimer);

            //load plugins
            LoadPlugins();

            

            NetworkChange.NetworkAddressChanged += NetworkChangeNetworkAddressChanged;
          
            ShowHideMediaPane();
            if (!string.IsNullOrEmpty(Conf.MediaPanelSize))
            {
                string[] dd = Conf.MediaPanelSize.Split('x');
                int d1 = Convert.ToInt32(dd[0]);
                int d2 = Convert.ToInt32(dd[1]);
                try
                {
                    splitContainer1.SplitterDistance = d1;
                  
                }
                catch
                {
                    // ignored
                }
            }
            //load in object list

            if (_startCommand.Trim().StartsWith("open"))
            {
                ParseCommand(_startCommand);
                _startCommand = "";
            }
            else
            {
                if (!File.Exists(Program.AppDataPath + @"XML\objects.xml"))
                {
                    File.Copy(Program.AppPath + @"XML\objects.xml", Program.AppDataPath + @"XML\objects.xml");
                }
                ParseCommand("open " + Program.AppDataPath + @"XML\objects.xml");
            }
            if (_startCommand != "")
            {
                ParseCommand(_startCommand);
            }

            StopAndStartServer();

            if (_mWindowState == null)
            {
                _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
            }

            if (Conf.Enabled_ShowGettingStarted)
                

            if (File.Exists(Program.AppDataPath + "custom.txt"))
            {
                string[] cfg =
                    File.ReadAllText(Program.AppDataPath + "custom.txt").Split(Environment.NewLine.ToCharArray());
                bool setSecure = false;
                foreach (string s in cfg)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string[] nv = s.Split('=');

                        if (nv.Length > 1)
                        {
                            switch (nv[0].ToLower().Trim())
                            {
                                case "business":
                                    Conf.Vendor = nv[1].Trim();
                                    break;
                                case "link":
                                    PurchaseLink = nv[1].Trim();
                                    break;
                                case "manufacturer":
                                    IPTYPE = Conf.DefaultManufacturer = nv[1].Trim();
                                    break;
                                case "model":
                                    IPMODEL = nv[1].Trim();
                                    break;
                                case "affiliateid":
                                case "affiliate id":
                                case "aid":
                                    int aid;
                                    if (Int32.TryParse(nv[1].Trim(), out aid))
                                    {
                                        Affiliateid = aid;
                                    }
                                    break;
                                case "tags":
                                    if (string.IsNullOrEmpty(Conf.Tags))
                                        Conf.Tags = nv[1].Trim();
                                    break;
                                case "featureset":
                                    //only want to set this on install (allow them to modify)
                                    if (Conf.FirstRun)
                                    {
                                        int featureset;
                                        if (Int32.TryParse(nv[1].Trim(), out featureset))
                                        {
                                            Conf.FeatureSet = featureset;
                                        }
                                    }
                                    break;
                                case "permissions":
                                    //only want to set this on install (allow them to modify)
                                    if (Conf.FirstRun)
                                    {
                                        var groups = nv[1].Trim().Split('|');
                                        var l = new List<configurationGroup>();
                                        foreach (var g in groups)
                                        {
                                            if (!string.IsNullOrEmpty(g))
                                            {
                                                var g2 = g.Split(',');
                                                if (g2.Length >= 3)
                                                {
                                                    if (!string.IsNullOrEmpty(g2[0]))
                                                    {
                                                        int perm;
                                                        if (int.TryParse(g2[2], out perm))
                                                        {
                                                            l.Add(new configurationGroup
                                                                  {
                                                                      featureset = perm,
                                                                      name = g2[0],
                                                                      password =
                                                                          EncDec.EncryptData(g2[1],
                                                                              Conf.EncryptCode)
                                                                  });
                                                        }
                                                    }
                                                }
                                            }   
                                        }
                                        if (l.FirstOrDefault(p => p.name.ToLower() == "admin") == null)
                                        {
                                            l.Add(new configurationGroup{
                                                      featureset = 1,
                                                      name = "Admin",
                                                      password = ""
                                                  });
                                        }
                                        if (l.Count>0)
                                            Conf.Permissions = l.ToArray();

                                    }
                                    break;
                                case "webserver":
                                    string ws = nv[1].Trim().Trim('/');
                                    if (!string.IsNullOrEmpty(ws))
                                    {
                                        Webserver = ws;
                                        if (!setSecure)
                                            WebserverSecure = Webserver;
                                        CustomWebserver = true;
                                    }
                                    break;
                                case "webserversecure":
                                    WebserverSecure = nv[1].Trim().Trim('/');
                                    setSecure = true;
                                    break;
                                case "recordondetect":
                                    bool defaultRecordOnDetect;
                                    if (bool.TryParse(nv[1].Trim(), out defaultRecordOnDetect))
                                        Conf.DefaultRecordOnDetect = defaultRecordOnDetect;
                                    break;
                                case "recordonalert":
                                    bool defaultRecordOnAlert;
                                    if (bool.TryParse(nv[1].Trim(), out defaultRecordOnAlert))
                                        Conf.DefaultRecordOnAlert = defaultRecordOnAlert;
                                    break;
                            }
                        }
                    }
                }
                Conf.FirstRun = false;
                Logger.LogMessage("Webserver: " + Webserver);

                string logo = Program.AppDataPath + "logo.jpg";
                if (!File.Exists(logo))
                    logo = Program.AppDataPath + "logo.png";

                if (File.Exists(logo))
                {
                    try
                    {
                            
                        Image bmp = Image.FromFile(logo);
                        var pb = new PictureBox {Image = bmp};
                        pb.Width = pb.Image.Width;
                        pb.Height = pb.Image.Height;

                        pb.Left = _pnlCameras.Width/2 - pb.Width/2;
                        pb.Top = _pnlCameras.Height/2 - pb.Height/2;

                        _pnlCameras.Controls.Add(pb);
                        
                        _pnlCameras.BrandedImage = pb;
                        _pnlCameras.Width -=52;
                        _pnlCameras.Height += 50;
                        


                        }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
                _lastClicked = _pnlCameras;
            }

            LoadCommands();
            if (!SilentStartup && Conf.ViewController)
            {
                ShowViewController();
              
            }

           

            if (Conf.ShowPTZController && !SilentStartup)
                ShowHidePTZTool();


            ListGridViews();

            Conf.RunTimes++;

            try
            {
                _cputotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_total", true);
                _cpuCounter = new PerformanceCounter("Process", "% Processor Time",
                    Process.GetCurrentProcess().ProcessName, true);
                try
                {
                    _pcMem = new PerformanceCounter("Process", "Working Set - Private",
                        Process.GetCurrentProcess().ProcessName, true);
                }
                catch
                {
                    try
                    {
                        _pcMem = new PerformanceCounter("Memory", "Available MBytes");
                    }
                    catch (Exception ex2)
                    {
                        Logger.LogException(ex2);
                        _pcMem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                _cputotalCounter = null;
            }


            if (Conf.StartupForm != "iSpy")
            {
                ShowGridView(Conf.StartupForm);
                
            }

            foreach (var cg in Conf.GridViews)
            {
                if (cg.ShowAtStartup)
                {
                    ShowGridView(cg.name);
                }
            }

            var t = new Thread(()=>ConnectServices()) {IsBackground = true};
            t.Start();

            _updateTimer.Start();
            _houseKeepingTimer.Start();
        }

        internal void ShowGridView(string name)
        {
            configurationGrid cg = Conf.GridViews.FirstOrDefault(p => p.name == name);
            if (cg != null)
            {
                for(int i=0;i<_views.Count;i++)
                {
                    GridView g = _views[i];
                    if (g != null && !g.IsDisposed)
                    {
                        if (g.Cg == cg)
                        {
                            g.BringToFront();
                            g.Focus();
                            return;
                        }
                    }
                    else
                    {
                        _views.RemoveAt(i);
                        i--;
                    }
                        
                }
                var gv = new GridView(this, ref cg);
                gv.Show();
                _views.Add(gv);
            }
        }
        private readonly List<GridView> _views = new List<GridView>();

        public static void LoadPlugins()
        {
            Plugins = new List<string>();
            if (Directory.Exists(Program.AppPath + "Plugins"))
            {
                var plugindir = new DirectoryInfo(Program.AppPath + "Plugins");
                Logger.LogMessage("Checking Plugins...");
                foreach (FileInfo dll in plugindir.GetFiles("*.dll"))
                {
                    AddPlugin(dll);
                }
                foreach (DirectoryInfo d in plugindir.GetDirectories())
                {
                    Logger.LogMessage(d.Name);
                    foreach (FileInfo dll in d.GetFiles("*.dll"))
                    {
                        AddPlugin(dll);
                    }
                }
            }
        }

        private static void NetworkChangeNetworkAddressChanged(object sender, EventArgs e)
        {
            //schedule update check for a few seconds as a network change involves 2 calls to this event - removing and adding.
            if (_rescanIPTimer == null)
            {
                _rescanIPTimer = new Timer(5000);
                _rescanIPTimer.Elapsed += RescanIPTimerElapsed;
                _rescanIPTimer.Start();
            }
        }

        private static void RescanIPTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _rescanIPTimer.Stop();
            _rescanIPTimer = null;
            try
            {
                if (Conf.IPMode == "IPv4")
                {
                    _ipv4Addresses = null;
                    bool iplisted = false;
                    foreach (IPAddress ip in AddressListIPv4)
                    {
                        if (Conf.IPv4Address == ip.ToString())
                            iplisted = true;
                    }
                    if (!iplisted)
                    {
                        _ipv4Address = "";
                        Conf.IPv4Address = AddressIPv4;
                    }
                    if (iplisted)
                        return;
                }
                if (!string.IsNullOrEmpty(Conf.WSUsername) && !string.IsNullOrEmpty(Conf.WSPassword))
                {
                    switch (Conf.IPMode)
                    {
                        case "IPv4":

                            Logger.LogError(
                                "Your IP address has changed. Please set a static IP address for your local computer to ensure uninterrupted connectivity.");
                            //force reload of ip info
                            AddressIPv4 = Conf.IPv4Address;
                            if (Conf.Subscribed)
                            {
                                if (Conf.DHCPReroute && Conf.IPMode == "IPv4")
                                {
                                    //check if IP address has changed
                                    if (Conf.UseUPNP)
                                    {
                                        //change router ports
                                        try
                                        {
                                            if (NATControl.SetPorts(Conf.ServerPort, Conf.LANPort))
                                                Logger.LogMessage("Router port forwarding has been updated. (" +
                                                                        Conf.IPv4Address + ")");
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.LogException(ex);
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogMessage(
                                            "Please check Use UPNP in web settings to handle this automatically");
                                    }
                                }
                                else
                                {
                                    Logger.LogMessage(
                                        "Enable DHCP Reroute in Web Settings to handle this automatically");
                                }
                            }
                            MWS.StopServer();
                            MWS.StartServer();
                            WsWrapper.ForceSync();
                            break;
                        case "IPv6":
                            _ipv6Addresses = null;
                            bool iplisted = false;
                            foreach (IPAddress ip in AddressListIPv6)
                            {
                                if (Conf.IPv6Address == ip.ToString())
                                    iplisted = true;
                            }
                            if (!iplisted)
                            {
                                Logger.LogError(
                                    "Your IP address has changed. Please set a static IP address for your local computer to ensure uninterrupted connectivity.");
                                _ipv6Address = "";
                                AddressIPv6 = Conf.IPv6Address;
                                Conf.IPv6Address = AddressIPv6;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"network change");
            }
        }

        internal void RenderResources()
        {
            Helper.SetTitle(this);
        
    
            switchToolStripMenuItem.Text = LocRm.GetString("Switch");
            onToolStripMenuItem.Text = LocRm.GetString("On");
            offToolStripMenuItem.Text = LocRm.GetString("Off");
          
       
            
            _addCameraToolStripMenuItem.Text = LocRm.GetString("AddCamera");
           
          
         
            _deleteToolStripMenuItem.Text = LocRm.GetString("remove");
            _editToolStripMenuItem.Text = LocRm.GetString("Edit");
    
          
          
            fullScreenToolStripMenuItem.Text = LocRm.GetString("fullScreen");
         
         
          

           

          


          
            _recordNowToolStripMenuItem.Text = LocRm.GetString("RecordNow");
            
            _showFilesToolStripMenuItem.Text = LocRm.GetString("ShowFiles");
          
          
            _takePhotoToolStripMenuItem.Text = LocRm.GetString("TakePhoto");
         
           
          
         
            
            fullScreenToolStripMenuItem.Text = LocRm.GetString("Fullscreen");
           
           
            
           
           

          
           
          
           
          



        
           

        

            
            if (!Helper.HasFeature(Enums.Features.Access_Media))
                splitContainer1.Panel2Collapsed = true;


            statusStrip1.Visible = Conf.ShowStatus && Helper.HasFeature(Enums.Features.View_Status_Bar);
         
            
            ShowHideMediaPane();
            mediaPanelControl1.RenderResources();
            
        }

        private void HouseKeepingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _houseKeepingTimer.Stop();
            if (LayoutPanel.NeedsRedraw)
            {
                _pnlCameras.PerformLayout();
                _pnlCameras.Invalidate();
                LayoutPanel.NeedsRedraw = false;
            }
            if (NeedsResourceUpdate)
            {
                RenderResources();
                NeedsResourceUpdate = false;
            }
            if (_cputotalCounter != null)
            {
                try
                {
                    while (_cpuAverages.Count > 4)
                        _cpuAverages.RemoveAt(0);
                    _cpuAverages.Add(_cpuCounter.NextValue()/Environment.ProcessorCount);

                    CpuUsage = _cpuAverages.Sum()/_cpuAverages.Count;
                    CpuTotal = _cputotalCounter.NextValue();
                    _counters = $"CPU: {CpuUsage:0.00}%";

                    if (_pcMem != null)
                    {
                        _counters += " RAM Usage: " + Convert.ToInt32(_pcMem.RawValue/1048576) + "Mb";
                    }
                    tsslMonitor.Text = _counters;
                }
                catch (Exception ex)
                {
                    // _cputotalCounter = null;
                    Logger.LogException(ex);
                }

                HighCPU = CpuTotal > _conf.CPUMax;
            }
            else
            {
                _counters = "Stats Unavailable - See Log File";
            }

            if (_lastOver > DateTime.MinValue)
            {
                if (_lastOver < Helper.Now.AddSeconds(-4))
                {
                    tsslMediaInfo.Text = "";
                    _lastOver = DateTime.MinValue;
                }
            }

            _pingCounter++;

            if (NeedsMediaRefresh > DateTime.MinValue && NeedsMediaRefresh < Helper.Now.AddSeconds(-1))
                LoadPreviews();


            if (Resizing)
            {
                if (_lastResize < Helper.Now.AddSeconds(-1))
                    Resizing = false;
            }

            if (_pingCounter >= 301)
            {
                _pingCounter = 0;
                //auto save
                try
                {
                    SaveObjects("");
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                try
                {
                    SaveConfig();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            try
            {
                if (!MWS.Running)
                {

                    if (MWS.NumErr >= 5)
                    {
                        Logger.LogMessage("Server not running - restarting");
                        StopAndStartServer();
                    }
                }
                else
                {
                   
                }
                

                if (Conf.ServicesEnabled && !WsWrapper.LoginFailed)
                {
                    if (NeedsSync)
                    {
                        WsWrapper.ForceSync();
                    }
                    WsWrapper.PingServer();
                }


                _storageCounter++;
                if (_storageCounter == 3600) // every hour
                {
                    RunStorageManagement();
                    _storageCounter = 0;
                }


                if (_pingCounter == 80)
                {
                    var t = new Thread(SaveFileData) {IsBackground = true, Name = "Saving File Data"};
                    t.Start();
                }

                if (_needsDelete)
                {
                    _needsDelete = false;
                    try
                    {
                        if (_tDelete == null || _tDelete.Join(TimeSpan.Zero))
                        {
                            _tDelete = new Thread(DeleteFiles) {IsBackground = true};
                            _tDelete.Start();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            Logger.WriteLogs();
            if (!_shuttingDown)
                _houseKeepingTimer.Start();
        }


        public void RunStorageManagement(bool abortIfRunning = false)
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.RunStorageManagementDelegate(RunStorageManagement), abortIfRunning);
                return;
            }


            if (StorageThreadRunning)
            {

                if (abortIfRunning)
                {
                    try
                    {
                        _storageThread.Abort();
                    }
                    catch
                    {
                        //may have exited
                    }
                }
            }
            if (!StorageThreadRunning)
            {
                lock (ThreadLock)
                {
                    bool r = Conf.MediaDirectories.Any(p => p.Enable_Storage_Management);
                    r = r || Cameras.Any(p => p.settings.storagemanagement.enabled);
                    r = r || Microphones.Any(p => p.settings.storagemanagement.enabled);
                    if (r)
                    {
                        Logger.LogMessage("Running Storage Management");
                        _storageThread = new Thread(DeleteOldFiles) {IsBackground = true};
                        _storageThread.Start();
                    }
                }
            }
            else
                Logger.LogMessage("Storage Management is already running");
        }

        private void UpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Stop();

            foreach (Control c in _pnlCameras.Controls)
            {
                try
                {
                    var cameraWindow = c as CameraWindow;
                    if (cameraWindow != null)
                    {
                        cameraWindow.Tick();
                        continue;
                    }
                    var volumeLevel = c as VolumeLevel;
                    if (volumeLevel != null)
                    {
                        volumeLevel.Tick();
                        continue;
                    }
                    var floorPlanControl = c as FloorPlanControl;
                    if (floorPlanControl != null)
                    {
                        FloorPlanControl fpc = floorPlanControl;
                        if (fpc.Fpobject.needsupdate)
                        {
                            fpc.NeedsRefresh = true;
                            fpc.Fpobject.needsupdate = false;
                        }
                        fpc.Tick();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            if (!_shuttingDown)
                _updateTimer.Start();
        }

        private void FswChanged(object sender, FileSystemEventArgs e)
        {
            _fsw.EnableRaisingEvents = false;
            bool err = true;
            int i = 0;
            try
            {
                string txt = "";
                while (err && i < 5)
                {
                    try
                    {
                        using (var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                while (sr.EndOfStream == false)
                                {
                                    txt = sr.ReadLine();
                                    err = false;
                                }
                                sr.Close();
                            }
                            fs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        i++;
                        Thread.Sleep(500);
                    }
                }
                if (!string.IsNullOrEmpty(txt))
                    ParseCommand(txt.Trim());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            _fsw.EnableRaisingEvents = true;
        }

        private void ParseCommand(string command)
        {
            if (command == null) throw new ArgumentNullException("command");
            try
            {
                command = Uri.UnescapeDataString(command);

                if (command.ToLower().StartsWith("open "))
                {
                    Logger.LogMessage("Loading List: " + command);
                    if (InvokeRequired)
                        Invoke(new Delegates.ExternalCommandDelegate(LoadObjectList), command.Substring(5).Trim('"'));
                    else
                        LoadObjectList(command.Substring(5).Trim('"'));

                    return;
                }
                ProcessCommandString(command);

                if (command.ToLower() == "showform")
                {
                    UISync.Execute(ShowIfUnlocked);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.Show(LocRm.GetString("LoadFailed").Replace("[MESSAGE]", ex.Message));
            }
        }

        internal void ProcessCommandString(string command)
        {
            int i = command.ToLower().IndexOf("commands ", StringComparison.Ordinal);
            if (i != -1)
            {
                command = command.Substring(i + 9);
                string[] commands = command.Trim('"').Split('|');
                foreach (string command2 in commands)
                {
                    if (!string.IsNullOrEmpty(command2))
                    {
                        Logger.LogMessage("Running Command: " + command2);
                        if (InvokeRequired)
                            Invoke(new Delegates.ExternalCommandDelegate(ProcessCommandInternal), command2.Trim('"'));
                        else
                            ProcessCommandInternal(command2.Trim('"'));
                    }
                }
            }
        }

        internal static void ProcessCommandInternal(string command)
        {
            //parse command into new format
            string[] cfg = command.Split(',');
            string newcommand;
            switch (cfg.Length)
            {
                default:
                    //generic command
                    newcommand = cfg[0];
                    break;
                case 2:
                    //group command
                    newcommand = cfg[0] + "?group=" + cfg[1];
                    break;
                case 3:
                    //specific device
                    newcommand = cfg[0] + "?ot=" + cfg[1] + "&oid=" + cfg[2];
                    break;
            }
            MWS.ProcessCommandInternal(newcommand);
        }

        public void SetBackground()
        {
            _pnlCameras.BackColor = Conf.MainColor.ToColor();
          
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
               
            }
            catch
            {
                // ignored
            }
            base.OnClosed(e);
        }

        private void MenuItem2Click(object sender, EventArgs e)
        {
            var helpform = new HelpForm();
            helpform.ShowDialog(this);
            helpform.Dispose();
        }

        internal static string StopAndStartServer()
        {
            string message = "";
            try
            {
                MWS.StopServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            Application.DoEvents();
            try
            {
                message = MWS.StartServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return message;
        }

        private void MenuItem4Click(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void MenuItem5Click(object sender, EventArgs e)
        {
            StartBrowser("https://www.bysec.sk/");
        }

        private void MenuItem10Click(object sender, EventArgs e)
        {
            CheckForUpdates(false);
        }

        private void CheckForUpdates(bool suppressMessages)
        {
            try
            {
                if (_updateChecker != null && !_updateChecker.Join(TimeSpan.Zero))
                    return;
            }
            catch
            {
                return;
            }

            _updateChecker = new Thread(() => DoUpdateCheck(suppressMessages));
            _updateChecker.Start();
        }

        private void DoUpdateCheck(bool suppressMessages)
        {
            string version = "";
            try
            {
                version = WsWrapper.ProductLatestVersion(ProductID);
                if (version == LocRm.GetString("iSpyDown"))
                {
                    throw new Exception("down");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                if (!suppressMessages)
                {
                    UISync.Execute(() => MessageBox.Show(LocRm.GetString("CheckUpdateError"), LocRm.GetString("Error")));
                }
            }
            if (version != "" && version != LocRm.GetString("iSpyDown"))
            {
                var verThis = new Version(Application.ProductVersion);
                var verLatest = new Version(version);
                if (verThis < verLatest)
                {
                    UISync.Execute(ShowNewVersion);
                }
                else
                {
                    if (!suppressMessages)
                        UISync.Execute(
                            () =>
                                MessageBox.Show(LocRm.GetString("HaveLatest"), LocRm.GetString("Note"),
                                    MessageBoxButtons.OK));
                }
            }
        }

        private void ShowNewVersion()
        {
            using (var nv = new NewVersion())
            {
                nv.ShowDialog(this);
            }
        }

        private void MenuItem8Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        public void ShowSettings(int tabindex, IWin32Window owner = null)
        {
            int pi = Conf.PreviewItems;
            var settings = new Settings { MainClass = this, InitialTab = tabindex };
            if (settings.ShowDialog(owner ?? this) == DialogResult.OK)
            {
                if (pi != Conf.PreviewItems)
                    NeedsMediaRefresh = Helper.Now;

                _pnlCameras.BackColor = Conf.MainColor.ToColor();
               

                if (!string.IsNullOrEmpty(Conf.Joystick.id))
                {
                    if (_jst == null)
                    {
                        _jst = new JoystickDevice();
                    }
                    _jst.ReleaseJoystick();
                    if (_tmrJoystick != null)
                    {
                        _tmrJoystick.Stop();
                        _tmrJoystick = null;
                    }

                    bool jsactive = false;
                    string[] sticks = _jst.FindJoysticks();
                    foreach (string js in sticks)
                    {
                        string[] nameid = js.Split('|');
                        if (nameid[1] == Conf.Joystick.id)
                        {
                            Guid g = Guid.Parse(nameid[1]);
                            jsactive = _jst.AcquireJoystick(g);
                        }
                    }

                    if (!jsactive)
                    {
                        _jst.ReleaseJoystick();
                        _jst = null;
                    }
                    else
                    {
                        _tmrJoystick = new Timer(100);
                        _tmrJoystick.Elapsed += TmrJoystickElapsed;
                        _tmrJoystick.Start();
                    }
                }
                else
                {
                    if (_tmrJoystick != null)
                    {
                        _tmrJoystick.Stop();
                        _tmrJoystick = null;
                    }

                    if (_jst != null)
                    {
                        _jst.ReleaseJoystick();
                        _jst = null;
                    }
                }
            }

            if (settings.ReloadResources)
            {
                RenderResources();
                LoadCommands();
            }
            AddressIPv4 = ""; //forces reload
            AddressIPv6 = "";
            settings.Dispose();
            SaveConfig();
            Refresh();
        }

        private void MenuItem11Click(object sender, EventArgs e)
        {
            
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            Resizing = true;
            if (WindowState == FormWindowState.Minimized)
            {
                if (Conf.TrayOnMinimise)
                {
                    Hide();
                    if (Conf.BalloonTips)
                    {
                        if (Conf.BalloonTips)
                        {
                           
                        }
                    }
                }
            }
            else
            {
                _previousWindowState = WindowState;
                if (Conf.AutoLayout)
                    _pnlCameras.LayoutObjects(0, 0);
                else
                {
                    _pnlCameras.AutoGrid();
                }
                if (!IsOnScreen(this))
                {
                    Left = 0;
                    Top = 0;
                }
            }
        }

        private void NotifyIcon1DoubleClick(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }

        private CheckPassword _cp;
        private bool _locked;

        public void ShowIfUnlocked()
        {
            if (Visible == false || WindowState == FormWindowState.Minimized)
            {
                if (Conf.Enable_Password_Protect || _locked)
                {
                    if (_cp == null)
                    {
                        using (_cp = new CheckPassword())
                        {
                            _cp.ShowDialog(this);
                            if (_cp.DialogResult == DialogResult.OK)
                            {
                                _locked = false;
                                ShowForm(-1);
                            }
                        }
                        _cp = null;
                    }
                }
                else
                {
                    ShowForm(-1);
                }
            }
            else
            {
                ShowForm(-1);
            }
        }

        private void MainFormFormClosing1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (Conf.MinimiseOnClose && !ShuttingDown)
                {
                    e.Cancel = true;
                    WindowState = FormWindowState.Minimized;
                    return;
                }
            }
            ShuttingDown = true;
            if (_mIcp != null && _mCookie != -1)
            {
                try
                {
                    _mIcp.Unadvise(_mCookie);
                }
                catch
                {
                    // ignored
                }
            }
            Exit();
        }

        private void Exit()
        {
            try
            {
                SaveObjects("");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            _shuttingDown = true;
            WsWrapper.Disconnect();
            try
            {
                MWS.StopServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            

            ThreadKillDelay = 3000;

            _houseKeepingTimer?.Stop();
            _updateTimer?.Stop();
            _tmrJoystick?.Stop();


         

            if (Conf.BalloonTips)
            {
                if (Conf.BalloonTips)
                {
                    
                }
            }
            _closing = true;
            

            try
            {
                SaveConfig();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            try
            {
                if (_talkSource != null)
                {
                    _talkSource.Stop();
                    _talkSource = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            try
            {
                if (_talkTarget != null)
                {
                    _talkTarget.Stop();
                    _talkTarget = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            try
            {
                RemoveObjects();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            
            try
            {
                File.WriteAllText(Program.AppDataPath + "exit.txt", "OK");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (StorageThreadRunning)
            {
                try
                {
                    _storageThread.Join(ThreadKillDelay);
                }
                catch
                {
                }
            }
            Logger.WriteLogs();
        }


        private void ControlNotification(object sender, NotificationType e)
        {
            if (Conf.BalloonTips)
            {
                
            }
        }


        private void NotifyIcon1BalloonTipClicked(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }


        public static string RandomString(int length)
        {
            string b = "";

            for (int i = 0; i < length; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*Random.NextDouble() + 65)));
                b += ch;
            }
            return b;
        }

        private void SetNewStartPosition()
        {
            if (LayoutMode == Enums.LayoutMode.AutoGrid)
            {
                _pnlCameras.LayoutControlsInGrid();
                _pnlCameras.AutoGrid();
                return;
            }
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void VolumeControlRemoteCommand(object sender, ThreadSafeCommand e)
        {
            Delegates.InvokeMethod i = DoInvoke;
            Invoke(i, e.Command);
        }

        internal void ConnectServices(bool checkforUpdates = true)
        {
            if (Conf.ServicesEnabled)
            {
                if (Conf.UseUPNP)
                {
                    NATControl.SetPorts(Conf.ServerPort, Conf.LANPort);
                }

                string[] result =
                    WsWrapper.TestConnection(Conf.WSUsername, Conf.WSPassword, Conf.Loopback);

                if (result.Length > 0 && result[0] == "OK")
                {
                    WsWrapper.Connect();
                    NeedsSync = true;
                    EmailAddress = result[2];
                    MobileNumber = result[4];
                    Conf.Reseller = result[5];

                    Conf.ServicesEnabled = true;
                    Conf.Subscribed = (Convert.ToBoolean(result[1]));

                    Helper.SetTitle(this);

                    if (result[3] == "")
                    {
                        LoopBack = Conf.Loopback;
                        WsWrapper.Connect(Conf.Loopback);
                    }
                    else
                    {
                        LoopBack = false;
                    }
                }
            }
            if (checkforUpdates && Conf.Enable_Update_Check && !SilentStartup)
            {
                UISync.Execute(() => CheckForUpdates(true));
            }
            SilentStartup = false;
        }


        private void EditToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                EditCamera(window.Camobject);
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                EditMicrophone(level.Micobject);
            }
            var target = ContextTarget as FloorPlanControl;
            if (target != null)
            {
                EditFloorplan(target.Fpobject);
            }
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                RemoveCamera(window, true);
                return;
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                RemoveMicrophone(level, true);
                return;
            }
            var fpc = ContextTarget as FloorPlanControl;
            if (fpc != null)
            {
                RemoveFloorplan(fpc, true);
            }
        }


        private void ToolStripButton4Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        public static void GoSubscribe()
        {
            OpenUrl(Website + "/subscribe.aspx");
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                try
                {
                    var p = new Process { StartInfo = { FileName = DefaultBrowser, Arguments = url } };
                    p.Start();
                }
                catch (Exception ex2)
                {
                    Logger.LogException(ex2, "OpenURL");
                }
            }

        }

        private void ActivateToolStripMenuItemClick(object sender, EventArgs e)
        {
            
        }

        private void WebsiteToolstripMenuItemClick(object sender, EventArgs e)
        {
            StartBrowser("https://www.bysec.sk/");
        }

        private void HelpToolstripMenuItemClick(object sender, EventArgs e)
        {
            var helpform = new HelpForm();
            helpform.ShowDialog(this);
            helpform.Dispose();
        }

        private void ShowToolstripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(-1);
        }

        public void ShowForm(double opacity)
        {
            Activate();
            Visible = true;
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = _previousWindowState;
            }
            if (opacity > -1)
                Opacity = opacity;

            //Process currentProcess = Process.GetCurrentProcess();
            //IntPtr hWnd = currentProcess.MainWindowHandle;
            //if (hWnd != IntPtr.Zero)
            //{
            //    SetForegroundWindow(hWnd);
            //}
            TopMost = Conf.AlwaysOnTop;
        }

        private void UnlockToolstripMenuItemClick(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
        }

        private void AddCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(3);
        }

        private void AddMicrophoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddMicrophone(0);
        }

        private void CtxtMainFormOpening(object sender, CancelEventArgs e)
        {
        

            
        }

        public static void StartBrowser(string url)
        {
            if (url != "")
                OpenUrl(url);
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShuttingDown = true;
            Close();
        }

        private void MenuItem3Click(object sender, EventArgs e)
        {
            Connect(false);
        }

        private void MenuItem18Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                string loc = d.Entry + "audio\\";

                if (Directory.Exists(loc))
                {
                    string[] files = Directory.GetFiles(loc, "*.*", SearchOption.AllDirectories);
                    foreach (string t in files)
                    {
                        try
                        {
                            FileOperations.Delete(t);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                loc = d.Entry + "video\\";
                if (Directory.Exists(loc))
                {
                    string[] files = Directory.GetFiles(loc, "*.*", SearchOption.AllDirectories);
                    foreach (string t in files)
                    {
                        try
                        {
                            FileOperations.Delete(t);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
            foreach (objectsCamera oc in Cameras)
            {
                CameraWindow occ = GetCameraWindow(oc.id);
                occ?.ClearFileList();
               
               
            }
            foreach (objectsMicrophone om in Microphones)
            {
                VolumeLevel omc = GetVolumeLevel(om.id);
                omc?.ClearFileList();
            }
            LoadPreviews();
            MessageBox.Show(LocRm.GetString("FilesDeleted"), LocRm.GetString("Note"));
        }

        private void MenuItem20Click(object sender, EventArgs e)
        {
            ShowLogFile();
        }

        private void ShowLogFile()
        {
            Process.Start(Program.AppDataPath + "log_" + Logger.NextLog + ".htm");
        }

        private void ResetSizeToolStripMenuItemClick(object sender, EventArgs e)
        {
            _pnlCameras.Minimize(ContextTarget, true);
        }

        private void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowSettings(0);
        }


        private void MenuItem19Click(object sender, EventArgs e)
        {
            SaveObjectList(true);
        }

        public void SaveObjectList(bool warn = true)
        {
            if (Cameras.Count == 0 && Microphones.Count == 0)
            {
                if (warn)
                    MessageBox.Show(LocRm.GetString("NothingToExport"), LocRm.GetString("Error"));
                return;
            }

            bool save = true;
            string filename = _currentFileName;
            if (warn)
            {

                using (var saveFileDialog = new SaveFileDialog
                                            {
                                                InitialDirectory = _lastPath,
                                                Filter = "BYSEC files (*.bysec)|*.bysec|XML Files (*.xml)|*.xml"
                                            })
                {

                    save = saveFileDialog.ShowDialog(this) == DialogResult.OK;
                    filename = saveFileDialog.FileName;
                }
            }
            if (save)
            {
                if (filename.Trim() != "")
                {
                    SaveObjects(filename);
                    try
                    {
                        var fi = new FileInfo(filename);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }
                }
            }
        }


        private void MenuItem21Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "BYSEC Files (*.bysec)|*.bysec|XML Files (*.xml)|*.xml";
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
                        LoadObjectList(fileName.Trim());
                    }
                }
            }
        }

        private void ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            if (ContextTarget is CameraWindow)
            {
                //id = ((CameraWindow) ContextTarget).Camobject.id.ToString();
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }

            if (ContextTarget is VolumeLevel)
            {
                //id = ((VolumeLevel) ContextTarget).Micobject.id.ToString();
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }

            if (ContextTarget is FloorPlanControl)
            {
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }
        }

        public void Connect(bool silent)
        {
            Connect(Webpage, silent);
        }

        public void Connect(string successUrl, bool silent)
        {
            if (!MWS.Running)
            {
                string message = StopAndStartServer();
                if (message != "")
                {
                    if (!silent)
                        MessageBox.Show(this, message);
                    return;
                }
            }
            if (WsWrapper.WebsiteLive)
            {
                if (Conf.WSUsername != null && Conf.WSUsername.Trim() != "")
                {
                    if (Conf.UseUPNP)
                    {
                        NATControl.SetPorts(Conf.ServerPort, Conf.LANPort);
                    }
                    WsWrapper.Connect();
                    WsWrapper.ForceSync();
                    if (WsWrapper.WebsiteLive)
                    {
                        if (successUrl != "")
                            StartBrowser(successUrl);
                        return;
                    }
                    if (!silent && !_shuttingDown)
                        Logger.LogMessage(LocRm.GetString("WebsiteDown"));
                    return;
                }
                var ws = new Webservices();
                ws.ShowDialog(this);
                if (!string.IsNullOrEmpty(ws.EmailAddress))
                    EmailAddress = ws.EmailAddress;
                if (ws.DialogResult == DialogResult.Yes || ws.DialogResult == DialogResult.No)
                {
                    ws.Dispose();
                    Connect(successUrl, silent);
                    return;
                }
                ws.Dispose();
            }
            else
            {
                Logger.LogMessage(LocRm.GetString("WebsiteDown"));
            }
        }

        private void MenuItem7Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                string foldername = s.Entry + "video\\";
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void MenuItem23Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                string foldername = s.Entry + "audio\\";
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void MenuItem25Click(object sender, EventArgs e)
        {
            ViewMobile();
        }


        private void MainFormHelpButtonClicked(object sender, CancelEventArgs e)
        {
            var helpform = new HelpForm();
            helpform.ShowDialog(this);
            helpform.Dispose();
        }

        private void menuItem21_Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutOptimised();
        }

        private void ShowISpy10PercentOpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(.1);
        }

        private void ShowISpy30OpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(.3);
        }

        private void ShowISpy100PercentOpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(1);
        }

        private void CtxtTaskbarOpening(object sender, CancelEventArgs e)
        {
           

           
        }


        private void MenuItem26Click(object sender, EventArgs e)
        {
            OpenUrl(Website + "/donate.aspx");
        }

        private void RecordNowToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cameraControl = window;
                cameraControl.RecordSwitch(!cameraControl.Recording);
            }

            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var volumeControl = level;
                volumeControl.RecordSwitch(!volumeControl.Recording);
            }
        }

        private void ShowFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                ShowFiles(cw);   
                return;
            }

            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                ShowFiles(vl);
                return;
            }
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                Process.Start(s.Entry);
            }
        }

        internal void ShowFiles(ISpyControl ctrl)
        {
            var cw = ctrl as CameraWindow;
            if (cw != null)
            {
                ShowFiles(cw);
                return;
            }
            var vl = ctrl as VolumeLevel;
            if (vl != null)
            {
                ShowFiles(vl);
                return;
            }
        }
        
        internal void ShowFiles(CameraWindow cw)
        {
            string foldername = Helper.GetMediaDirectory(2, cw.Camobject.id) + "video\\" + cw.Camobject.directory + "\\";
            if (!foldername.EndsWith(@"\"))
                foldername += @"\";
            Process.Start(foldername);
            cw.Camobject.newrecordingcount = 0;
        }

        internal void ShowFiles(VolumeLevel vl)
        {
            string foldername = Helper.GetMediaDirectory(1, vl.Micobject.id) + "audio\\" + vl.Micobject.directory + "\\";
            if (!foldername.EndsWith(@"\"))
                foldername += @"\";
            Process.Start(foldername);
            vl.Micobject.newrecordingcount = 0;
        }

        private void ViewMediaOnAMobileDeviceToolStripMenuItemClick(object sender, EventArgs e)
        {
            ViewMobile();
        }

        private void ViewMobile()
        {
            if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
            {
                OpenUrl(Webserver + "/mobile/");
            }
            else
                WebConnect();
        }

        private void AddFloorPlanToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFloorPlan();
        }

        private void ListenToolStripMenuItemClick(object sender, EventArgs e)
        {
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.Listening = !vf.Listening;
            }
        }

        private void MenuItem31Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            RemoveObjects();
        }

        private void MenuItem34Click(object sender, EventArgs e)
        {
        }


        private void MenuItem33Click(object sender, EventArgs e)
        {
        }

        private void ToolStripButton8Click1(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void MenuItem35Click(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void ToolStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void RemoteCommandsToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void MenuItem37Click(object sender, EventArgs e)
        {
            MessageBox.Show(LocRm.GetString("EditInstruct"), LocRm.GetString("Note"));
        }

        private void PositionToolStripMenuItemClick(object sender, EventArgs e)
        {
            var p = (PictureBox) ContextTarget;
            int w = p.Width;
            int h = p.Height;
            int x = p.Location.X;
            int y = p.Location.Y;

            var le = new LayoutEditor {X = x, Y = y, W = w, H = h};


            if (le.ShowDialog(this) == DialogResult.OK)
            {
                _pnlCameras.PositionPanel(p, new Point(le.X, le.Y), le.W, le.H);
            }
            le.Dispose();
        }



        private void MenuItem38Click(object sender, EventArgs e)
        {
            StartBrowser(Website + "/producthistory.aspx?productid=11");
        }

        private void MenuItem39Click(object sender, EventArgs e)
        {
        }

        private void TakePhotoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                window?.Snapshot();
            }
        }

        private void ToolStripDropDownButton1Click(object sender, EventArgs e)
        {
        }

        private void ThruWebsiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
            {
                OpenUrl(Webpage);
            }
            else
                WebConnect();
        }

        private void OnMobileDevicesToolStripMenuItemClick(object sender, EventArgs e)
        {
            ViewMobile();
        }

        private void LocalCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(3);
        }

        private void IpCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(1);
        }

        private void MicrophoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddMicrophone(0);
        }

        private void FloorPlanToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFloorPlan();
        }

        private void MenuItem12Click(object sender, EventArgs e)
        {
            //+26 height for control bar
            _pnlCameras.LayoutObjects(164, 146);
        }

        private void MenuItem14Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(324, 266);
        }

        private void MenuItem29Click1(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(0, 0);
        }

        private void ToolStripButton1Click1(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void WebConnect()
        {
            var ws = new Webservices();
            ws.ShowDialog(this);
            if (ws.EmailAddress != "")
            {
                EmailAddress = ws.EmailAddress;
                MobileNumber = ws.MobileNumber;
            }
            if (ws.DialogResult == DialogResult.Yes)
            {
                Connect(false);
            }
            ws.Dispose();
            Helper.SetTitle(this);
        }

        private void MenuItem17Click(object sender, EventArgs e)
        {
        }

        private void ResetRecordingCounterToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cw = window;
                cw.Camobject.newrecordingcount = 0;
                cw.Custom = false;
                if (cw.VolumeControl != null)
                {
                    cw.VolumeControl.Micobject.newrecordingcount = 0;
                    cw.VolumeControl.Invalidate();
                }
                cw.Invalidate();
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vw = level;
                vw.Micobject.newrecordingcount = 0;
                if (vw.Paired)
                {
                    objectsCamera oc = Cameras.SingleOrDefault(p => p.settings.micpair == vw.Micobject.id);
                    if (oc != null)
                    {
                        CameraWindow cw = GetCameraWindow(oc.id);
                        cw.Camobject.newrecordingcount = 0;
                        cw.Invalidate();
                    }
                }
                vw.Invalidate();
            }
        }

        private void MenuItem15Click(object sender, EventArgs e)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    cameraControl.Camobject.newrecordingcount = 0;
                    cameraControl.Invalidate();
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    var volumeControl = level;
                    volumeControl.Micobject.newrecordingcount = 0;
                    volumeControl.Invalidate();
                }
            }
        }

        private void SwitchAllOnToolStripMenuItemClick(object sender, EventArgs e)
        {
            SwitchObjects(false, true);
        }

        private void SwitchAllOffToolStripMenuItemClick(object sender, EventArgs e)
        {
            SwitchObjects(false, false);
        }

        private void MenuItem22Click1(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                      {
                          InitialDirectory = Program.AppDataPath,
                          Filter = "iSpy Log Files (*.htm)|*.htm|XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
                      };

            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            string fileName = ofd.FileName;

            if (fileName.Trim() != "")
            {
                Process.Start(ofd.FileName);
            }
        }

        private void USbCamerasAndMicrophonesOnOtherToolStripMenuItemClick(object sender, EventArgs e)
        {
            OpenUrl(Website + "/download_ispyserver.aspx");
        }

        private void MenuItem24Click(object sender, EventArgs e)
        {
            SwitchObjects(false, true);
        }

        private void MenuItem40Click(object sender, EventArgs e)
        {
            SwitchObjects(false, false);
        }

        private void MenuItem41Click(object sender, EventArgs e)
        {
            SwitchObjects(true, false);
        }

        private void MenuItem28Click1(object sender, EventArgs e)
        {
            SwitchObjects(true, true);
        }

        private void MenuItem24Click1(object sender, EventArgs e)
        {
            ApplySchedule();
        }

        public void ApplySchedule()
        {
            foreach (objectsCamera cam in _cameras)
            {
                if (cam.schedule.active)
                {
                    CameraWindow cw = GetCamera(cam.id);
                    cw.ApplySchedule();
                }
            }

            foreach (objectsMicrophone mic in _microphones)
            {
                if (mic.schedule.active)
                {
                    VolumeLevel vl = GetVolumeLevel(mic.id);
                    vl.ApplySchedule();
                }
            }
        }

        private void ApplyScheduleToolStripMenuItemClick1(object sender, EventArgs e)
        {
            ApplySchedule();
        }

        private void ApplyScheduleToolStripMenuItem1Click(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cameraControl = window;
                cameraControl.ApplySchedule();
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.ApplySchedule();
            }
        }

        private void MenuItem24Click2(object sender, EventArgs e)
        {
            ShowGettingStarted();
        }

        private void ShowGettingStarted()
        {
            var gs = new GettingStarted();
            gs.Closed += _gs_Closed;
            gs.Show(this);
            gs.Activate();
        }

        private void _gs_Closed(object sender, EventArgs e)
        {
            if (((GettingStarted) sender).LangChanged)
            {
                RenderResources();
                LoadCommands();
                Refresh();
            }
        }

        private void MenuItem28Click2(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(644, 506);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShuttingDown = true;
            Close();
        }


        public void ExternalClose()
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.CloseDelegate(ExternalClose));
                return;
            }
            ShuttingDown = true;
            Close();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(ContextTarget);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            var helpform = new HelpForm();
            helpform.ShowDialog(this);
            helpform.Dispose();
        }

        private void inExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                string foldername = d.Entry;
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void menuItem1_Click_1(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(-1, -1);
        }

        private void llblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        internal void MediaSelectAll()
        {
            bool check = false, first = true;
            lock (ThreadLock)
            {
                foreach (Control c in flowPreview.Controls)
                {
                    var pb = c as PreviewBox;
                    if (pb != null)
                    {
                        if (first)
                            check = !pb.Selected;
                        first = false;
                        pb.Selected = check;
                    }
                }
                flowPreview.Invalidate(true);
            }
        }


        private void opacityToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowForm(.1);
        }

        private void opacityToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowForm(.3);
        }

        private void opacityToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ShowForm(1);
        }

        private void autoLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
           
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void saveLayoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _pnlCameras.SaveLayout();
          
        }

        private void resetLayoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _pnlCameras.ResetLayout();
        }

        private void fullScreenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MaxMin();
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
        }

        private void fileMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
          
        }

        private void toolStripToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           
        }

        private void alwaysOnTopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
           
            TopMost = Conf.AlwaysOnTop;
        }

        private void mediaPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
           
            ShowHideMediaPane();
        }

        private void ShowHideMediaPane()
        {
            if (Conf.ShowMediaPanel && Helper.HasFeature(Enums.Features.Access_Media))
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
            }
        }

        private void iPCameraWithWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(1, true);
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            OpenUrl(PurchaseLink);
        }

        private void tsbPlugins_Click(object sender, EventArgs e)
        {
            OpenUrl("http://www.ispyconnect.com/plugins.aspx");
        }

        private void flowPreview_MouseEnter(object sender, EventArgs e)
        {
            //flowPreview.Focus();
        }

        private void flowPreview_Click(object sender, EventArgs e)
        {
        }

        private void flCommands_MouseEnter(object sender, EventArgs e)
        {
            //flCommands.Focus();
        }

        public void PTZToolUpdate(CameraWindow cw)
        {
            if (_ptzTool != null)
            {
                _ptzTool.CameraControl = cw;
            }
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;
            if (e.KeyCode == Keys.PageUp)
            {
                ProcessKey("previous_control");
                return;
            }

            if (e.KeyCode == Keys.PageDown)
            {
                ProcessKey("next_control"); return;
            }
            if (!e.Alt && !e.Shift && e.Control)
            {
                if (e.KeyCode == Keys.P )
                {
                    ProcessKey("play"); return;
                }

                if (e.KeyCode == Keys.S)
                {
                    ProcessKey("stop"); return;
                }

                if (e.KeyCode == Keys.R)
                {
                    ProcessKey("record"); return;
                }

                if (e.KeyCode == Keys.Z)
                {
                    ProcessKey("zoom"); return;
                }

                if (e.KeyCode == Keys.T )
                {
                    ProcessKey("talk"); return;
                }

                if (e.KeyCode == Keys.L )
                {
                    ProcessKey("listen"); return;
                }

                if (e.KeyCode == Keys.G )
                {
                    ProcessKey("grab"); return;
                }

                if (e.KeyCode == Keys.E )
                {
                    ProcessKey("edit"); return;
                }

                if (e.KeyCode == Keys.F )
                {
                    ProcessKey("tags"); return;
                }

                if (e.KeyCode == Keys.I)
                {
                    ProcessKey("import"); return;
                }
            }
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                ProcessKey("power"); return;
            }
            if (e.KeyCode.ToString() == "D0")
            {
                MaximiseControl(10); return;
            }
            if (e.KeyCode.ToString() == "D1")
            {
                MaximiseControl(0); return;
            }
            if (e.KeyCode.ToString() == "D2")
            {
                MaximiseControl(1); return;
            }
            if (e.KeyCode.ToString() == "D3")
            {
                MaximiseControl(2); return;
            }
            if (e.KeyCode.ToString() == "D4")
            {
                MaximiseControl(3); return;
            }
            if (e.KeyCode.ToString() == "D5")
            {
                MaximiseControl(4); return;
            }
            if (e.KeyCode.ToString() == "D6")
            {
                MaximiseControl(5); return;
            }
            if (e.KeyCode.ToString() == "D7")
            {
                MaximiseControl(6); return;
            }
            if (e.KeyCode.ToString() == "D8")
            {
                MaximiseControl(7); return;
            }
            if (e.KeyCode.ToString() == "D9")
            {
                MaximiseControl(8); return;
            }

            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                MaxMin(); return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (_lastClicked == _pnlCameras)
                {
                    ProcessKey("delete");
                    ProcessKey("next_control");
                    return;
                }
                if (_lastClicked == flowPreview)
                {
                    MediaDeleteSelected();
                }
            }
            if (e.KeyCode.ToString()=="Menu")
            {
                
              
            }
            int i = -1;
            var c = GetActiveControl(out i);
            if (i > -1)
            {
                var cw = c as CameraWindow;
                if (cw != null)
                {
                    var converter = new KeysConverter();
                    string cmd = converter.ConvertToString(e.KeyData);
                    if (cw.ExecutePluginShortcut(cmd))
                        return;
                }
            }
            //unhandled
        }

        private void MaximiseControl(int index)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                if (c.Tag is int)
                {
                    if ((int) c.Tag == index)
                    {
                        _pnlCameras.Maximise(c, true);
                        c.Focus();
                        break;
                    }
                }
            }
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            if (_vc != null)
            {
                _vc.Close();
                _vc = null;
            }
            else
                ShowViewController();
        }

        private void ShowViewController()
        {
            if (_vc == null)
            {
                _vc = new ViewController(_pnlCameras);
                if (_pnlCameras.Height > 0)
                {
                    double ar = Convert.ToDouble(_pnlCameras.Height)/Convert.ToDouble(_pnlCameras.Width);
                    _vc.Width = 180;
                    _vc.Height = Convert.ToInt32(ar*_vc.Width);
                }
                _vc.TopMost = true;

                _vc.Show();
                _vc.Closed += _vc_Closed;
                
            }
            else
            {
                _vc.Show();
            }
        }

        private void _vc_Closed(object sender, EventArgs e)
        {
            _vc = null;
          
        }

        private void _pnlCameras_Scroll(object sender, ScrollEventArgs e)
        {
            if (_vc != null)
                _vc.Redraw();
        }

        private void _toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.bottom;
            Arrange(true);
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.left;
            Arrange(true);
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.right;
            Arrange(true);
        }

        private void Arrange(bool ShowIfHidden)
        {
            if (!Conf.ShowMediaPanel)
            {
                if (ShowIfHidden)
                {
                 
                    Conf.ShowMediaPanel = true;
                    ShowHideMediaPane();
                }
                else
                    return;
            }

            SuspendLayout();
            try
            {
                var lm = (LayoutModes) Conf.LayoutMode;


                switch (lm)
                {
                    case LayoutModes.bottom:
                        splitContainer1.Orientation = Orientation.Horizontal;
                        splitContainer1.RightToLeft = RightToLeft.No;

                      
                        break;
                    case LayoutModes.left:
                        splitContainer1.Orientation = Orientation.Vertical;
                        splitContainer1.RightToLeft = RightToLeft.Yes;

                  
                        break;
                    case LayoutModes.right:
                        splitContainer1.Orientation = Orientation.Vertical;
                        splitContainer1.RightToLeft = RightToLeft.No;

                  

                        splitContainer1.SplitterDistance = splitContainer1.Width - 200;
              

                        break;
                }
            }
            catch
            {
            }
            ResumeLayout(true);
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void pTZControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void ShowHidePTZTool()
        {
            bool bShow = true;
            if (_ptzTool != null)
            {
                _ptzTool.Close();
                bShow = false;
            }
            else
            {
                _ptzTool = new PTZTool {Owner = this};
                _ptzTool.Show(this);
                _ptzTool.Closing += PTZToolClosing;
                _ptzTool.CameraControl = null;
                for (int i = 0; i < _pnlCameras.Controls.Count; i++)
                {
                    Control c = _pnlCameras.Controls[i];
                    if (c.Focused && c is CameraWindow)
                    {
                        _ptzTool.CameraControl = (CameraWindow) c;
                        break;
                    }
                }
            }
            
              
            Conf.ShowPTZController = bShow;
        }

        private void PTZToolClosing(object sender, CancelEventArgs e)
        {
          
               
            Conf.ShowPTZController = false;
            _ptzTool = null;
        }

        public void TalkTo(CameraWindow cw, bool talk)
        {
            if (string.IsNullOrEmpty(Conf.TalkMic))
                return;

            if (_talkSource != null)
            {
                _talkSource.Stop();
                _talkSource = null;
            }
            if (_talkTarget != null)
            {
                _talkTarget.Stop();
                _talkTarget = null;
            }

            if (!talk)
            {
                if (cw.VolumeControl != null)
                {
                    cw.VolumeControl.Listening = false;
                }
                return;
            }
            Application.DoEvents();
            TalkCamera = cw;
            _talkSource = new TalkDeviceStream(Conf.TalkMic) {RecordingFormat = new WaveFormat(8000, 16, 1)};
            _talkSource.AudioFinished += _talkSource_AudioFinished;

            if (!_talkSource.IsRunning)
                _talkSource.Start();

            _talkTarget = TalkHelper.GetTalkTarget(cw.Camobject, _talkSource);
            _talkTarget.TalkStopped += TalkTargetTalkStopped;
            _talkTarget.Start();

            //auto listen
            if (cw.VolumeControl != null)
            {
                cw.VolumeControl.Listening = true;
            }
        }

        private void _talkSource_AudioFinished(object sender, PlayingFinishedEventArgs e)
        {
            //Logger.LogMessage("Talk Finished: " + reason);
        }

        private void TalkTargetTalkStopped(object sender, EventArgs e)
        {
            if (TalkCamera != null)
            {
                TalkCamera.Talking = false;
            }
        }

        private void pTZToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void viewControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vc != null)
            {
                _vc.Close();
                _vc = null;
            }
            else
                ShowViewController();
        }

        private void menuItem22_Click(object sender, EventArgs e)
        {
            Conf.LockLayout = !Conf.LockLayout;
           
        }

        private void iSpyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.iSpy);
        }

        private void defaultPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.Default);
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.Website);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaDeleteSelected();
        }

        private void pTZControllerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void showInFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pb = ((PreviewBox) ContextTarget);

            string argument = @"/select, " + pb.FileName;
            Process.Start("explorer.exe", argument);
        }

        private void otherVideoSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(8);
        }

        private void videoFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(5);
        }

        private void saveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fbdSaveTo.ShowDialog(Handle))
                {
                    lock (ThreadLock)
                    {
                        for (int i = 0; i < flowPreview.Controls.Count; i++)
                        {
                            var pb = flowPreview.Controls[i] as PreviewBox;
                            if (pb != null && pb.Selected)
                            {
                                var fi = new FileInfo(pb.FileName);
                                File.Copy(pb.FileName, _fbdSaveTo.FileName + @"\" + fi.Name);
                            }
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }      

        private void _tsslStats_Click(object sender, EventArgs e)
        {
            if (!MWS.Running)
            {
                ShowLogFile();
            }
            else
            {
                
                if (WsWrapper.WebsiteLive && !WsWrapper.LoginFailed && !string.IsNullOrEmpty(Conf.WSUsername))
                {
                    if (Conf.ServicesEnabled)
                    {
                        OpenUrl(!Conf.Subscribed
                            ? Webserver + "/subscribe.aspx"
                            : Webpage);
                    }
                    else
                    {
                        OpenUrl(Webserver);
                    }
                }
                else
                {
                    WebConnect();
                }
                
            }
        }

        private void UnlockLayout()
        {
           
        }

        private void MaxMin()
        {
           
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
         
        }

        public static string Webpage
        {
            get
            {
                if (CustomWebserver)
                    return Webserver + "/watch_new.aspx";
                return Webserver + "/monitor/";
            }
        }


        private void ListGridViews()
        {
          

            foreach (configurationGrid gv in Conf.GridViews)
            {
                var tsi = new ToolStripMenuItem(gv.name, Resources.Video2);
                tsi.Click += tsi_Click;
             
                var mi = new MenuItem(gv.name,mi_click);
           
            }
        }

        void mi_click(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            ShowGridView(mi.Text);
        }
        
        void tsi_Click(object sender, EventArgs e)
        {
            var mi = (ToolStripItem)sender;
            ShowGridView(mi.Text);
        }

        void tsi_MaximiseClick(object sender, EventArgs e)
        {
            var mi = (ToolStripItem)sender;
            foreach (Control o in _pnlCameras.Controls)
            {
                var ic = o as ISpyControl;
                if (ic!=null && ic.ObjectName == mi.Text)
                {
                    _pnlCameras.Maximise(ic);
                    return;
                }
            }
        }

        

        public void EditGridView(string name, IWin32Window parent = null)
        {
            if (parent == null)
                parent = this;
            configurationGrid cg = Conf.GridViews.FirstOrDefault(p => p.name == name);
            if (cg != null)
            {
                var gvc = new GridViewCustom
                          {
                              Cols = cg.Columns,
                              Rows = cg.Rows,
                              GridName = cg.name,
                              FullScreen = cg.FullScreen,
                              AlwaysOnTop = cg.AlwaysOnTop,
                              Display = cg.Display,
                              Framerate = cg.Framerate,
                              Mode = cg.ModeIndex,
                              ModeConfig = cg.ModeConfig,
                              Overlays = cg.Overlays,
                              Fill = cg.Fill,
                              ShowAtStartup = cg.ShowAtStartup,
                          };
               // bool b = ((Form) parent).TopMost;
                //((Form) parent).TopMost = false;
                gvc.ShowDialog(parent);
                //((Form)parent).TopMost = b;
                if (gvc.DialogResult == DialogResult.OK)
                {
                    cg.Columns = gvc.Cols;
                    cg.Rows = gvc.Rows;
                    cg.name = gvc.GridName;
                    cg.FullScreen = gvc.FullScreen;
                    cg.AlwaysOnTop = gvc.AlwaysOnTop;
                    cg.Display = gvc.Display;
                    cg.Framerate = gvc.Framerate;
                    cg.ModeIndex = gvc.Mode;
                    cg.ModeConfig = gvc.ModeConfig;
                    cg.Overlays = gvc.Overlays;
                    cg.Fill = gvc.Fill;
                    cg.ShowAtStartup = gvc.ShowAtStartup;
                    ListGridViews();
                }
                gvc.Dispose();
            }
        }

        private void oNVIFCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(9);
        }


        private void configurePluginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cameraControl = ContextTarget as CameraWindow;
            if (cameraControl?.Camera?.Plugin != null)
            {
                cameraControl.ConfigurePlugin();
            }
        }

        private void flowPreview_MouseLeave(object sender, EventArgs e)
        {
            tsslMediaInfo.Text = "";
            //panel1.Hide(); - can't do this as not compatible with touch
        }


        private void llblFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        internal void MediaFilter()
        {
            var f = new Filter();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                LoadPreviews();
            }
            f.Dispose(); 
        }

        private void menuItem26_Click(object sender, EventArgs e)
        {
            
          
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void flowPreview_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = flowPreview;
            flowPreview.Focus();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

#region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ctxtMainForm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.switchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._recordNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._takePhotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslMediaInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslMonitor = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this._localCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.recordListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchOFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid4XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid8XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid16XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutOurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buyLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this._pnlCameras = new iSpyApplication.Controls.LayoutPanel();
            this.flowPreview = new iSpyApplication.Controls.MediaPanel();
            this.mediaPanelControl1 = new iSpyApplication.Controls.MediaPanelControl();
            this.ctxtMainForm.SuspendLayout();
            this.ctxtMnu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ctxtMainForm
            // 
            this.ctxtMainForm.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addCameraToolStripMenuItem});
            this.ctxtMainForm.Name = "_ctxtMainForm";
            this.ctxtMainForm.Size = new System.Drawing.Size(145, 30);
            this.ctxtMainForm.Opening += new System.ComponentModel.CancelEventHandler(this.CtxtMainFormOpening);
            // 
            // _addCameraToolStripMenuItem
            // 
            this._addCameraToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_addCameraToolStripMenuItem.Image")));
            this._addCameraToolStripMenuItem.Name = "_addCameraToolStripMenuItem";
            this._addCameraToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this._addCameraToolStripMenuItem.Text = "Add &Camera";
            this._addCameraToolStripMenuItem.Click += new System.EventHandler(this.AddCameraToolStripMenuItemClick);
            // 
            // ctxtMnu
            // 
            this.ctxtMnu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchToolStripMenuItem,
            this._recordNowToolStripMenuItem,
            this._takePhotoToolStripMenuItem,
            this._editToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this._showFilesToolStripMenuItem,
            this._deleteToolStripMenuItem});
            this.ctxtMnu.Name = "_ctxtMnu";
            this.ctxtMnu.Size = new System.Drawing.Size(144, 186);
            this.ctxtMnu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtMnu_Opening);
            // 
            // switchToolStripMenuItem
            // 
            this.switchToolStripMenuItem.AutoSize = false;
            this.switchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onToolStripMenuItem,
            this.offToolStripMenuItem});
            this.switchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("switchToolStripMenuItem.Image")));
            this.switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            this.switchToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.switchToolStripMenuItem.Text = "Switch";
            // 
            // onToolStripMenuItem
            // 
            this.onToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("onToolStripMenuItem.Image")));
            this.onToolStripMenuItem.Name = "onToolStripMenuItem";
            this.onToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.onToolStripMenuItem.Text = "On";
            this.onToolStripMenuItem.Click += new System.EventHandler(this.onToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("offToolStripMenuItem.Image")));
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // _recordNowToolStripMenuItem
            // 
            this._recordNowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_recordNowToolStripMenuItem.Image")));
            this._recordNowToolStripMenuItem.Name = "_recordNowToolStripMenuItem";
            this._recordNowToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this._recordNowToolStripMenuItem.Text = "Record Now";
            this._recordNowToolStripMenuItem.Click += new System.EventHandler(this.RecordNowToolStripMenuItemClick);
            // 
            // _takePhotoToolStripMenuItem
            // 
            this._takePhotoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_takePhotoToolStripMenuItem.Image")));
            this._takePhotoToolStripMenuItem.Name = "_takePhotoToolStripMenuItem";
            this._takePhotoToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this._takePhotoToolStripMenuItem.Text = "Take Photo";
            this._takePhotoToolStripMenuItem.Click += new System.EventHandler(this.TakePhotoToolStripMenuItemClick);
            // 
            // _editToolStripMenuItem
            // 
            this._editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_editToolStripMenuItem.Image")));
            this._editToolStripMenuItem.Name = "_editToolStripMenuItem";
            this._editToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this._editToolStripMenuItem.Text = "&Edit";
            this._editToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItemClick);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fullScreenToolStripMenuItem.Image")));
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // _showFilesToolStripMenuItem
            // 
            this._showFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_showFilesToolStripMenuItem.Image")));
            this._showFilesToolStripMenuItem.Name = "_showFilesToolStripMenuItem";
            this._showFilesToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this._showFilesToolStripMenuItem.Text = "Show Files";
            this._showFilesToolStripMenuItem.Click += new System.EventHandler(this.ShowFilesToolStripMenuItemClick);
            // 
            // _deleteToolStripMenuItem
            // 
            this._deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_deleteToolStripMenuItem.Image")));
            this._deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            this._deleteToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this._deleteToolStripMenuItem.Text = "&Remove";
            this._deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItemClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslMediaInfo,
            this.tsslMonitor});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(966, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // tsslMediaInfo
            // 
            this.tsslMediaInfo.Name = "tsslMediaInfo";
            this.tsslMediaInfo.Size = new System.Drawing.Size(0, 21);
            // 
            // tsslMonitor
            // 
            this.tsslMonitor.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsslMonitor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tsslMonitor.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsslMonitor.ForeColor = System.Drawing.Color.White;
            this.tsslMonitor.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsslMonitor.Margin = new System.Windows.Forms.Padding(0);
            this.tsslMonitor.Name = "tsslMonitor";
            this.tsslMonitor.Size = new System.Drawing.Size(112, 26);
            this.tsslMonitor.Text = "Monitoring...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Gray;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AllowDrop = true;
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.DimGray;
            this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splitContainer1.Panel1.Controls.Add(this.panel5);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Enabled = false;
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Panel2MinSize = 1;
            this.splitContainer1.Size = new System.Drawing.Size(966, 500);
            this.splitContainer1.SplitterDistance = 470;
            this.splitContainer1.TabIndex = 21;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this._pnlCameras);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(57, 24);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(709, 446);
            this.panel5.TabIndex = 27;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(57, 446);
            this.panel2.TabIndex = 26;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(50, 50);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripDropDownButton2,
            this.toolStripButton3,
            this.toolStripButton5,
            this.toolStripButton2,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(57, 446);
            this.toolStrip1.TabIndex = 24;
            // 
            // _toolStripDropDownButton2
            // 
            this._toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._localCameraToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this._toolStripDropDownButton2.Font = new System.Drawing.Font("Algerian", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._toolStripDropDownButton2.Image = global::iSpyApplication.Properties.Resources.IPCamera;
            this._toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripDropDownButton2.Name = "_toolStripDropDownButton2";
            this._toolStripDropDownButton2.ShowDropDownArrow = false;
            this._toolStripDropDownButton2.Size = new System.Drawing.Size(56, 54);
            this._toolStripDropDownButton2.Text = "ADD CAM";
            this._toolStripDropDownButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._toolStripDropDownButton2.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this._toolStripDropDownButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this._toolStripDropDownButton2.Click += new System.EventHandler(this._toolStripDropDownButton2_Click_1);
            // 
            // _localCameraToolStripMenuItem
            // 
            this._localCameraToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this._localCameraToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._localCameraToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.local;
            this._localCameraToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._localCameraToolStripMenuItem.Name = "_localCameraToolStripMenuItem";
            this._localCameraToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._localCameraToolStripMenuItem.Size = new System.Drawing.Size(173, 20);
            this._localCameraToolStripMenuItem.Text = "Local Camera";
            this._localCameraToolStripMenuItem.Click += new System.EventHandler(this._localCameraToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.toolStripMenuItem2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem2.Image = global::iSpyApplication.Properties.Resources.Steam1;
            this.toolStripMenuItem2.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripMenuItem2.Size = new System.Drawing.Size(173, 20);
            this.toolStripMenuItem2.Text = "IP Camera";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.toolStripMenuItem3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem3.Image = global::iSpyApplication.Properties.Resources.circle;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem3.Text = "ONVIF Camera";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItem4.Image = global::iSpyApplication.Properties.Resources.video_file;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(173, 22);
            this.toolStripMenuItem4.Text = "Video File";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Font = new System.Drawing.Font("Algerian", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton3.Image = global::iSpyApplication.Properties.Resources.icons8_плагин_64;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton3.Text = "Plugins";
            this.toolStripButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton5.Text = "Events";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::iSpyApplication.Properties.Resources.icons8_сетка_активности_64;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton2.Text = "Grid 4X";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::iSpyApplication.Properties.Resources.icons8_сетка_4x4_64;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton6.Text = "Grid 9X ";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::iSpyApplication.Properties.Resources.icons8_сетка_2х2_64;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton7.Text = "Grid 16X";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::iSpyApplication.Properties.Resources.icons8_тюрьма_64;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(56, 54);
            this.toolStripButton1.Text = "Windows";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.menuStrip1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(766, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,
            this.openLayoutToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.saveLayoutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // openLayoutToolStripMenuItem
            // 
            this.openLayoutToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.openLayoutToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.openLayoutToolStripMenuItem.Name = "openLayoutToolStripMenuItem";
            this.openLayoutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.openLayoutToolStripMenuItem.Text = "Open Layout";
            this.openLayoutToolStripMenuItem.Click += new System.EventHandler(this.openLayoutToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.fullScreenToolStripMenuItem1,
            this.recordListToolStripMenuItem});
            this.viewToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.viewToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.filesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.filesToolStripMenuItem.Text = "Files";
            this.filesToolStripMenuItem.Click += new System.EventHandler(this.filesToolStripMenuItem_Click);
            // 
            // fullScreenToolStripMenuItem1
            // 
            this.fullScreenToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.fullScreenToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.fullScreenToolStripMenuItem1.Name = "fullScreenToolStripMenuItem1";
            this.fullScreenToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.fullScreenToolStripMenuItem1.Text = "Full Screen";
            this.fullScreenToolStripMenuItem1.Click += new System.EventHandler(this.fullScreenToolStripMenuItem1_Click_1);
            // 
            // recordListToolStripMenuItem
            // 
            this.recordListToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.recordListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oNToolStripMenuItem1,
            this.oFFToolStripMenuItem1});
            this.recordListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.recordListToolStripMenuItem.Name = "recordListToolStripMenuItem";
            this.recordListToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.recordListToolStripMenuItem.Text = "Record List";
            // 
            // oNToolStripMenuItem1
            // 
            this.oNToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.oNToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.oNToolStripMenuItem1.Name = "oNToolStripMenuItem1";
            this.oNToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.oNToolStripMenuItem1.Text = "ON";
            this.oNToolStripMenuItem1.Click += new System.EventHandler(this.oNToolStripMenuItem1_Click);
            // 
            // oFFToolStripMenuItem1
            // 
            this.oFFToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.oFFToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.oFFToolStripMenuItem1.Name = "oFFToolStripMenuItem1";
            this.oFFToolStripMenuItem1.Size = new System.Drawing.Size(105, 22);
            this.oFFToolStripMenuItem1.Text = "OFF";
            this.oFFToolStripMenuItem1.Click += new System.EventHandler(this.oFFToolStripMenuItem1_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchONToolStripMenuItem,
            this.switchOFFToolStripMenuItem,
            this.grid4XToolStripMenuItem,
            this.grid8XToolStripMenuItem,
            this.grid16XToolStripMenuItem});
            this.optionsToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // switchONToolStripMenuItem
            // 
            this.switchONToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.switchONToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.switchONToolStripMenuItem.Name = "switchONToolStripMenuItem";
            this.switchONToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.switchONToolStripMenuItem.Text = "Switch ON";
            this.switchONToolStripMenuItem.Click += new System.EventHandler(this.switchONToolStripMenuItem_Click);
            // 
            // switchOFFToolStripMenuItem
            // 
            this.switchOFFToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.switchOFFToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.switchOFFToolStripMenuItem.Name = "switchOFFToolStripMenuItem";
            this.switchOFFToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.switchOFFToolStripMenuItem.Text = "Switch OFF";
            this.switchOFFToolStripMenuItem.Click += new System.EventHandler(this.switchOFFToolStripMenuItem_Click);
            // 
            // grid4XToolStripMenuItem
            // 
            this.grid4XToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.grid4XToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.grid4XToolStripMenuItem.Name = "grid4XToolStripMenuItem";
            this.grid4XToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.grid4XToolStripMenuItem.Text = "Grid 4 X";
            this.grid4XToolStripMenuItem.Click += new System.EventHandler(this.grid4XToolStripMenuItem_Click);
            // 
            // grid8XToolStripMenuItem
            // 
            this.grid8XToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.grid8XToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.grid8XToolStripMenuItem.Name = "grid8XToolStripMenuItem";
            this.grid8XToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.grid8XToolStripMenuItem.Text = "Grid 9 X";
            this.grid8XToolStripMenuItem.Click += new System.EventHandler(this.grid8XToolStripMenuItem_Click);
            // 
            // grid16XToolStripMenuItem
            // 
            this.grid16XToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.grid16XToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.grid16XToolStripMenuItem.Name = "grid16XToolStripMenuItem";
            this.grid16XToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.grid16XToolStripMenuItem.Text = "Grid 16 X";
            this.grid16XToolStripMenuItem.Click += new System.EventHandler(this.grid16XToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutOurToolStripMenuItem,
            this.licenseToolStripMenuItem});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutOurToolStripMenuItem
            // 
            this.aboutOurToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.aboutOurToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutOurToolStripMenuItem.Name = "aboutOurToolStripMenuItem";
            this.aboutOurToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.aboutOurToolStripMenuItem.Text = "About ass";
            this.aboutOurToolStripMenuItem.Click += new System.EventHandler(this.aboutOurToolStripMenuItem_Click);
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.licenseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerToolStripMenuItem,
            this.buyLicenseToolStripMenuItem});
            this.licenseToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.licenseToolStripMenuItem.Text = "License";
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.registerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.registerToolStripMenuItem.Text = "Register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // buyLicenseToolStripMenuItem
            // 
            this.buyLicenseToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.buyLicenseToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.buyLicenseToolStripMenuItem.Name = "buyLicenseToolStripMenuItem";
            this.buyLicenseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.buyLicenseToolStripMenuItem.Text = "Buy License";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(766, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 470);
            this.panel1.TabIndex = 22;
            this.panel1.Visible = false;
            this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.mediaPanelControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 402);
            this.panel4.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.flowPreview);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 32);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 370);
            this.panel6.TabIndex = 22;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(143)))), ((int)(((byte)(186)))));
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 402);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 68);
            this.panel3.TabIndex = 22;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::iSpyApplication.Properties.Resources.Logo_white_TM;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 68);
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 10000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // _pnlCameras
            // 
            this._pnlCameras.AutoScroll = true;
            this._pnlCameras.BackColor = System.Drawing.Color.Gray;
            this._pnlCameras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._pnlCameras.ContextMenuStrip = this.ctxtMainForm;
            this._pnlCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCameras.Location = new System.Drawing.Point(0, 0);
            this._pnlCameras.Margin = new System.Windows.Forms.Padding(0);
            this._pnlCameras.Name = "_pnlCameras";
            this._pnlCameras.Size = new System.Drawing.Size(709, 446);
            this._pnlCameras.TabIndex = 19;
            this._pnlCameras.Scroll += new System.Windows.Forms.ScrollEventHandler(this._pnlCameras_Scroll);
            this._pnlCameras.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlCameras_Paint);
            this._pnlCameras.MouseDown += new System.Windows.Forms.MouseEventHandler(this._pnlCameras_MouseDown);
            this._pnlCameras.Resize += new System.EventHandler(this._pnlCameras_Resize);
            // 
            // flowPreview
            // 
            this.flowPreview.AutoScroll = true;
            this.flowPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(143)))), ((int)(((byte)(186)))));
            this.flowPreview.ContextMenuStrip = this.ctxtMainForm;
            this.flowPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPreview.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPreview.Location = new System.Drawing.Point(0, 0);
            this.flowPreview.Margin = new System.Windows.Forms.Padding(0);
            this.flowPreview.Name = "flowPreview";
            this.flowPreview.Padding = new System.Windows.Forms.Padding(2);
            this.flowPreview.Size = new System.Drawing.Size(200, 370);
            this.flowPreview.TabIndex = 0;
            this.flowPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.flowPreview_Paint_1);
            this.flowPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.flowPreview_MouseDown);
            this.flowPreview.MouseEnter += new System.EventHandler(this.flowPreview_MouseEnter);
            this.flowPreview.MouseLeave += new System.EventHandler(this.flowPreview_MouseLeave);
            // 
            // mediaPanelControl1
            // 
            this.mediaPanelControl1.AutoSize = true;
            this.mediaPanelControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(143)))), ((int)(((byte)(186)))));
            this.mediaPanelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediaPanelControl1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaPanelControl1.Location = new System.Drawing.Point(0, 0);
            this.mediaPanelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.mediaPanelControl1.Name = "mediaPanelControl1";
            this.mediaPanelControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.mediaPanelControl1.Size = new System.Drawing.Size(200, 32);
            this.mediaPanelControl1.TabIndex = 21;
            this.mediaPanelControl1.Load += new System.EventHandler(this.mediaPanelControl1_Load);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(966, 500);
            this.Controls.Add(this.splitContainer1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IQ VMS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainFormHelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing1);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.ctxtMainForm.ResumeLayout(false);
            this.ctxtMnu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

#endregion

        

        private enum LayoutModes
        {
            bottom,
            left,
            right
        };

        

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

        private void menuItem27_Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(120,50);
        }

        private void menuItem28_Click(object sender, EventArgs e)
        {
            if (_cameras != null && (_cameras.Count > 0 || _microphones.Count > 0 || _floorplans.Count > 0))
            {
                switch (
                    MessageBox.Show(this, LocRm.GetString("SaveObjectsFirst"), LocRm.GetString("Confirm"),
                                    MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveObjectList();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            _houseKeepingTimer.Stop();
        
            Application.DoEvents();
            RemoveObjects();
            flowPreview.Loading = true;

            _cameras = new List<objectsCamera>();
            _microphones = new List<objectsMicrophone>();
            _floorplans = new List<objectsFloorplan>();
            _actions = new List<objectsActionsEntry>();

            RenderObjects();
            Application.DoEvents();
            try
            {
                _houseKeepingTimer.Start();
            }
            catch (Exception)
            {
            }

        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageGridViews();
        }

        private void ManageGridViews()
        {
            var gvm = new GridViewManager { MainClass = this };
            gvm.ShowDialog(this);
            gvm.Dispose();
            ListGridViews();
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaArchiveSelected();

        }

        private void menuItem29_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Conf.ArchiveNew))
            {
                MessageBox.Show(this, LocRm.GetString("SpecifyArchiveLocation"));
                ShowSettings(2);
            }
            if (!string.IsNullOrWhiteSpace(Conf.ArchiveNew))
            {
                var dir = Conf.ArchiveNew;
                if (dir.IndexOf("{")>-1)
                    dir = dir.Substring(0, dir.IndexOf("{"));
                Process.Start(dir);
            }
        }

        private void menuItem25_Click(object sender, EventArgs e)
        {
            var vdm = new VirtualDeviceManager();
            vdm.ShowDialog(this);
            vdm.Dispose();
        }

        private bool _resizing;
        private DateTime _lastResize = Helper.Now;
        private bool Resizing
        {

            get { return _resizing; }
            set
            {
                _resizing = value;
                _lastResize = Helper.Now;
            }
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            Resizing = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Resizing = false;
        }

        private void gridViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uploadToGoogleDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaUploadCloud();            
        }

        private void ctxtPlayer_Opening(object sender, CancelEventArgs e)
        {

        }

        private void uploadToYouTubePublicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            lock (ThreadLock)
            {
                for (int i = 0; i < flowPreview.Controls.Count; i++)
                {
                    var pb = flowPreview.Controls[i] as PreviewBox;
                    if (pb != null && pb.Selected)
                    {
                        bool b;
                        msg = YouTubeUploader.Upload(pb.Oid, pb.FileName, out b);
                    }
                }                
            }
            if (msg != "")
                MessageBox.Show(this, msg);
        }

        private void mediaPanelControl1_Load(object sender, EventArgs e)
        {

        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogFile();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = ContextTarget as ISpyControl;
            if (obj!=null)
                obj.Enable();
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = ContextTarget as ISpyControl;
            if (obj != null)
                obj.Disable();
        }

        private void alertsOnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.alerts.active = true;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.alerts.active = true;
            }
        }

        private void alertsOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.alerts.active = false;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.alerts.active = false;
            }
        }

        private void scheduleOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.schedule.active = true;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.schedule.active = true;
            }
        }

        private void scheduleOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.schedule.active = false;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.schedule.active = false;
            }
        }

        private void pTZScheduleOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.ptzschedule.active = true;
            }
        }

        private void pTZScheduleOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.ptzschedule.active = false;
            }
        }

        private void menuItem36_Click(object sender, EventArgs e)
        {
            using (var imp = new Importer())
            {
                imp.ShowDialog(this);
            }
        }

        private void menuItem37_Click(object sender, EventArgs e)
        {
            if (_cp == null)
            {
                using (_cp = new CheckPassword())
                {
                    _cp.ShowDialog(this);
                    if (_cp.DialogResult == DialogResult.OK)
                    {
                        _locked = false;
                        ShowForm(-1);
                    }
                }
                _cp = null;
            }
            
        }

        private void tagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessKey("tags");
        }

        private void openWebInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            window?.OpenWebInterface();
        }

        private void menuItem32_Click(object sender, EventArgs e)
        {
            ManageGridViews();
        }

        private Panel _lastClicked;
        private void _pnlCameras_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = _pnlCameras;
        }

        private void flCommands_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void menuItem39_Click(object sender, EventArgs e)
        {
          
            _pnlCameras.Invalidate();
        }

        private void _pnlCameras_Resize(object sender, EventArgs e)
        {
            _pnlCameras.Invalidate();
        }

        private FindObject _fo = new FindObject();
        private void menuItem40_Click(object sender, EventArgs e)
        {
            if (_fo.IsDisposed)
                _fo = new FindObject();
            _fo.Owner = this;
            _fo.Show(this);
        }

        private void _pnlCameras_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Detects d = new Detects();
            d.Show();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void _localCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(3);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddCamera(1);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AddCamera(9);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            AddCamera(5);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Detects d = new Detects();
            d.Show();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveObjectList(true);
        }

        private void openLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "BYSEC Files (*.bysec)|*.bysec|XML Files (*.xml)|*.xml";
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
                        LoadObjectList(fileName.Trim());
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                string foldername = s.Entry + "video\\";
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void fullScreenToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

            MaxMin();
        }

        private void switchONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchObjects(false, true);
        }
        
        private void switchOFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchObjects(false, false);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void aboutOurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog(this);
            form.Dispose();
        }

        private void grid4XToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            foreach (objectsCamera cam in _cameras)
            {
                
              
                CameraWindow cw = GetCamera(cam.id);

                int xpos = (_pnlCameras.Width) / 2;
                int ypost = (_pnlCameras.Height) / 2;
                if (cw.ObjectName == "Camera 0")
                {
                    cw.Select();
                    cw.Width = xpos;
                    cw.Height = ypost;

                    cw.Location = new Point(0, 0);
                  
                }
                
                if (cw.ObjectName == "Camera 1")
                {
                    cw.Select();
                    cw.Width = xpos;
                    cw.Height = ypost;
                    
                    cw.Location = new Point(xpos, 0);
                }
                if (cw.ObjectName == "Camera 2")
                {
                   
                    cw.Width = xpos;
                    cw.Height = ypost;
                    cw.Location = new Point(0, ypost);
                }
                int ypos = (_pnlCameras.Height) / 2;
                if (cw.ObjectName == "Camera 3")
                {
                    
                    cw.Width = xpos;
                    cw.Height = ypost;
                    cw.Location = new Point(xpos, ypost);
                    
                }
                
             

            }

            
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ManageGridViews();
        }

        private void _toolStripDropDownButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowPreview_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grid8XToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (objectsCamera cam in _cameras)
            {


                CameraWindow cw = GetCamera(cam.id);
                int Didth = _pnlCameras.Width / 3;
                int Didth_f = _pnlCameras.Height / 3;
                
                if (cw.ObjectName == "Camera 0")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location =  new Point(0,0);

                }
                if (cw.ObjectName == "Camera 1")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, 0);

                }
                if (cw.ObjectName == "Camera 2")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth+Didth, 0);

                }
                ////////////////////////
                if (cw.ObjectName == "Camera 3")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f);

                }
                if (cw.ObjectName == "Camera 4")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f);

                }
                if (cw.ObjectName == "Camera 5")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*2, Didth_f);

                }
                /////////////////////////////////////////////////
                if (cw.ObjectName == "Camera 6")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f*2);

                }
                if (cw.ObjectName == "Camera 7")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f*2);

                }
                if (cw.ObjectName == "Camera 8")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*2, Didth_f * 2);

                }
            }
        }

        private void grid16XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (objectsCamera cam in _cameras)
            {


                CameraWindow cw = GetCamera(cam.id);
                int Didth = _pnlCameras.Width / 4;
                int Didth_f = _pnlCameras.Height / 4;

                if (cw.ObjectName == "Camera 0")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, 0);
                }
                if (cw.ObjectName == "Camera 1")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, 0);
                }
                if (cw.ObjectName == "Camera 2")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*2, 0);
                }
                if (cw.ObjectName == "Camera 3")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*3, 0);
                }
                ///////////////////////////////////////////////////
                ////////////////////////////////////////////////
                ////2 ryad///////////////////////////////////

                if(cw.ObjectName == "Camera 4")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f);
                }
                if (cw.ObjectName == "Camera 5")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f);
                }
                if (cw.ObjectName == "Camera 6")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*2, Didth_f);
                }
                if (cw.ObjectName == "Camera 7")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth*3, Didth_f);
                }


                /////////////////////////////////////////////////////////////\
                //////////////////////////////////////////////////////////////
                //////////3ryad//////////////
                if (cw.ObjectName == "Camera 8")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f*2);
                }
                if (cw.ObjectName == "Camera 9")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f*2);
                }
                if (cw.ObjectName == "Camera 10")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f*2);
                }
                if (cw.ObjectName == "Camera 11")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, Didth_f*2);
                }
                ///////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////
                /////////////////////////4ryad///////////////////////////////////
                if (cw.ObjectName == "Camera 12")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 13")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 14")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 15")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, Didth_f * 3);
                }

                

            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            else
            {
                panel1.Visible = true;
            }
            
        }

        private void oNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void oFFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Register_License f = new Register_License();
            f.Show();
        }

        public void timer2_Tick(object sender, EventArgs e)
        {

            Intro_Box f = new Intro_Box();
            f.Close();
            
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void flowPreview_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            foreach (objectsCamera cam in _cameras)
            {


                CameraWindow cw = GetCamera(cam.id);

                int xpos = (_pnlCameras.Width) / 2;
                int ypost = (_pnlCameras.Height) / 2;
                if (cw.ObjectName == "Camera 0")
                {
                    cw.Select();
                    cw.Width = xpos;
                    cw.Height = ypost;

                    cw.Location = new Point(0, 0);

                }

                if (cw.ObjectName == "Camera 1")
                {
                    cw.Select();
                    cw.Width = xpos;
                    cw.Height = ypost;

                    cw.Location = new Point(xpos, 0);
                }
                if (cw.ObjectName == "Camera 2")
                {

                    cw.Width = xpos;
                    cw.Height = ypost;
                    cw.Location = new Point(0, ypost);
                }
                int ypos = (_pnlCameras.Height) / 2;
                if (cw.ObjectName == "Camera 3")
                {

                    cw.Width = xpos;
                    cw.Height = ypost;
                    cw.Location = new Point(xpos, ypost);

                }



            }


        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            foreach (objectsCamera cam in _cameras)
            {


                CameraWindow cw = GetCamera(cam.id);
                int Didth = _pnlCameras.Width / 3;
                int Didth_f = _pnlCameras.Height / 3;

                if (cw.ObjectName == "Camera 0")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, 0);

                }
                if (cw.ObjectName == "Camera 1")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, 0);

                }
                if (cw.ObjectName == "Camera 2")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth + Didth, 0);

                }
                ////////////////////////
                if (cw.ObjectName == "Camera 3")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f);

                }
                if (cw.ObjectName == "Camera 4")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f);

                }
                if (cw.ObjectName == "Camera 5")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f);

                }
                /////////////////////////////////////////////////
                if (cw.ObjectName == "Camera 6")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f * 2);

                }
                if (cw.ObjectName == "Camera 7")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f * 2);

                }
                if (cw.ObjectName == "Camera 8")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f * 2);

                }
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            foreach (objectsCamera cam in _cameras)
            {


                CameraWindow cw = GetCamera(cam.id);
                int Didth = _pnlCameras.Width / 4;
                int Didth_f = _pnlCameras.Height / 4;

                if (cw.ObjectName == "Camera 0")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, 0);
                }
                if (cw.ObjectName == "Camera 1")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, 0);
                }
                if (cw.ObjectName == "Camera 2")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, 0);
                }
                if (cw.ObjectName == "Camera 3")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, 0);
                }
                ///////////////////////////////////////////////////
                ////////////////////////////////////////////////
                ////2 ryad///////////////////////////////////

                if (cw.ObjectName == "Camera 4")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f);
                }
                if (cw.ObjectName == "Camera 5")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f);
                }
                if (cw.ObjectName == "Camera 6")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f);
                }
                if (cw.ObjectName == "Camera 7")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, Didth_f);
                }


                /////////////////////////////////////////////////////////////\
                //////////////////////////////////////////////////////////////
                //////////3ryad//////////////
                if (cw.ObjectName == "Camera 8")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f * 2);
                }
                if (cw.ObjectName == "Camera 9")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f * 2);
                }
                if (cw.ObjectName == "Camera 10")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f * 2);
                }
                if (cw.ObjectName == "Camera 11")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, Didth_f * 2);
                }
                ///////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////
                /////////////////////////4ryad///////////////////////////////////
                if (cw.ObjectName == "Camera 12")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(0, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 13")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 14")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 2, Didth_f * 3);
                }
                if (cw.ObjectName == "Camera 15")
                {
                    cw.Width = Didth;
                    cw.Height = Didth_f;
                    cw.Location = new Point(Didth * 3, Didth_f * 3);
                }



            }
        }

        private void menuItem33_Click(object sender, EventArgs e)
        {
            _locked = true;
            WindowState = FormWindowState.Minimized;
        }

        private void tsslPRO_Click(object sender, EventArgs e)
        {
            OpenUrl(Website + "/download-agent.aspx");
        }

        private void ctxtMnu_Opening(object sender, CancelEventArgs e)
        {
         
            _deleteToolStripMenuItem.Visible = _editToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Edit);
          
                    _showFilesToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Access_Media);
            
        }


        private CommandButtons cmdButtons = null;
        private void menuItem35_Click(object sender, EventArgs e)
        {
            ShowCommandButtonWindow();
        }

        internal void ShowCommandButtonWindow()
        {
            if (cmdButtons != null)
            {
                cmdButtons.Close();
                cmdButtons.Dispose();
            }

            cmdButtons = new CommandButtons();
            cmdButtons.Show(this);
        }
    }
}