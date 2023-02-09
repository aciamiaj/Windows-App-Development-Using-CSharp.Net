using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PatientMastarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Patient_Mastar_form pmf = new Patient_Mastar_form();
            pmf.MdiParent = this;
            pmf.Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.P)
            {
                patientMastarToolStripMenuItem.PerformClick();
            }
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search sf = new Search();
            sf.MdiParent = this;
            sf.Show();
        }
    }
}
