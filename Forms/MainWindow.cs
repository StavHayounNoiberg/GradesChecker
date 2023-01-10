using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace GradesChecker
{
    public partial class MainWindow : Form
    {
        private UserData UserData;
        private DateTime Start;
        private TimeSpan Interval;
        private TimeSpan SettingsInterval;
        private WebControls web;
        private bool isConnected = true;

        public MainWindow(UserData data)
        {
            InitializeComponent();
            web = new WebControls();
            this.UserData = data;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (Functions.CheckCredentials(UserData) == false)
            {
            Start:
                Form DataGUI = new DataInputGUI(ref UserData);
                DialogResult result = DataGUI.ShowDialog();
                if (result == DialogResult.Retry)
                {
                    MessageBox.Show("Please fill all forms", "Fill Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    goto Start;
                }
                else if (result == DialogResult.Cancel)
                {
                    StopAndExitButton_Click(null, null);
                    return;
                }
            }

            var toMinimize = Functions.LoadSettings(out decimal d, out bool b);
            IntervalTimePick.Value = d;
            SettingsInterval = TimeSpan.FromMinutes((double)IntervalTimePick.Value);
            Interval = TimeSpan.FromSeconds(1);
            SleepTimeCheckBox.Checked = b;
            Start = DateTime.Now;
            CheckTimer.Start();
            if (toMinimize)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                TrayIcon.Visible = true;
            }
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            TrayIcon.Visible = false;
        }

        private void UpdateSettingsButton_Click(object sender, EventArgs e)
        {
            CheckTimer.Stop();
            Start = DateTime.Now;
            Functions.ExportSettings(IntervalTimePick.Value, SleepTimeCheckBox.Checked);
            SettingsInterval = TimeSpan.FromMinutes((double)IntervalTimePick.Value);
            if (isConnected)
                Interval = SettingsInterval;
            CheckTimer.Start();
            this.WindowState = FormWindowState.Minimized;
        }

        private void StopAndExitButton_Click(object sender, EventArgs e)
        {
            if (CheckGradesBW.IsBusy)
                CheckGradesBW.CancelAsync();

            Functions.DeleteAll();
            this.DialogResult = DialogResult.Abort;
            MessageBox.Show("Will not auto check anymore. Run GradesChecker again to enter your credentials and start checking.", "GradesChecker Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.ExitThread();
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan sleep_start = new TimeSpan(22, 0, 0);  // 10 PM
            TimeSpan sleep_end = new TimeSpan(8, 0, 0);     // 8 AM
            TimeSpan sleep_now = DateTime.Now.TimeOfDay;

            if (SleepTimeCheckBox.Checked)
            {
                if ((sleep_now > sleep_start) || (sleep_now < sleep_end))
                    return;
            }

            double remainingSeconds = Interval.TotalSeconds - (DateTime.Now - Start).TotalSeconds;
            TimeStatusLabel.Text = TimeSpan.FromSeconds(remainingSeconds).ToString(@"hh\:mm\:ss");
            if (remainingSeconds <= 0)
            {
                StartCheck();
            }
        }

        private void StartCheck()
        {
            TimeToCheck_panel.Hide();
            CheckInProgress_panel.Show();
            CheckInProgress_panel.BringToFront();
            CheckTimer.Stop();
            progressBar.Value = 0;
            CheckGradesBW.RunWorkerAsync();
        }

        private void ChangeToState(bool state)
        {
            if (state == false)  // false indicates no internet connection
            {
                TimeToCheck_label.Text = "No network. Retry:";
                Interval = TimeSpan.FromSeconds(10);
            }
            else
            {
                TimeToCheck_label.Text = "Time to next check:";
                Interval = SettingsInterval;
            }
        }

        private void CheckGradesBW_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Check Internet Connection
            isConnected = Functions.CheckInternetConnection();
            ChangeToState(isConnected);
            if (isConnected == false)
                return;
            #endregion
            #region Get Old Grades

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            List<Grade> old_grades = Functions.GetOldGrades();
            if (old_grades == null)
                old_grades = new List<Grade>();

            CheckGradesBW.ReportProgress(10);

            #endregion
            #region Open Chrome

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            IWebDriver driver;
            try
            {
                driver = web.CreateWebDriver();
            }
            catch
            {
                e.Result = new Tuple<string, bool>("Cannot open Chrome. Make sure you installed Chrome browser version 108.0.5359.125 or above.\nIf you got this message after your computer went to sleep, make sure you allow network connectivity during standby mode in computer settings.", false);
                return;
            }

            CheckGradesBW.ReportProgress(25);

            #endregion
            #region Login

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                driver.Quit();
                return;
            }

            try
            {
                driver.Navigate().GoToUrl("https://sso.afeka.ac.il/my.policy");
                web.SignInAndGetPage(ref driver, UserData);
            }
            catch
            {
                e.Result = new Tuple<string, bool>("Cannot sign in to AfekaNet. Auto Checking will stop. Make sure your credentials are correct and try again.", true);
                driver.Quit();
                return;
            }

            CheckGradesBW.ReportProgress(40);

            #endregion
            #region Go To Grades Page

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                driver.Quit();
                return;
            }

            try
            {
                web.GoToGradesPage(ref driver, UserData);
            }
            catch
            {
                e.Result = new Tuple<string, bool>("Cannot find page. Are you sure the year and semester you chose are correct?\nMake sure your internet connection is stable and credentials are correct.\nIt is also possible that AfekaNet pages have changed.", false);
                driver.Quit();
                return;
            }

            CheckGradesBW.ReportProgress(60);

            #endregion
            #region Find All Grades

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                driver.Quit();
                return;
            }

            List<Grade> new_grades = new List<Grade>();
            try
            {
                new_grades = web.FindAllGrades(ref driver);
            }
            catch
            {
                e.Result = new Tuple<string, bool>("Cannot find page. Are you sure the year and semester you chose are correct?\nMake sure your internet connection is stable. It is possible that AfekaNet pages have changed.", false);
                driver.Quit();
                return;
            }
            driver.Quit();

            CheckGradesBW.ReportProgress(80);

            #endregion
            #region Compare Grades

            if (CheckGradesBW.CancellationPending)
            {
                e.Cancel = true;
                driver.Quit();
                return;
            }

            try
            {
                List<Grade> difference = new_grades.Except(old_grades).ToList();

                if (difference.Count > 0)
                {
                    string messaage = Functions.CreateMessage(difference);
                    Functions.SendMessage(messaage, UserData);
                }
                Functions.ExportData(new_grades);
            }
            catch
            {
                e.Result = new Tuple<string, bool>("Cannot send message. Make sure your internet connection is stable and port 587 is accesible.", false);
                return;
            }

            CheckGradesBW.ReportProgress(100);

            #endregion    
        }

        private void CheckGradesBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void CheckGradesBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Unexpected error, restart GradesChecker and try again.");
                Functions.DeleteAll();
                Application.Exit();
            }
            else if (e.Cancelled) { }
            else if (e.Result != null)
            {
                Tuple<string, bool> result = (Tuple<string, bool>)e.Result;
                MessageBox.Show(result.Item1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result.Item2)
                {
                    Functions.DeleteAll();
                    Application.Exit();
                }
            }
            Start = DateTime.Now;
            CheckInProgress_panel.Hide();
            TimeToCheck_panel.Show();
            TimeToCheck_panel.BringToFront();
            CheckTimer.Start();
        }
    }
}
