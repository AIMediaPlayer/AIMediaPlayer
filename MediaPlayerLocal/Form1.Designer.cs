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
            this.labelMediaTimeSpan = new System.Windows.Forms.Label();
            this.textBoxCurrentMediaTitle = new System.Windows.Forms.TextBox();
            this.panelPlaylist = new System.Windows.Forms.Panel();
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.mediaProgressBar = new MediaPlayerLocal.MediaProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).BeginInit();
            this.panelVideo.SuspendLayout();
            this.panelPlaylist.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPlayMedia
            // 
            this.buttonPlayMedia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlayMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.12F);
            this.buttonPlayMedia.Location = new System.Drawing.Point(18, 512);
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
            this.buttonOpenFile.Location = new System.Drawing.Point(652, 510);
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
            this.videoView.Size = new System.Drawing.Size(855, 372);
            this.videoView.TabIndex = 5;
            this.videoView.Text = "videoView";
            // 
            // panelVideo
            // 
            this.panelVideo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelVideo.Controls.Add(this.labelMediaTimeSpan);
            this.panelVideo.Controls.Add(this.mediaProgressBar);
            this.panelVideo.Controls.Add(this.textBoxCurrentMediaTitle);
            this.panelVideo.Controls.Add(this.videoView);
            this.panelVideo.Controls.Add(this.buttonOpenFile);
            this.panelVideo.Controls.Add(this.buttonPlayMedia);
            this.panelVideo.Location = new System.Drawing.Point(0, 0);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(893, 578);
            this.panelVideo.TabIndex = 7;
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
            // textBoxCurrentMediaTitle
            // 
            this.textBoxCurrentMediaTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrentMediaTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurrentMediaTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCurrentMediaTitle.Location = new System.Drawing.Point(18, 20);
            this.textBoxCurrentMediaTitle.Name = "textBoxCurrentMediaTitle";
            this.textBoxCurrentMediaTitle.Size = new System.Drawing.Size(855, 21);
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
            // mediaProgressBar
            // 
            this.mediaProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mediaProgressBar.Location = new System.Drawing.Point(18, 407);
            this.mediaProgressBar.Name = "mediaProgressBar";
            this.mediaProgressBar.Size = new System.Drawing.Size(855, 33);
            this.mediaProgressBar.TabIndex = 11;
            this.mediaProgressBar.Value = 0F;
            this.mediaProgressBar.Load += new System.EventHandler(this.mediaProgressBar_Load);
            this.mediaProgressBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseDown);
            this.mediaProgressBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseMove);
            this.mediaProgressBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mediaProgressBar_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 578);
            this.Controls.Add(this.panelPlaylist);
            this.Controls.Add(this.panelVideo);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            this.panelVideo.ResumeLayout(false);
            this.panelVideo.PerformLayout();
            this.panelPlaylist.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}

