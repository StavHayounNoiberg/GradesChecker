namespace GradesChecker
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.StopAndExitButton = new System.Windows.Forms.Button();
            this.UpdateSettingsButton = new System.Windows.Forms.Button();
            this.IntervalTimePick = new System.Windows.Forms.NumericUpDown();
            this.SleepTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.CheckInterval_label1 = new System.Windows.Forms.Label();
            this.CheckInterval_label2 = new System.Windows.Forms.Label();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.Logo1 = new System.Windows.Forms.Label();
            this.Logo2 = new System.Windows.Forms.Label();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.CheckTimer = new System.Windows.Forms.Timer(this.components);
            this.CheckGradesBW = new System.ComponentModel.BackgroundWorker();
            this.TimeToCheck_label = new System.Windows.Forms.Label();
            this.TimeStatusLabel = new System.Windows.Forms.Label();
            this.TimeToCheck_panel = new System.Windows.Forms.Panel();
            this.CheckInProgress_panel = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.CheckInProgress_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.IntervalTimePick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.TimeToCheck_panel.SuspendLayout();
            this.CheckInProgress_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StopAndExitButton
            // 
            this.StopAndExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.StopAndExitButton, "StopAndExitButton");
            this.StopAndExitButton.Name = "StopAndExitButton";
            this.StopAndExitButton.UseVisualStyleBackColor = true;
            this.StopAndExitButton.Click += new System.EventHandler(this.StopAndExitButton_Click);
            // 
            // UpdateSettingsButton
            // 
            resources.ApplyResources(this.UpdateSettingsButton, "UpdateSettingsButton");
            this.UpdateSettingsButton.Name = "UpdateSettingsButton";
            this.UpdateSettingsButton.UseVisualStyleBackColor = true;
            this.UpdateSettingsButton.Click += new System.EventHandler(this.UpdateSettingsButton_Click);
            // 
            // IntervalTimePick
            // 
            resources.ApplyResources(this.IntervalTimePick, "IntervalTimePick");
            this.IntervalTimePick.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.IntervalTimePick.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.IntervalTimePick.Name = "IntervalTimePick";
            this.IntervalTimePick.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // SleepTimeCheckBox
            // 
            resources.ApplyResources(this.SleepTimeCheckBox, "SleepTimeCheckBox");
            this.SleepTimeCheckBox.Checked = true;
            this.SleepTimeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SleepTimeCheckBox.Name = "SleepTimeCheckBox";
            this.SleepTimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // CheckInterval_label1
            // 
            resources.ApplyResources(this.CheckInterval_label1, "CheckInterval_label1");
            this.CheckInterval_label1.Name = "CheckInterval_label1";
            // 
            // CheckInterval_label2
            // 
            resources.ApplyResources(this.CheckInterval_label2, "CheckInterval_label2");
            this.CheckInterval_label2.Name = "CheckInterval_label2";
            // 
            // PictureBox
            // 
            this.PictureBox.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.PictureBox, "PictureBox");
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.TabStop = false;
            // 
            // Logo1
            // 
            resources.ApplyResources(this.Logo1, "Logo1");
            this.Logo1.Name = "Logo1";
            // 
            // Logo2
            // 
            resources.ApplyResources(this.Logo2, "Logo2");
            this.Logo2.Name = "Logo2";
            // 
            // TrayIcon
            // 
            resources.ApplyResources(this.TrayIcon, "TrayIcon");
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // CheckTimer
            // 
            this.CheckTimer.Interval = 1000;
            this.CheckTimer.Tick += new System.EventHandler(this.CheckTimer_Tick);
            // 
            // CheckGradesBW
            // 
            this.CheckGradesBW.WorkerReportsProgress = true;
            this.CheckGradesBW.WorkerSupportsCancellation = true;
            this.CheckGradesBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckGradesBW_DoWork);
            this.CheckGradesBW.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.CheckGradesBW_ProgressChanged);
            this.CheckGradesBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CheckGradesBW_RunWorkerCompleted);
            // 
            // TimeToCheck_label
            // 
            resources.ApplyResources(this.TimeToCheck_label, "TimeToCheck_label");
            this.TimeToCheck_label.Name = "TimeToCheck_label";
            // 
            // TimeStatusLabel
            // 
            resources.ApplyResources(this.TimeStatusLabel, "TimeStatusLabel");
            this.TimeStatusLabel.Name = "TimeStatusLabel";
            // 
            // TimeToCheck_panel
            // 
            this.TimeToCheck_panel.Controls.Add(this.TimeStatusLabel);
            this.TimeToCheck_panel.Controls.Add(this.TimeToCheck_label);
            resources.ApplyResources(this.TimeToCheck_panel, "TimeToCheck_panel");
            this.TimeToCheck_panel.Name = "TimeToCheck_panel";
            // 
            // CheckInProgress_panel
            // 
            this.CheckInProgress_panel.Controls.Add(this.progressBar);
            this.CheckInProgress_panel.Controls.Add(this.CheckInProgress_Label);
            resources.ApplyResources(this.CheckInProgress_panel, "CheckInProgress_panel");
            this.CheckInProgress_panel.Name = "CheckInProgress_panel";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // CheckInProgress_Label
            // 
            resources.ApplyResources(this.CheckInProgress_Label, "CheckInProgress_Label");
            this.CheckInProgress_Label.Name = "CheckInProgress_Label";
            // 
            // MainWindow
            // 
            this.AcceptButton = this.UpdateSettingsButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.StopAndExitButton;
            this.Controls.Add(this.UpdateSettingsButton);
            this.Controls.Add(this.StopAndExitButton);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.Logo1);
            this.Controls.Add(this.Logo2);
            this.Controls.Add(this.CheckInterval_label1);
            this.Controls.Add(this.IntervalTimePick);
            this.Controls.Add(this.CheckInterval_label2);
            this.Controls.Add(this.SleepTimeCheckBox);
            this.Controls.Add(this.TimeToCheck_panel);
            this.Controls.Add(this.CheckInProgress_panel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.IntervalTimePick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.TimeToCheck_panel.ResumeLayout(false);
            this.TimeToCheck_panel.PerformLayout();
            this.CheckInProgress_panel.ResumeLayout(false);
            this.CheckInProgress_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StopAndExitButton;
        private System.Windows.Forms.Button UpdateSettingsButton;
        private System.Windows.Forms.NumericUpDown IntervalTimePick;
        private System.Windows.Forms.CheckBox SleepTimeCheckBox;
        private System.Windows.Forms.Label CheckInterval_label1;
        private System.Windows.Forms.Label CheckInterval_label2;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Label Logo1;
        private System.Windows.Forms.Label Logo2;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.Timer CheckTimer;
        private System.ComponentModel.BackgroundWorker CheckGradesBW;
        private System.Windows.Forms.Label TimeToCheck_label;
        private System.Windows.Forms.Label TimeStatusLabel;
        private System.Windows.Forms.Panel TimeToCheck_panel;
        private System.Windows.Forms.Panel CheckInProgress_panel;
        private System.Windows.Forms.Label CheckInProgress_Label;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}