using BIMStore.BLL;
using BIMStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        loginDAL dal = new loginDAL();
        public static string loggedIn;
        bool donotshowfrm = false;

        private void pboxClose_Click(object sender, EventArgs e)
        {
            //Code to close this form
            this.Close();
            Environment.Exit(0);
        }

        private void Calculate(int i)
        {
            double pow = Math.Pow(i, i);
        }

        public void DoWork(IProgress<int> progress)
        {
            // This method is executed in the context of
            // another thread (different than the main UI thread),
            // so use only thread-safe code
            for (int j = 0; j < 10000; j++)
            {
                Calculate(j);

                // Use progress to notify UI thread that progress has
                // changed
                if (progress != null)
                    progress.Report((j + 1) * 100 / 10000);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;

            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                progressBar1.Value = v;
            });

            // Run operation in another thread
            await Task.Run(() => DoWork(progress));

            tbl_users l = new tbl_users();
            l.username = txtUsername.Text.Trim();
            l.password = txtPassword.Text.Trim();
            //l.user_type = cmbUserType.Text.Trim();

            //Checking the login credentials
            tbl_users usr = dal.loginCheck(l);
            if (usr != null)
            {
                l.user_type = usr.user_type;

                //Login Successfull
                loggedIn = l.username;
                //Need to open Respective Forms based on User Type
                switch (l.user_type)
                {
                    case "Admin":
                        {
                            //Display Admin Dashboard
                            frmAdminDashboard admin = new frmAdminDashboard();
                            admin.Show();
                            this.Hide();
                        }
                        break;

                    case "User":
                        {
                            //Display User Dashboard
                            frmUserDashboard user = new frmUserDashboard();
                            user.Show();
                            this.Hide();
                        }
                        break;

                    default:
                        {
                            //Display an error message
                            MessageBox.Show("Invalid User Type.");
                        }
                        break;
                }
            }
            else
            {
                //login Failed
                MessageBox.Show("Login Failed. Try Again");
            }

        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            bool user_available = dal.loginCheckFirst();
            if ( user_available == false)
            {
                frmUsers user = new frmUsers();
                user.Show();
                user.FirstLogin();
                donotshowfrm = true;
            }
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            if (donotshowfrm)
            {
                this.Hide();
            }
        }
    }
}
