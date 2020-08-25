using iSpyApplication.Controls;

namespace iSpyApplication
{
    partial class VideoSource
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoSource));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.onvifWizard1 = new iSpyApplication.Controls.ONVIFWizard();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.videoResolutionsCombo = new System.Windows.Forms.ComboBox();
            this.devicesCombo = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.videoInputsCombo = new System.Windows.Forms.ComboBox();
            this.snapshotResolutionsCombo = new System.Windows.Forms.ComboBox();
            this.chkAutoImageSettings = new System.Windows.Forms.CheckBox();
            this.snapshotsLabel = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoCaptureVideo = new System.Windows.Forms.RadioButton();
            this.rdoCaptureSnapshots = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLogin2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDecodeKey = new System.Windows.Forms.TextBox();
            this.cmbMJPEGURL = new System.Windows.Forms.ComboBox();
            this.tcSource = new System.Windows.Forms.TabControl();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tcSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(328, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 33);
            this.button1.TabIndex = 9;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(230, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 33);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(331, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(382, 35);
            this.flowLayoutPanel1.TabIndex = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 536);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(713, 35);
            this.panel2.TabIndex = 61;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.onvifWizard1);
            this.tabPage10.Controls.Add(this.splitter1);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(705, 507);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "ONVIF";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // onvifWizard1
            // 
            this.onvifWizard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.onvifWizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.onvifWizard1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.onvifWizard1.ForeColor = System.Drawing.Color.White;
            this.onvifWizard1.Location = new System.Drawing.Point(3, 0);
            this.onvifWizard1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.onvifWizard1.Name = "onvifWizard1";
            this.onvifWizard1.Size = new System.Drawing.Size(702, 507);
            this.onvifWizard1.TabIndex = 59;
            this.onvifWizard1.Load += new System.EventHandler(this.onvifWizard1_Load);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 507);
            this.splitter1.TabIndex = 58;
            this.splitter1.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage4.Controls.Add(this.tableLayoutPanel4);
            this.tabPage4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage4.ForeColor = System.Drawing.Color.White;
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage4.Size = new System.Drawing.Size(705, 507);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Local Device";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.93408F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.06592F));
            this.tableLayoutPanel4.Controls.Add(this.label39, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label38, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.videoResolutionsCombo, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.devicesCombo, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label37, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.videoInputsCombo, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.snapshotResolutionsCombo, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.chkAutoImageSettings, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.snapshotsLabel, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label35, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel5, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(693, 229);
            this.tableLayoutPanel4.TabIndex = 22;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(6, 6);
            this.label39.Margin = new System.Windows.Forms.Padding(6);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(106, 16);
            this.label39.TabIndex = 11;
            this.label39.Text = "Video device:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 99);
            this.label38.Margin = new System.Windows.Forms.Padding(6);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(82, 19);
            this.label38.TabIndex = 12;
            this.label38.Text = "Video resoluton:";
            // 
            // videoResolutionsCombo
            // 
            this.videoResolutionsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoResolutionsCombo.FormattingEnabled = true;
            this.videoResolutionsCombo.Location = new System.Drawing.Point(134, 96);
            this.videoResolutionsCombo.Name = "videoResolutionsCombo";
            this.videoResolutionsCombo.Size = new System.Drawing.Size(127, 24);
            this.videoResolutionsCombo.TabIndex = 13;
            this.videoResolutionsCombo.SelectedIndexChanged += new System.EventHandler(this.videoResolutionsCombo_SelectedIndexChanged);
            // 
            // devicesCombo
            // 
            this.devicesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicesCombo.FormattingEnabled = true;
            this.devicesCombo.Location = new System.Drawing.Point(134, 3);
            this.devicesCombo.Name = "devicesCombo";
            this.devicesCombo.Size = new System.Drawing.Size(127, 24);
            this.devicesCombo.TabIndex = 9;
            this.devicesCombo.SelectedIndexChanged += new System.EventHandler(this.devicesCombo_SelectedIndexChanged_1);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 37);
            this.label37.Margin = new System.Windows.Forms.Padding(6);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(95, 16);
            this.label37.TabIndex = 16;
            this.label37.Text = "Video input:";
            // 
            // videoInputsCombo
            // 
            this.videoInputsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.videoInputsCombo.FormattingEnabled = true;
            this.videoInputsCombo.Location = new System.Drawing.Point(134, 34);
            this.videoInputsCombo.Name = "videoInputsCombo";
            this.videoInputsCombo.Size = new System.Drawing.Size(127, 24);
            this.videoInputsCombo.TabIndex = 17;
            this.videoInputsCombo.SelectedIndexChanged += new System.EventHandler(this.videoInputsCombo_SelectedIndexChanged);
            // 
            // snapshotResolutionsCombo
            // 
            this.snapshotResolutionsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.snapshotResolutionsCombo.FormattingEnabled = true;
            this.snapshotResolutionsCombo.Location = new System.Drawing.Point(134, 127);
            this.snapshotResolutionsCombo.Name = "snapshotResolutionsCombo";
            this.snapshotResolutionsCombo.Size = new System.Drawing.Size(127, 24);
            this.snapshotResolutionsCombo.TabIndex = 14;
            this.snapshotResolutionsCombo.SelectedIndexChanged += new System.EventHandler(this.snapshotResolutionsCombo_SelectedIndexChanged);
            // 
            // chkAutoImageSettings
            // 
            this.chkAutoImageSettings.AutoSize = true;
            this.chkAutoImageSettings.Location = new System.Drawing.Point(137, 161);
            this.chkAutoImageSettings.Margin = new System.Windows.Forms.Padding(6);
            this.chkAutoImageSettings.Name = "chkAutoImageSettings";
            this.chkAutoImageSettings.Size = new System.Drawing.Size(217, 20);
            this.chkAutoImageSettings.TabIndex = 18;
            this.chkAutoImageSettings.Text = "Automatic Image Settings";
            this.chkAutoImageSettings.UseVisualStyleBackColor = true;
            // 
            // snapshotsLabel
            // 
            this.snapshotsLabel.AutoSize = true;
            this.snapshotsLabel.Location = new System.Drawing.Point(6, 130);
            this.snapshotsLabel.Margin = new System.Windows.Forms.Padding(6);
            this.snapshotsLabel.Name = "snapshotsLabel";
            this.snapshotsLabel.Size = new System.Drawing.Size(82, 19);
            this.snapshotsLabel.TabIndex = 15;
            this.snapshotsLabel.Text = "Snapshot resoluton:";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 68);
            this.label35.Margin = new System.Windows.Forms.Padding(6);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(116, 16);
            this.label35.TabIndex = 19;
            this.label35.Text = "Capture mode:";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.rdoCaptureVideo);
            this.flowLayoutPanel5.Controls.Add(this.rdoCaptureSnapshots);
            this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(131, 62);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(562, 31);
            this.flowLayoutPanel5.TabIndex = 20;
            // 
            // rdoCaptureVideo
            // 
            this.rdoCaptureVideo.AutoSize = true;
            this.rdoCaptureVideo.Location = new System.Drawing.Point(6, 6);
            this.rdoCaptureVideo.Margin = new System.Windows.Forms.Padding(6);
            this.rdoCaptureVideo.Name = "rdoCaptureVideo";
            this.rdoCaptureVideo.Size = new System.Drawing.Size(67, 20);
            this.rdoCaptureVideo.TabIndex = 0;
            this.rdoCaptureVideo.TabStop = true;
            this.rdoCaptureVideo.Text = "Video";
            this.rdoCaptureVideo.UseVisualStyleBackColor = true;
            // 
            // rdoCaptureSnapshots
            // 
            this.rdoCaptureSnapshots.AutoSize = true;
            this.rdoCaptureSnapshots.Location = new System.Drawing.Point(85, 6);
            this.rdoCaptureSnapshots.Margin = new System.Windows.Forms.Padding(6);
            this.rdoCaptureSnapshots.Name = "rdoCaptureSnapshots";
            this.rdoCaptureSnapshots.Size = new System.Drawing.Size(102, 20);
            this.rdoCaptureSnapshots.TabIndex = 1;
            this.rdoCaptureSnapshots.TabStop = true;
            this.rdoCaptureSnapshots.Text = "Snapshots";
            this.rdoCaptureSnapshots.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage2.Size = new System.Drawing.Size(705, 507);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "MJPEG URL";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLogin2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtPassword2, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label20, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtDecodeKey, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.cmbMJPEGURL, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(693, 117);
            this.tableLayoutPanel2.TabIndex = 49;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(3, 8);
            this.label15.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(82, 16);
            this.label15.TabIndex = 41;
            this.label15.Text = "Username";
            // 
            // txtLogin2
            // 
            this.txtLogin2.Location = new System.Drawing.Point(102, 3);
            this.txtLogin2.Name = "txtLogin2";
            this.txtLogin2.Size = new System.Drawing.Size(156, 23);
            this.txtLogin2.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "MJPEG URL";
            // 
            // txtPassword2
            // 
            this.txtPassword2.Location = new System.Drawing.Point(102, 32);
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.PasswordChar = '*';
            this.txtPassword2.Size = new System.Drawing.Size(156, 23);
            this.txtPassword2.TabIndex = 44;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(3, 37);
            this.label17.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(78, 16);
            this.label17.TabIndex = 42;
            this.label17.Text = "Password";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(3, 96);
            this.label20.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(93, 16);
            this.label20.TabIndex = 51;
            this.label20.Text = "Decode Key";
            // 
            // txtDecodeKey
            // 
            this.txtDecodeKey.Location = new System.Drawing.Point(102, 91);
            this.txtDecodeKey.Name = "txtDecodeKey";
            this.txtDecodeKey.PasswordChar = '*';
            this.txtDecodeKey.Size = new System.Drawing.Size(223, 23);
            this.txtDecodeKey.TabIndex = 52;
            // 
            // cmbMJPEGURL
            // 
            this.cmbMJPEGURL.FormattingEnabled = true;
            this.cmbMJPEGURL.Location = new System.Drawing.Point(102, 61);
            this.cmbMJPEGURL.Name = "cmbMJPEGURL";
            this.cmbMJPEGURL.Size = new System.Drawing.Size(465, 24);
            this.cmbMJPEGURL.TabIndex = 13;
            this.cmbMJPEGURL.SelectedIndexChanged += new System.EventHandler(this.cmbMJPEGURL_SelectedIndexChanged);
            this.cmbMJPEGURL.Click += new System.EventHandler(this.cmbMJPEGURL_Click);
            // 
            // tcSource
            // 
            this.tcSource.Controls.Add(this.tabPage2);
            this.tcSource.Controls.Add(this.tabPage4);
            this.tcSource.Controls.Add(this.tabPage10);
            this.tcSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSource.Location = new System.Drawing.Point(3, 3);
            this.tcSource.Name = "tcSource";
            this.tcSource.SelectedIndex = 0;
            this.tcSource.Size = new System.Drawing.Size(713, 533);
            this.tcSource.TabIndex = 57;
            this.tcSource.SelectedIndexChanged += new System.EventHandler(this.tcSource_SelectedIndexChanged);
            // 
            // VideoSource
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(719, 574);
            this.Controls.Add(this.tcSource);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "VideoSource";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Video Source";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoSource_FormClosing);
            this.Load += new System.EventHandler(this.VideoSourceLoad);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tcSource.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPage10;
        private ONVIFWizard onvifWizard1;
        private System.Windows.Forms.Splitter splitter1;
        public System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox videoResolutionsCombo;
        private System.Windows.Forms.ComboBox devicesCombo;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox videoInputsCombo;
        private System.Windows.Forms.ComboBox snapshotResolutionsCombo;
        private System.Windows.Forms.CheckBox chkAutoImageSettings;
        private System.Windows.Forms.Label snapshotsLabel;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.RadioButton rdoCaptureVideo;
        private System.Windows.Forms.RadioButton rdoCaptureSnapshots;
        public System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtLogin2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDecodeKey;
        private System.Windows.Forms.ComboBox cmbMJPEGURL;
        public System.Windows.Forms.TabControl tcSource;
    }
}