using BIMStore.BLL;
using BIMStore.DAL;
using BIMStore.Properties;
using BIMStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BIMStore
{
    public partial class frmAdminDashboard : Form
    {
        public frmAdminDashboard()
        {
            InitializeComponent();
        }

        transactionDAL tDAL = new transactionDAL();
        DeaCustDAL dcDAL = new DeaCustDAL();
        System.Timers.Timer aTimer = new System.Timers.Timer();

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            //Adding test form for debug mode only
#if (DEBUG)
            ExtendmenuStripItems();
#endif
            lblLoggedInUser.Text = frmLogin.loggedIn;
            LoadChart();
            LoadInventoryWarning();

            setupTabControlLeaveEnter();

            aTimer.SynchronizingObject = this;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            tabControl.SelectedIndex = (tabControl.SelectedIndex + 1 < tabControl.TabCount) ?
                 tabControl.SelectedIndex + 1 : tabControl.SelectedIndex = 0;
        }
        //bool change changethis = !changethis;

        //Set a public static method to specify whether the form is purchase or sale
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmUsers user = new frmUsers();
            user.Show();
        }

        private void frmAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            aTimer.Enabled = false;
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmCategories category = new frmCategories();
            category.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmProducts product = new frmProducts();
            product.Show();
        }

        private void deealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmTransactions transaction = new frmTransactions();
            transaction.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void companydataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }

        private void purchaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            aTimer.Enabled = false;
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.transactionType = "Purchase";
            purchase.Show();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sale
            aTimer.Enabled = false;
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Sale";
            sale.Show();
        }

        private void bookingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to 
            aTimer.Enabled = false;
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Booking";
            sale.Show();
            sale.LoadBooking();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmSales sales = new frmSales();
            sales.Show();
        }

        public class asdf
        {
            public Icon newicon { get; set; }
            public tbl_products prods { get; set; }
        }

        private void LoadInventoryWarning()
        {
            pbwarning.Image = Resources.warning.ToBitmap();

            productsDAL pdal = new productsDAL();
            List<tbl_products> warningProducts = pdal.GetProductsByWarning();
            dgvInventoryWarning.DataSource = warningProducts;
        }

        private void LoadChart()
        {
            Chart.Series.Clear();
            Chart.ChartAreas.Clear();
            Chart.Titles.Clear();
            Chart.ChartAreas.Add("new");
            Chart.ChartAreas[0].AxisX.Interval = 1;
            //Chart.Size = new Size(1000, 500);
            Chart.Titles.Add("Total Income");

            Series seriesTI = Chart.Series.Add("Total Income");
            seriesTI.ChartType = SeriesChartType.Spline;
            seriesTI.BorderWidth = 2;
            seriesTI.MarkerStyle = MarkerStyle.Circle;
            seriesTI.MarkerSize = 8;

            List<tbl_dea_cust> custs = dcDAL.SearchAllDeaOrCust("Customer");
            Dictionary<int, Series> custsprofit = new Dictionary<int, Series>();
            foreach (var cust in custs)
            {
                Series seriesXY = Chart.Series.Add(cust.last_name.ToString());
                seriesXY.ChartType = SeriesChartType.Spline;
                seriesXY.MarkerStyle = MarkerStyle.Circle;
                seriesXY.MarkerSize = 8;
                custsprofit.Add(cust.Id, seriesXY);
            }

            int Year = DateTime.Now.Year;
            DateTime target = new DateTime(Year, 12, 31);
            DateTime now = new DateTime(Year, 01, 01);

            while (now < target)
            {
                Dictionary<int, decimal> aa = new Dictionary<int, decimal>();
                DateTime DateFrom = new DateTime(now.Year, now.Month, 01, 0, 0, 0);
                DateTime DateTo = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year,now.Month), 23, 59, 59);
                String Monthname = DateFrom.ToString("MMM", CultureInfo.InvariantCulture);

                List<tbl_transactions> trans = tDAL.SearchCustByDate(DateFrom, DateTo);
                decimal profit = 0;
                foreach ( var tran in trans )
                {
                    profit += tran.grandTotal;

                    if ( aa.ContainsKey((int)tran.dea_cust_id))
                    {
                        aa[(int)tran.dea_cust_id] += tran.grandTotal;
                    } else
                    {
                        aa.Add((int)tran.dea_cust_id, tran.grandTotal);
                    }
                }
                seriesTI.Points.AddXY(Monthname, profit);

                foreach (var value in custsprofit)
                {
                    if (aa.ContainsKey(value.Key))
                    {
                        value.Value.Points.AddXY(Monthname, aa[value.Key].ToString().Replace(",", "."));                        
                    } else
                    {
                        value.Value.Points.AddXY(Monthname, 0);
                    }

                }
                // do something with target.Month and target.Year
                now = now.AddMonths(1);
            }

            foreach (var point in seriesTI.Points)
            {
                point.ToolTip = point.YValues[0].ToString() + " €";
            }
            foreach (var cust in custs)
            {
                foreach (var point in custsprofit[cust.Id].Points)
                {
                    point.ToolTip = point.YValues[0].ToString() + " €";
                }

            }
        }

        private void frmAdminDashboard_Activated(object sender, EventArgs e)
        {
            LoadChart();
            LoadInventoryWarning();
            aTimer.Enabled = true;
        }

        private void frmAdminDashboard_Deactivate(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
        }

        private void setupTabControlLeaveEnter ()
        {
            tabControl.MouseEnter += new EventHandler(control_MouseEnter);
            tabControl.MouseEnter += new EventHandler(control_MouseLeave);
            foreach (TabPage page in tabControl.TabPages)
            {
                foreach (Control control in page.Controls)
                {
                    control.MouseEnter += new EventHandler(control_MouseEnter);
                    control.MouseLeave += new EventHandler(control_MouseLeave);
                }
            }
        }

        private void control_MouseEnter(object sender, EventArgs e)
        {
            if (Form.ActiveForm == this)
            {
                aTimer.Enabled = false;
            }
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            if (Form.ActiveForm == this)
            {
                aTimer.Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }


        private void ExtendmenuStripItems()
        {
            ToolStripMenuItem test = new ToolStripMenuItem();
            test.Name = "Test";
            test.Text = "Test";
            test.Tag = "Test";
            test.Click += new EventHandler(testToolStripMenuItem_Click);
            menuStripTop.Items.Add(test);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            frmTest test = new frmTest();
            test.Show();
        }
    }
}
