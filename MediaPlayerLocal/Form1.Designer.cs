using MediaPlayerLocal;

namespace local
{
    partial class Form1
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
            this.buttonPlayMedia = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.listBoxTitles = new System.Windows.Forms.ListBox();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.videoView = new LibVLCSharp.WinForms.VideoView();
            this.panelVideo = new System.Windows.Forms.Panel();
            this.buttonRepeat = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.mediaProgressBarAudio = new MediaPlayerLocal.MediaProgressBar();
            this.labelMediaTimeSpan = new System.Windows.Forms.Label();
            this.mediaProgressBar = new MediaPlayerLocal.MediaProgressBar();
            this.textBoxCurrentMediaTitle = new System.Windows.Forms.TextBox();
            this.panelPlaylist = new System.Windows.Forms.Panel();
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.menuStripSubtitle = new System.Windows.Forms.MenuStrip();
            this.subtitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).BeginInit();
            this.panelVideo.SuspendLayout();
            this.panelPlaylist.SuspendLayout();
            this.menuStripSubtitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPlayMedia
            // 
            this.buttonPlayMedia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlayMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonPlayMedia.Location = new System.Drawing.Point(18, 476);
            this.buttonPlayMedia.Name = "buttonPlayMedia";
            this.buttonPlayMedia.Size = new System.Drawing.Size(177, 52);
            this.buttonPlayMedia.TabIndex = 0;
            this.buttonPlayMedia.Text = "PLAY";
            this.buttonPlayMedia.UseVisualStyleBackColor = true;
            this.buttonPlayMedia.Click += new System.EventHandler(this.buttonPlayMedia_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonOpenFile.Location = new System.Drawing.Point(651, 474);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(221, 54);
            this.buttonOpenFile.TabIndex = 1;
            this.buttonOpenFile.Text = "OPEN";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // listBoxTitles
            // 
            this.listBoxTitles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTitles.FormattingEnabled = true;
            this.listBoxTitles.ItemHeight = 16;
            this.listBoxTitles.Location = new System.Drawing.Point(3, 20);
            this.listBoxTitles.Name = "listBoxTitles";
            this.listBoxTitles.Size = new System.Drawing.Size(314, 468);
            this.listBoxTitles.TabIndex = 3;
            this.listBoxTitles.SelectedIndexChanged += new System.EventHandler(this.listBoxTitles_SelectedIndexChanged);
            this.listBoxTitles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxTitles_DoubleClick);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonLoadFile.Location = new System.Drawing.Point(107, 512);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(210, 54);
            this.buttonLoadFile.TabIndex = 4;
            this.buttonLoadFile.Text = "LOAD MEDIA";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // videoView
            // 
            this.videoView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoView.BackColor = System.Drawing.Color.Black;
            this.videoView.Location = new System.Drawing.Point(18, 29);
            this.videoView.MediaPlayer = null;
            this.videoView.Name = "videoView";
            this.videoView.Size = new System.Drawing.Size(854, 336);
            this.videoView.TabIndex = 5;
            this.videoView.Text = "videoView";
            // 
            // panelVideo
            // 
            this.panelVideo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelVideo.Controls.Add(this.buttonRepeat);
            this.panelVideo.Controls.Add(this.buttonStop);
            this.panelVideo.Controls.Add(this.mediaProgressBarAudio);
            this.panelVideo.Controls.Add(this.labelMediaTimeSpan);
            this.panelVideo.Controls.Add(this.mediaProgressBar);
            this.panelVideo.Controls.Add(this.textBoxCurrentMediaTitle);
            this.panelVideo.Controls.Add(this.videoView);
            this.panelVideo.Controls.Add(this.buttonOpenFile);
            this.panelVideo.Controls.Add(this.buttonPlayMedia);
            this.panelVideo.Location = new System.Drawing.Point(1, 36);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(892, 542);
            this.panelVideo.TabIndex = 7;
            // 
            // buttonRepeat
            // 
            this.buttonRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRepeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonRepeat.Location = new System.Drawing.Point(384, 478);
            this.buttonRepeat.Name = "buttonRepeat";
            this.buttonRepeat.Size = new System.Drawing.Size(177, 52);
            this.buttonRepeat.TabIndex = 15;
            this.buttonRepeat.Text = "REPEAT";
            this.buttonRepeat.UseVisualStyleBackColor = true;
            this.buttonRepeat.Click += new System.EventHandler(this.buttonRepeat_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonStop.Location = new System.Drawing.Point(201, 477);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(177, 52);
            this.buttonStop.TabIndex = 14;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // mediaProgressBarAudio
            // 
            this.mediaProgressBarAudio.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaProgressBarAudio.Location = new System.Drawing.Point(696, 440);
            this.mediaProgressBarAudio.Name = "mediaProgressBarAudio";
            this.mediaProgressBarAudio.Size = new System.Drawing.Size(177, 25);
            this.mediaProgressBarAudio.TabIndex = 13;
            this.mediaProgressBarAudio.Value = 0.5F;
            this.mediaProgressBarAudio.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBarAudio_MouseMove);
            this.mediaProgressBarAudio.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBarAudio_MouseUp);
            // 
            // labelMediaTimeSpan
            // 
            this.labelMediaTimeSpan.AutoSize = true;
            this.labelMediaTimeSpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.labelMediaTimeSpan.Location = new System.Drawing.Point(21, 443);
            this.labelMediaTimeSpan.Name = "labelMediaTimeSpan";
            this.labelMediaTimeSpan.Size = new System.Drawing.Size(165, 22);
            this.labelMediaTimeSpan.TabIndex = 12;
            this.labelMediaTimeSpan.Text = "00:00:00 / 00:00:00";
            // 
            // mediaProgressBar
            // 
            this.mediaProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaProgressBar.Location = new System.Drawing.Point(18, 407);
            this.mediaProgressBar.Name = "mediaProgressBar";
            this.mediaProgressBar.Size = new System.Drawing.Size(855, 33);
            this.mediaProgressBar.TabIndex = 11;
            this.mediaProgressBar.Value = 0F;
            this.mediaProgressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseDown);
            this.mediaProgressBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseMove);
            this.mediaProgressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseUp);
            // 
            // textBoxCurrentMediaTitle
            // 
            this.textBoxCurrentMediaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrentMediaTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurrentMediaTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCurrentMediaTitle.Location = new System.Drawing.Point(18, 20);
            this.textBoxCurrentMediaTitle.Name = "textBoxCurrentMediaTitle";
            this.textBoxCurrentMediaTitle.Size = new System.Drawing.Size(854, 21);
            this.textBoxCurrentMediaTitle.TabIndex = 8;
            // 
            // panelPlaylist
            // 
            this.panelPlaylist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelPlaylist.Controls.Add(this.listBoxTitles);
            this.panelPlaylist.Controls.Add(this.buttonLoadFile);
            this.panelPlaylist.Location = new System.Drawing.Point(899, 0);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(335, 578);
            this.panelPlaylist.TabIndex = 8;
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Interval = 500;
            // 
            // menuStripSubtitle
            // 
            this.menuStripSubtitle.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripSubtitle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleToolStripMenuItem});
            this.menuStripSubtitle.Location = new System.Drawing.Point(0, 0);
            this.menuStripSubtitle.Name = "menuStripSubtitle";
            this.menuStripSubtitle.Size = new System.Drawing.Size(1234, 28);
            this.menuStripSubtitle.TabIndex = 10;
            // 
            // subtitleToolStripMenuItem
            // 
            this.subtitleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleAddToolStripMenuItem,
            this.subtitleListToolStripMenuItem});
            this.subtitleToolStripMenuItem.Name = "subtitleToolStripMenuItem";
            this.subtitleToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.subtitleToolStripMenuItem.Text = "Subtitle";
            // 
            // subtitleAddToolStripMenuItem
            // 
            this.subtitleAddToolStripMenuItem.Enabled = false;
            this.subtitleAddToolStripMenuItem.Name = "subtitleAddToolStripMenuItem";
            this.subtitleAddToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.subtitleAddToolStripMenuItem.Text = "Add subtitle file";
            this.subtitleAddToolStripMenuItem.Click += new System.EventHandler(this.addSubtitleFileToolStripMenuItem_Click);
            // 
            // subtitleListToolStripMenuItem
            // 
            this.subtitleListToolStripMenuItem.Enabled = false;
            this.subtitleListToolStripMenuItem.Name = "subtitleListToolStripMenuItem";
            this.subtitleListToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.subtitleListToolStripMenuItem.Text = "Subtitle list";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 578);
            this.Controls.Add(this.menuStripSubtitle);
            this.Controls.Add(this.panelPlaylist);
            this.Controls.Add(this.panelVideo);
            this.MainMenuStrip = this.menuStripSubtitle;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            this.panelVideo.ResumeLayout(false);
            this.panelVideo.PerformLayout();
            this.panelPlaylist.ResumeLayout(false);
            this.menuStripSubtitle.ResumeLayout(false);
            this.menuStripSubtitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPlayMedia;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.ListBox listBoxTitles;
        private System.Windows.Forms.Button buttonLoadFile;
        private LibVLCSharp.WinForms.VideoView videoView;
        private System.Windows.Forms.TextBox textBoxCurrentMediaTitle;
        private System.Windows.Forms.Panel panelVideo;
        private System.Windows.Forms.Panel panelPlaylist;
        private System.Windows.Forms.Timer timerUpdateUI;

        private MediaProgressBar mediaProgressBar;
        private System.Windows.Forms.Label labelMediaTimeSpan;
        private MediaProgressBar mediaProgressBarAudio;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonRepeat;
        private System.Windows.Forms.MenuStrip menuStripSubtitle;
        private System.Windows.Forms.ToolStripMenuItem subtitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitleAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitleListToolStripMenuItem;
    }
}

