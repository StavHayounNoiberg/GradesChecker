using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace GradesChecker
{
    public static class Functions
    {
        const string data_file = "data.dat";
        const string credentials_file = "creds.dat";
        const string settings_file = "settings.dat";

        public static bool GetCredentials(ref UserData data)
        {
            if (File.Exists(credentials_file) == false)
                return false;

            FileStream stream = new FileStream(credentials_file, FileMode.Open);
            BinaryReader bRead = new BinaryReader(stream);

            data.Username = bRead.ReadString();
            data.Password = bRead.ReadString();
            data.Year = bRead.ReadString();
            data.Semester = bRead.ReadString();
            data.Email = bRead.ReadString();

            bRead.Close();
            stream.Close();
            return true;
        }

        public static bool CheckCredentials(UserData data)
        {
            if (data.Username != null && data.Password != null && data.Semester != null && data.Email != null)
                return true;
            return false;
        }

        public static bool GetFormsData(ref UserData data, string username, string password, decimal year, string semester)
        {
            if (username.Length > 0 && password.Length > 0 && semester.Length > 0)
            {
                data.Username = username;
                data.Password = password;
                data.Year = year.ToString();
                data.Semester = semester;
                data.Email = string.Join("", data.Username, "@s.afeka.ac.il");
                ExportCredentials(data);
                return true;
            }
            return false;
        }

        private static void ExportCredentials(UserData data)
        {
            FileStream stream = new FileStream(credentials_file, FileMode.Create);
            BinaryWriter bWrite = new BinaryWriter(stream);

            bWrite.Write(data.Username);
            bWrite.Write(data.Password);
            bWrite.Write(data.Year);
            bWrite.Write(data.Semester);
            bWrite.Write(data.Email);

            bWrite.Close();
            stream.Close();
        }

        public static bool LoadSettings(out decimal interval, out bool sleeptime)
        {
            interval = 20;
            sleeptime = true;
            if (File.Exists(settings_file))
            {
                FileStream stream = new FileStream(settings_file, FileMode.Open);
                BinaryReader bRead = new BinaryReader(stream);

                interval = bRead.ReadDecimal();
                sleeptime = bRead.ReadBoolean();

                bRead.Close();
                stream.Close();

                return true;
            }
            return false;
        }

        internal static void ExportSettings(decimal timeInterval, bool sleepTime)
        {

            FileStream stream = new FileStream(settings_file, FileMode.Create);
            BinaryWriter bWrite = new BinaryWriter(stream);

            bWrite.Write(timeInterval);
            bWrite.Write(sleepTime);

            bWrite.Close();
            stream.Close();
        }

        public static bool CheckInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "8.8.8.8"; // google.com
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status != IPStatus.Success)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<Grade> GetOldGrades()
        {
            if (File.Exists(data_file) == false)
                return null;

            FileStream stream = new FileStream(data_file, FileMode.Open);
            BinaryReader bRead = new BinaryReader(stream);

            int count = bRead.ReadInt32();
            List<Grade> old_grades = new List<Grade>();
            for (int i = 0; i < count; i++)
            {
                Grade g = new Grade
                {
                    courseID = bRead.ReadInt32(),
                    title = bRead.ReadString(),
                    subID = bRead.ReadString(),
                    grade = bRead.ReadInt32()
                };
                old_grades.Add(g);
            }

            bRead.Close();
            stream.Close();
            return old_grades;
        }

        public static string CreateMessage(List<Grade> difference)
        {
            string message = "";

            if (difference.Count == 1)
                message = "New grade found on AfekaNet!\n\nCourse: " + difference[0].title + " " + difference[0].subID + "\nGrade: " + Convert.ToString(difference[0].grade);

            else if (difference.Count > 1)
            {
                string courses = "";
                foreach (var g in difference)
                    courses = string.Join("\n\n", courses, "Course: " + g.title + " " + g.subID + "\nGrade: " + Convert.ToString(g.grade));
                message = "New grades found on AfekaNet!" + courses;
            }
            return message;
        }

        public static void SendMessage(string message, UserData data)
        {
            SmtpClient smptClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(data.Email, data.Password),
                EnableSsl = true,
            };

            smptClient.Send(data.Email, data.Email, "New Grade Updated", message);
        }

        public static void ExportData(List<Grade> new_grades)
        {
            FileStream stream = new FileStream(data_file, FileMode.Create);
            BinaryWriter bWrite = new BinaryWriter(stream);

            bWrite.Write(new_grades.Count);
            foreach (var g in new_grades)
            {
                bWrite.Write(g.courseID);
                bWrite.Write(g.title);
                bWrite.Write(g.subID);
                bWrite.Write(g.grade);
            }

            bWrite.Close();
            stream.Close();
        }

        public static void DeleteAll()
        {
            if (File.Exists(credentials_file))
                File.Delete(credentials_file);
            if (File.Exists(data_file))
                File.Delete(data_file);
            if (File.Exists(settings_file))
                File.Delete(settings_file);
            var VBS_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\Startup\GradesChecker.vbs");
            if (File.Exists(VBS_file))
                File.Delete(VBS_file);
        }
    }
}
