using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmCompanyData : Form
    {
        public frmCompanyData()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Addd Functionality to Close this form
            this.Hide();
        }

        private void frmCompanyData_Load(object sender, EventArgs e)
        {
        }
                
    }
}
