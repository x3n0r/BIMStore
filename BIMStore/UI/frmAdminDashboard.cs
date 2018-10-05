using BIMStore.DAL;
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

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
            LoadChart();
        }

        //Set a public static method to specify whether the form is purchase or sale
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers user = new frmUsers();
            user.Show();
        }

        private void frmAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategories category = new frmCategories();
            category.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProducts product = new frmProducts();
            product.Show();
        }

        private void deealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTransactions transaction = new frmTransactions();
            transaction.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void companydataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }


        private void lblbtnCompanyData_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }

        private void purchaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.transactionType = "Purchase";
            purchase.Show();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sale
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Sale";
            sale.Show();
        }

        private void bookingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sale
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Booking";
            sale.Show();
            sale.LoadBooking();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSales sales = new frmSales();
            sales.Show();
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
                    profit += (decimal)tran.grandTotal;

                    if ( aa.ContainsKey((int)tran.dea_cust_id))
                    {
                        aa[(int)tran.dea_cust_id] += (decimal)tran.grandTotal;
                    } else
                    {
                        aa.Add((int)tran.dea_cust_id, (decimal)tran.grandTotal);
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
        }
    }
}
