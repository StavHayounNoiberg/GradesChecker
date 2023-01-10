using System;
using System.IO;
using System.Windows.Forms;

namespace GradesChecker
{
    static class GUI
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AutoRunApp();
            var UserData = new UserData();
            Functions.GetCredentials(ref UserData);
            Application.Run(new MainWindow(UserData));
        }

        static void AutoRunApp()
        {
            try
            {
                var Direct = Directory.GetCurrentDirectory();
                var VBS_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\Startup\GradesChecker.vbs");
                if (File.Exists(VBS_file) == false)
                {
                    var VBS_content = "set shell = CreateObject(\"WScript.Shell\")\n" + "shell.CurrentDirectory = \"" + Direct.ToString() + "\"\nshell.Run \"GradesChecker.exe\", 0, True";
                    File.WriteAllText(VBS_file, VBS_content);
                }
            }
            catch
            {
                MessageBox.Show("Cannot create autorun on startup! Restart and try again");
                System.Environment.Exit(1);
            }
        }
    }
}