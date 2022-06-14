using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessObject;
using DataAccess;
using DataAccess.Repository;

namespace MyStoreWinApp
{
    public partial class frmMemberDetails : Form
    {
        public frmMemberDetails()
        {
            InitializeComponent();
        }
        public MemberDAO MemberRepository;
        public bool InsertOrUpdate { get; set; }
        public static Member MemberInfo { get; set; }
        private void frmMemberDetails_Load(object sender, EventArgs e)
        {
            //cmbManufacturer.SelectedIndex = 0;
            txtMemberID.Enabled = !InsertOrUpdate;
            MemberRepository = MemberDAO.Instance;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var member = new Member
                {
                    MemberID = int.Parse(txtMemberID.Text),
                    MemberName = txtMemberName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    City=txtCity.Text,
                    Country = txtCountry.Text
                };

                MemberRepository.Update(member);
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add a new Member" : "Update a member");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private void frmMemberDetails_show(object sender, EventArgs e)
        {
            txtMemberID.Text = MemberInfo.MemberID.ToString();
            txtMemberName.Text = MemberInfo.MemberName;
            txtEmail.Text = MemberInfo.Email.ToString();
            txtPassword.Text = MemberInfo.Password.ToString();
            txtCity.Text = MemberInfo.City.ToString();
            txtCountry.Text = MemberInfo.Country.ToString();
        }
    }
}
