using System;
using System.Windows.Forms;

namespace GradesChecker
{
    public partial class DataInputGUI : Form
    {
        private UserData UserData;
        public DataInputGUI(ref UserData userData)
        {
            InitializeComponent();
            yearBox.Value = DateTime.Now.Year;
            Semester_ComboBox.SelectedIndex = 0;
            this.UserData = userData;
        }

        private void Submit_button_Click(object sender, EventArgs e)
        {
            if (Username_TextBox.Text.Length == 0 || Password_TextBox.Text.Length == 0 || Semester_ComboBox.SelectedItem.ToString().Length == 0)
            {
                this.DialogResult = DialogResult.Retry;
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Hide();
            if (Functions.GetFormsData(ref UserData, Username_TextBox.Text, Password_TextBox.Text, yearBox.Value, Semester_ComboBox.SelectedItem.ToString()) == false)
                this.DialogResult = DialogResult.Retry;
            return;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
