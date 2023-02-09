using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace LMS_Project
{
    public partial class Patient_Mastar_form : Form
    {
        public Patient_Mastar_form()
        {
            InitializeComponent();
        }
        static string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\LMS.accdb";

        static OleDbConnection conn = new OleDbConnection(connStr);
        static OleDbCommand cmd = new OleDbCommand("", conn);

        private void Patient_Mastar_form_Load(object sender, EventArgs e)
        {
            dateTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            todayButton.PerformClick();
            loadTests();
            loadDoctors();
            idTextbox.Text = generateId();
            InsertMode();
            idTextbox.Enabled = false;
            dateTextBox.Enabled = false;
            nameTextBox.Focus();
        }
        private void loadTests()
        {
            testComboBox.Items.Clear();
            try
            {
                cmd.CommandText = "SELECT test_group FROM test_group";
                conn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                // Call Read before accessing data.
                while (dr.Read())
                {
                    testComboBox.Items.Add(dr[0]);
                }
                // Call Close when done reading.
                dr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        private void loadDoctors()
        {
            refComboBox.Items.Clear();
            try
            {
                cmd.CommandText = "SELECT DR_NAME FROM doctors";
                conn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                // Call Read before accessing data.
                while (dr.Read())
                {
                    refComboBox.Items.Add(dr[0]);
                }
                // Call Close when done reading.
                dr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        private void loadPatientData(string query)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            patientGridView.DataSource = ds.Tables[0];
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            loadPatientData("SELECT * From patient_mastar");
        }

        private void ByDateButton_Click(object sender, EventArgs e)
        {
            string theDate = dateTimePicker1.Value.ToString("dd-MM-yyyy");
            loadPatientData("SELECT * From patient_mastar WHERE file_date = '" + theDate + "'");
        }

        private void TodayButton_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            string theDate = today.ToString("dd-MM-yyyy");
            loadPatientData("SELECT * From patient_mastar WHERE file_date = '" + theDate + "'");
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            bool testExist = false;
            for (int i = 0; i < testsListBox.Items.Count; i++)
            {
                if (testsListBox.Items[i].ToString().Equals(testComboBox.Text))
                {
                    testExist = true;
                }
            }
            if (!testExist)
            {
                testsListBox.Items.Add(testComboBox.Text);
            }
            testComboBox.Focus();
            testComboBox.Text = "";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string tests = "";
            if (testsListBox.Items.Count > 0)
            {
                for (int i = 0; i < testsListBox.Items.Count; i++)
                {
                    if (i == 0)
                        tests = testsListBox.Items[0].ToString();
                    else
                        tests = tests + ", " + testsListBox.Items[i].ToString();
                }
            }
            try
            {
                conn.Open();
                cmd.CommandText = "insert into patient_mastar (LAB_ID, FILE_DATE, PATIENT_NAME, TEST_GROUP, AGE, GENDER, ADDRESS, CONTACT_NO, EMAIL, SAMPLE_RECEIVE_TIME, STATUS, REF_BY) values('" + idTextbox.Text + "', '" + dateTextBox.Text + "', '" + nameTextBox.Text + "', '" + tests + "', '" + ageTextBox.Text + "', '" + genderComboBox.Text + "','" + addressTextBox.Text + "', '" + contactTextBox.Text + "', '" + emailTextBox.Text + "', '" + dateTimePicker1.Text + "', 'Pending', '" + refComboBox.Text + "') ";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Inserted");
            }
            catch (Exception exp)
            { MessageBox.Show(exp.ToString()); }
            finally
            {
                conn.Close();
            }
            clearButton.PerformClick();
            loadPatientData("SELECT * From patient_mastar WHERE file_date = '" + dateTextBox.Text + "'");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            dateTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            idTextbox.Text = generateId();
            nameTextBox.Clear();
            ageTextBox.Clear();
            genderComboBox.Text = "";
            addressTextBox.Clear();
            contactTextBox.Clear();
            emailTextBox.Clear();
            refComboBox.Text = "";
            testComboBox.Text = "";
            testsListBox.Items.Clear();
            InsertMode();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            string tests = "";
            if (testsListBox.Items.Count > 0)
            {
                for (int i = 0; i < testsListBox.Items.Count; i++)
                {
                    if (i == 0)
                        tests = testsListBox.Items[0].ToString();
                    else
                        tests = tests + ", " + testsListBox.Items[i].ToString();
                }
            }
            try
            {
                conn.Open();
                cmd.CommandText = "UPDATE patient_mastar SET PATIENT_NAME = '" + nameTextBox.Text + "', TEST_GROUP = '" + tests + "', AGE = '" + ageTextBox.Text + "', GENDER = '" + genderComboBox.Text + "', ADDRESS = '" + addressTextBox.Text + "', CONTACT_NO = '" + contactTextBox.Text + "', EMAIL = '" + emailTextBox.Text + "', REF_BY = '" + refComboBox.Text + "' WHERE LAB_ID = '" + idTextbox.Text + "' AND FILE_DATE = '" + dateTextBox.Text+"'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated");
            }
            catch (Exception exp)
            { MessageBox.Show(exp.ToString()); }
            finally
            {
                conn.Close();
            }
            InsertMode();
            clearButton.PerformClick();
            loadPatientData("SELECT * From patient_mastar WHERE file_date = '" + dateTextBox.Text + "'");
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM patient_mastar WHERE LAB_ID = '" + idTextbox.Text + "' AND FILE_DATE = '" + dateTextBox.Text+"'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Deleted");
            }
            catch (Exception exp)
            { MessageBox.Show(exp.ToString()); }
            finally
            {
                conn.Close();
            }
            InsertMode();
            clearButton.PerformClick();
            loadPatientData("SELECT * From patient_mastar WHERE file_date = '" + dateTextBox.Text + "'");
        }

        private void PatientGridView_DoubleClick(object sender, EventArgs e)
        {
            int selectedrowindex = patientGridView.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = patientGridView.Rows[selectedrowindex];
            idTextbox.Text = Convert.ToString(selectedRow.Cells["LAB_ID"].Value);
            dateTextBox.Text = Convert.ToString(selectedRow.Cells["FILE_DATE"].Value);
            nameTextBox.Text = Convert.ToString(selectedRow.Cells["PATIENT_NAME"].Value); ;
            string[] tests = Convert.ToString(selectedRow.Cells["TEST_GROUP"].Value).Split(',');
            testsListBox.Items.Clear();
            testsListBox.Items.Add(tests[0]);
            for (int i = 1; i < tests.Length; i++)
            {
                testsListBox.Items.Add(tests[i].Remove(0, 1));
            }
            ageTextBox.Text = Convert.ToString(selectedRow.Cells["AGE"].Value); ;
            genderComboBox.Text = Convert.ToString(selectedRow.Cells["GENDER"].Value); ;
            addressTextBox.Text = Convert.ToString(selectedRow.Cells["ADDRESS"].Value); ;
            contactTextBox.Text = Convert.ToString(selectedRow.Cells["CONTACT_NO"].Value); ;
            emailTextBox.Text = Convert.ToString(selectedRow.Cells["EMAIL"].Value);
            refComboBox.Text = Convert.ToString(selectedRow.Cells["REF_BY"].Value);

            UpdateMode();
        }
        private string generateId()
        {
            int max = 0;
            try
            {
                cmd.CommandText = "SELECT LAB_ID FROM patient_mastar WHERE FILE_DATE = '"+dateTextBox.Text+"'";
                conn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                // Call Read before accessing data.
                while (dr.Read())
                {
                    if(max < Convert.ToInt32(dr[0]))
                    {
                        max = Convert.ToInt32(dr[0]);
                    }
                }
                // Call Close when done reading.
                dr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return (max + 1).ToString();
        }
        private void InsertMode()
        {
            saveButton.Enabled = true;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
        }
        private void UpdateMode()
        {
            saveButton.Enabled = false;
            updateButton.Enabled = true;
            deleteButton.Enabled = true;
        }
        private void DateTextBox_TextChanged(object sender, EventArgs e)
        {
            //DateTime Test;
            //if (DateTime.TryParseExact(dateTextBox.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out Test))
            //{
                
            //}
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ageTextBox.Focus();
        }

        private void AgeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                genderComboBox.Focus();
        }

        private void GenderComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addressTextBox.Focus();
        }

        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                contactTextBox.Focus();
        }

        private void ContactTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                emailTextBox.Focus();
        }

        private void EmailTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                refComboBox.Focus();
        }

        private void RefComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                testComboBox.Focus();
        }

        private void TestComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                addButton.Focus();
        }

        private void Patient_Mastar_form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S && saveButton.Enabled == true)
            {
                saveButton.PerformClick();
            }
        }

        private void TestsListBox_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure!! Want to delete", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                testsListBox.Items.Remove(testsListBox.SelectedItem);
            }
        }
    }
}
