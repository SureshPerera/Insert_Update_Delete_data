using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace update_delete_data
{
    public partial class Form1 : Form
    {   
        Customer model = new Customer(); //create object to Customer table 
        DBEntities db = new DBEntities(); //  create object to connection
        string connectingString = @"Data Source=SURESH;Initial Catalog=EFDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();                    // When form load clear all
            PopulateDataGridViwe();     
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClancel_click(object sender, EventArgs e)
        {
            Clear();        
        }
        void Clear() // clear all input & disable delate btn
        {
            txtFirstName.Text = txtLastName.Text = txtcity.Text = txtAddress.Text = "";
            btnSave.Text = "Add";
            btnDelect.Enabled = false;
            model.CustomerId = 0;
        }
        void PopulateDataGridViwe()
        {
            dgvCustomers.AutoGenerateColumns = false; // stop to autoganarate collom "Address"
            using (SqlConnection sqlCon = new SqlConnection(connectingString)) // get to database data in to grid table 
            {
                dgvCustomers.DataSource = db.Customers.ToList<Customer>();
            }
        }
        private void btnSave_click(object sender, EventArgs e)
        {
            try
            {
              
                using (SqlConnection sqlCon = new SqlConnection(connectingString))
                {
                   
                        sqlCon.Open();
                        SqlCommand cmd = new SqlCommand("AddCustomerData", sqlCon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtcity.Text.Trim());
                        cmd.Parameters.AddWithValue("@Addresss", txtAddress.Text.Trim());
                        cmd.ExecuteNonQuery();

                   
                }
                Clear();
                MessageBox.Show("Submited SuccessFully");
                PopulateDataGridViwe();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnDelect_click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete?","EF CRUD OPERATIONS",MessageBoxButtons.YesNo)==DialogResult.Yes) 
            {
                using (SqlConnection sqlCon = new SqlConnection(connectingString))
                {
                    //var entry = db.Entry(model);
                   
                        //db.Customers.Attach(model);
                        db.Customers.Remove(model); //delect the record 
                        db.SaveChanges();
                        PopulateDataGridViwe();
                        Clear();
                        MessageBox.Show("Delated Successfuly");
                    
                }
            }
        }

        private void dgvCustomer_doubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvCustomers.CurrentRow.Index != -1)
                {
                    model.CustomerId = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerId"].Value);
                    using (SqlConnection sqlCon = new SqlConnection(connectingString))
                    {
                        model = db.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                        txtFirstName.Text = model.FirstName;
                        txtLastName.Text = model.LastName;
                        txtcity.Text = model.City;
                        txtAddress.Text = model.Addresss;
                    }
                    btnSave.Text = "Update";
                    btnDelect.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
