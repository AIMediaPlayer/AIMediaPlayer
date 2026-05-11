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
            this.buttonRepeatPlaylist = new System.Windows.Forms.Button();
            this.buttonSavePlaylist = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonShuffle = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
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
            this.buttonPlayMedia.Location = new System.Drawing.Point(14, 426);
            this.buttonPlayMedia.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlayMedia.Name = "buttonPlayMedia";
            this.buttonPlayMedia.Size = new System.Drawing.Size(114, 42);
            this.buttonPlayMedia.TabIndex = 0;
            this.buttonPlayMedia.Text = "PLAY";
            this.buttonPlayMedia.UseVisualStyleBackColor = true;
            this.buttonPlayMedia.Click += new System.EventHandler(this.buttonPlayMedia_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonOpenFile.Location = new System.Drawing.Point(5, 412);
            this.buttonOpenFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(113, 48);
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
            this.listBoxTitles.Location = new System.Drawing.Point(5, 11);
            this.listBoxTitles.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxTitles.Name = "listBoxTitles";
            this.listBoxTitles.Size = new System.Drawing.Size(236, 381);
            this.listBoxTitles.TabIndex = 3;
            this.listBoxTitles.SelectedIndexChanged += new System.EventHandler(this.listBoxTitles_SelectedIndexChanged);
            this.listBoxTitles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxTitles_DoubleClick);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonLoadFile.Location = new System.Drawing.Point(128, 412);
            this.buttonLoadFile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(113, 48);
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
            this.videoView.Location = new System.Drawing.Point(14, 24);
            this.videoView.Margin = new System.Windows.Forms.Padding(2);
            this.videoView.MediaPlayer = null;
            this.videoView.Name = "videoView";
            this.videoView.Size = new System.Drawing.Size(641, 302);
            this.videoView.TabIndex = 5;
            this.videoView.Text = "videoView";
            // 
            // panelVideo
            // 
            this.panelVideo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelVideo.Controls.Add(this.buttonRepeatPlaylist);
            this.panelVideo.Controls.Add(this.buttonSavePlaylist);
            this.panelVideo.Controls.Add(this.buttonLoad);
            this.panelVideo.Controls.Add(this.buttonSave);
            this.panelVideo.Controls.Add(this.buttonShuffle);
            this.panelVideo.Controls.Add(this.buttonPrevious);
            this.panelVideo.Controls.Add(this.buttonNext);
            this.panelVideo.Controls.Add(this.buttonRemove);
            this.panelVideo.Controls.Add(this.buttonStop);
            this.panelVideo.Controls.Add(this.mediaProgressBarAudio);
            this.panelVideo.Controls.Add(this.labelMediaTimeSpan);
            this.panelVideo.Controls.Add(this.mediaProgressBar);
            this.panelVideo.Controls.Add(this.textBoxCurrentMediaTitle);
            this.panelVideo.Controls.Add(this.videoView);
            this.panelVideo.Controls.Add(this.buttonPlayMedia);
            this.panelVideo.Location = new System.Drawing.Point(0, 0);
            this.panelVideo.Margin = new System.Windows.Forms.Padding(2);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(670, 470);
            this.panelVideo.TabIndex = 7;
            // 
            // labelMediaTimeSpan
            // 
            this.buttonRepeatPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRepeatPlaylist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonRepeatPlaylist.Location = new System.Drawing.Point(486, 428);
            this.buttonRepeatPlaylist.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRepeatPlaylist.Name = "buttonRepeatPlaylist";
            this.buttonRepeatPlaylist.Size = new System.Drawing.Size(169, 42);
            this.buttonRepeatPlaylist.TabIndex = 22;
            this.buttonRepeatPlaylist.Text = "REPEAT PLAYLIST";
            this.buttonRepeatPlaylist.UseVisualStyleBackColor = true;
            this.buttonRepeatPlaylist.Click += new System.EventHandler(this.buttonRepeatPlaylist_Click);
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
            this.buttonSavePlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSavePlaylist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonSavePlaylist.Location = new System.Drawing.Point(486, 380);
            this.buttonSavePlaylist.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSavePlaylist.Name = "buttonSavePlaylist";
            this.buttonSavePlaylist.Size = new System.Drawing.Size(169, 42);
            this.buttonSavePlaylist.TabIndex = 21;
            this.buttonSavePlaylist.Text = "SAVE PLAYLIST";
            this.buttonSavePlaylist.UseVisualStyleBackColor = true;
            this.buttonSavePlaylist.Click += new System.EventHandler(this.buttonSavePlaylist_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonLoad.Location = new System.Drawing.Point(368, 426);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(114, 42);
            this.buttonLoad.TabIndex = 20;
            this.buttonLoad.Text = "LOAD";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonSave.Location = new System.Drawing.Point(368, 380);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(114, 42);
            this.buttonSave.TabIndex = 19;
            this.buttonSave.Text = "SAVE";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonShuffle
            // 
            this.buttonShuffle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonShuffle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonShuffle.Location = new System.Drawing.Point(250, 380);
            this.buttonShuffle.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShuffle.Name = "buttonShuffle";
            this.buttonShuffle.Size = new System.Drawing.Size(114, 42);
            this.buttonShuffle.TabIndex = 18;
            this.buttonShuffle.Text = "SHUFFLE";
            this.buttonShuffle.UseVisualStyleBackColor = true;
            this.buttonShuffle.Click += new System.EventHandler(this.buttonShuffle_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonPrevious.Location = new System.Drawing.Point(132, 380);
            this.buttonPrevious.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(114, 42);
            this.buttonPrevious.TabIndex = 17;
            this.buttonPrevious.Text = "PREVIOUS";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonNext.Location = new System.Drawing.Point(14, 380);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(114, 42);
            this.buttonNext.TabIndex = 16;
            this.buttonNext.Text = "NEXT";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonRemove.Location = new System.Drawing.Point(250, 426);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(114, 42);
            this.buttonRemove.TabIndex = 15;
            this.buttonRemove.Text = "REMOVE";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // menuStripSubtitle
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonStop.Location = new System.Drawing.Point(131, 426);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(115, 42);
            this.buttonStop.TabIndex = 14;
            this.buttonStop.Text = "STOP";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
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
            this.mediaProgressBarAudio.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaProgressBarAudio.Location = new System.Drawing.Point(522, 358);
            this.mediaProgressBarAudio.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mediaProgressBarAudio.Name = "mediaProgressBarAudio";
            this.mediaProgressBarAudio.Size = new System.Drawing.Size(133, 20);
            this.mediaProgressBarAudio.TabIndex = 13;
            this.mediaProgressBarAudio.Value = 0.5F;
            this.mediaProgressBarAudio.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBarAudio_MouseMove);
            this.mediaProgressBarAudio.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBarAudio_MouseUp);
            // 
            // labelMediaTimeSpan
            // 
            this.labelMediaTimeSpan.AutoSize = true;
            this.labelMediaTimeSpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.labelMediaTimeSpan.Location = new System.Drawing.Point(16, 360);
            this.labelMediaTimeSpan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMediaTimeSpan.Name = "labelMediaTimeSpan";
            this.labelMediaTimeSpan.Size = new System.Drawing.Size(132, 18);
            this.labelMediaTimeSpan.TabIndex = 12;
            this.labelMediaTimeSpan.Text = "00:00:00 / 00:00:00";
            // 
            // mediaProgressBar
            // 
            this.mediaProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaProgressBar.Location = new System.Drawing.Point(14, 331);
            this.mediaProgressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mediaProgressBar.Name = "mediaProgressBar";
            this.mediaProgressBar.Size = new System.Drawing.Size(641, 27);
            this.mediaProgressBar.TabIndex = 11;
            this.mediaProgressBar.Value = 0F;
            this.mediaProgressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseDown);
            this.mediaProgressBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseMove);
            this.mediaProgressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseUp);
            this.subtitleAddToolStripMenuItem.Enabled = false;
            this.subtitleAddToolStripMenuItem.Name = "subtitleAddToolStripMenuItem";
            this.subtitleAddToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.subtitleAddToolStripMenuItem.Text = "Add subtitle file";
            this.subtitleAddToolStripMenuItem.Click += new System.EventHandler(this.addSubtitleFileToolStripMenuItem_Click);
            // 
            // textBoxCurrentMediaTitle
            // 
            this.textBoxCurrentMediaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrentMediaTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurrentMediaTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCurrentMediaTitle.Location = new System.Drawing.Point(14, 16);
            this.textBoxCurrentMediaTitle.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCurrentMediaTitle.Name = "textBoxCurrentMediaTitle";
            this.textBoxCurrentMediaTitle.Size = new System.Drawing.Size(641, 17);
            this.textBoxCurrentMediaTitle.TabIndex = 8;
            // 
            // panelPlaylist
            // 
            this.panelPlaylist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelPlaylist.Controls.Add(this.listBoxTitles);
            this.panelPlaylist.Controls.Add(this.buttonLoadFile);
            this.panelPlaylist.Controls.Add(this.buttonOpenFile);
            this.panelPlaylist.Location = new System.Drawing.Point(674, 0);
            this.panelPlaylist.Margin = new System.Windows.Forms.Padding(2);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(251, 470);
            this.panelPlaylist.TabIndex = 8;
            // 
            // timerUpdateUI
            // 
            this.subtitleListToolStripMenuItem.Enabled = false;
            this.subtitleListToolStripMenuItem.Name = "subtitleListToolStripMenuItem";
            this.subtitleListToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.subtitleListToolStripMenuItem.Text = "Subtitle list";
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Interval = 500;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 470);
            this.Controls.Add(this.panelPlaylist);
            this.Controls.Add(this.panelVideo);
            this.MainMenuStrip = this.menuStripSubtitle;
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonShuffle;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonRepeatPlaylist;
        private System.Windows.Forms.Button buttonSavePlaylist;
        private System.Windows.Forms.Button buttonRepeat;
        private System.Windows.Forms.MenuStrip menuStripSubtitle;
        private System.Windows.Forms.ToolStripMenuItem subtitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitleAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitleListToolStripMenuItem;
    }
}

