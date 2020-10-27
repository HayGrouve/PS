using Econtact.econtactClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Econtact
{
    public partial class Econtact : Form
    {
        public Econtact()
        {
            InitializeComponent();
        }
        ContactClass c = new ContactClass();
        DataTable dt = new DataTable();


        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Getting the values
            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtboxContactNumber.Text;
            c.Address = txtboxAdress.Text;
            c.Gender = cmbGender.Text;

            //Inserting the values
            bool success = c.Insert(c);
            if(success == true)
            {
                MessageBox.Show("New Contact Inserted!");
                Clear();
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Contact Was NOT Inserted!");
            }
        }
        public void Clear() {
            txtboxContactID.Text = "";
            txtboxFirstName.Text = "";
            txtboxLastName.Text = "";
            txtboxContactNumber.Text = "";
            txtboxAdress.Text = "";
            c.Gender = cmbGender.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting the values
                c.ContactID = int.Parse(txtboxContactID.Text);
                c.FirstName = txtboxFirstName.Text;
                c.LastName = txtboxLastName.Text;
                c.ContactNo = txtboxContactNumber.Text;
                c.Address = txtboxAdress.Text;
                c.Gender = cmbGender.Text;
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Invalid Input!");
                return;
            }
            
            //Updating the values
            bool success = c.Update(c);
            if (success == true)
            {
                MessageBox.Show("Contact Updated!");
                Clear();
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Contact Was NOT Updated!");
            }
        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            txtboxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            txtboxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            txtboxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            txtboxContactNumber.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            txtboxAdress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Getting the value
            try { 
            c.ContactID = int.Parse(txtboxContactID.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Invalid Input!");
                return;
            }

            //Updating the values
            bool success = c.Delete(c);
            if (success == true)
            {
                MessageBox.Show("Contact Deleted!");
                Clear();
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Contact Was NOT Deleted!");
            }
        }
        static string myConnStr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private void textboxSearch_TextChanged(object sender, EventArgs e)
        {
            //Getting the value
            string keyword = textboxSearch.Text;
            SqlConnection conn = new SqlConnection(myConnStr);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE FirstName LIKE '%"+keyword+"%' OR LastName LIKE '%"+keyword+ "%' OR Address LIKE '%"+keyword+"%'", conn);
            dt.Clear();
            adapter.Fill(dt);
            dgvContactList.DataSource = dt;


        }

        private static int count = 1;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string fileName = "Query" + count.ToString() + ".txt";
            StreamWriter myFile = new StreamWriter(fileName);
            foreach (DataRow row in dt.Rows)
            {
                myFile.Write("ID: ");
                foreach (DataColumn column in dt.Columns)
                {
                    myFile.Write(row[column]+ " ");
                }
                myFile.WriteLine("");
            }
            myFile.Close();
            count++;
        }
    }
}
