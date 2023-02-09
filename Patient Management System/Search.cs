using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS_Project
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }
        static string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\LMS.accdb";

        static OleDbConnection conn = new OleDbConnection(connStr);
        static OleDbCommand cmd = new OleDbCommand("", conn);
        private void Search_Load(object sender, EventArgs e)
        {
            loadPatientData("SELECT * From patient_mastar");
            loadDoctors();
        }
        private void loadPatientData(string query)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            patientGridView.DataSource = ds.Tables[0];
        }

        private void IdTextbox_TextChanged(object sender, EventArgs e)
        {
            if (idTextbox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("LAB_ID = '{0}'", idTextbox.Text);
            }
        }

        private void DateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (dateTextBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("FILE_DATE LIKE '{0}%' OR FILE_DATE LIKE '% {0}%'", dateTextBox.Text);
            }
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nameTextBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("PATIENT_NAME LIKE '{0}%' OR PATIENT_NAME LIKE '% {0}%'", nameTextBox.Text);
            }
        }

        private void AgeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ageTextBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("AGE = '{0}'", ageTextBox.Text);
            }
        }

        private void GenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (genderComboBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%'", genderComboBox.Text);
            }
        }
        private void RefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (refComboBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%'", refComboBox.Text);
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            string query = "";
            if (idTextbox.Text != "")
            {
                query += string.Format("(LAB_ID = '{0}')", idTextbox.Text);
                if (dateTextBox.Text != "")
                    query += string.Format("AND (FILE_DATE LIKE '{0}%' OR FILE_DATE LIKE '% {0}%')", dateTextBox.Text);
                if (nameTextBox.Text != "")
                    query += string.Format("AND (PATIENT_NAME LIKE '{0}%' OR PATIENT_NAME LIKE '% {0}%')", nameTextBox.Text);
                if (ageTextBox.Text != "")
                    query += string.Format("AND (AGE = '{0}%')", ageTextBox.Text);
                if (genderComboBox.Text != "")
                    query += string.Format("AND (GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%')", genderComboBox.Text);
                if (refComboBox.Text != "")
                    query += string.Format("AND (REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }
            else if (dateTextBox.Text != "")
            {
                query += string.Format("(FILE_DATE LIKE '{0}%' OR FILE_DATE LIKE '% {0}%')", dateTextBox.Text);
                if (nameTextBox.Text != "")
                    query += string.Format("AND (PATIENT_NAME LIKE '{0}%' OR PATIENT_NAME LIKE '% {0}%')", nameTextBox.Text);
                if (ageTextBox.Text != "")
                    query += string.Format("AND (AGE = '{0}%')", ageTextBox.Text);
                if (genderComboBox.Text != "")
                    query += string.Format("AND (GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%')", genderComboBox.Text);
                if (refComboBox.Text != "")
                    query += string.Format("AND (REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }
            else if (nameTextBox.Text != "")
            {
                query += string.Format("(PATIENT_NAME LIKE '{0}%' OR PATIENT_NAME LIKE '% {0}%')", nameTextBox.Text);
                if (ageTextBox.Text != "")
                    query += string.Format("AND (AGE = '{0}%')", ageTextBox.Text);
                if (genderComboBox.Text != "")
                    query += string.Format("AND (GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%')", genderComboBox.Text);
                if (refComboBox.Text != "")
                    query += string.Format("AND (REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }
            else if (ageTextBox.Text != "")
            {
                query += string.Format("(AGE = '{0}%')", ageTextBox.Text);
                if (genderComboBox.Text != "")
                    query += string.Format("AND (GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%')", genderComboBox.Text);
                if (refComboBox.Text != "")
                    query += string.Format("AND (REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }
            else if (genderComboBox.Text != "")
            {
                query += string.Format("(GENDER LIKE '{0}%' OR GENDER LIKE '% {0}%')", genderComboBox.Text);
                if (refComboBox.Text != "")
                    query += string.Format("AND (REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }
            else if (refComboBox.Text != "")
            {
                query += string.Format("(REF_BY LIKE '{0}%' OR REF_BY LIKE '% {0}%')", refComboBox.Text);
            }

            (patientGridView.DataSource as DataTable).DefaultView.RowFilter = query;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            idTextbox.Clear();
            nameTextBox.Clear();
            dateTextBox.Clear();
            ageTextBox.Clear();
            genderComboBox.Text = "";
            loadPatientData("SELECT * From patient_mastar");
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

        private void GoButton_Click(object sender, EventArgs e)
        {
            if (minAgeTextBox.Text != "" && maxAgeTextBox.Text != "")
            {
                (patientGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("AGE >= '{0}' AND AGE <= '{1}'", minAgeTextBox.Text, maxAgeTextBox.Text);
            }
        }
    }
}
