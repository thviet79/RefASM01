using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BusinessObject;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace MyStoreWinApp
{
    public partial class frmLogin : Form
    {
        public static Member admin;

        private readonly static string configuationFile = "appsettings.json";
        private readonly static string MemberID = "Admin:MemberID";
        private readonly static string Email = "Admin:Email";
        private readonly static string Password = "Admin:Password";
        private readonly static string MemberName = "Admin:MemberName";
        private readonly static string City = "Admin:City";
        private readonly static string Country = "Admin:Country";
        private MemberDAO memberDAO; 
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isAdmin = false;

            Member loginMember = null;

            if (txtEmail.Text.Equals(admin.Email) &&
                txtPassword.Text.Equals(admin.Password)){

                isAdmin = true;
                loginMember = admin;

            }

            if (loginMember == null) {

                loginMember =  memberDAO.GetMemberList.Find(member => 
                    txtEmail.Text.Equals(member.Email) &&
                    txtPassword.Text.Equals(member.Password)
                );

            }
            if (loginMember != null)
            {
                frmMemberManagement management = new frmMemberManagement();

                frmMemberManagement.Member = loginMember;
                frmMemberManagement.IsAdmin = isAdmin;
                frmMemberManagement.loginForm = this;
                management.Show();

                this.Hide();
            }
           
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtEmail.Clear();
            txtPassword.Clear();
        }

        private void AffterLoadForm(object sender, EventArgs e)
        {
            if (admin == null)
            {
                var serviceCollection = new ServiceCollection();
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile(configuationFile)
                    .Build();

                admin = new Member();
                admin.MemberID = Int32.Parse(configuration.GetSection(MemberID).Value); admin.MemberID = Int32.Parse(configuration.GetSection(MemberID).Value);
                admin.MemberName = configuration.GetSection(MemberName).Value;
                admin.Email = configuration.GetSection(Email).Value;
                admin.Password = configuration.GetSection(Password).Value;
                admin.City = configuration.GetSection(City).Value;
                admin.Country = configuration.GetSection(Country).Value;
                
                this.memberDAO = MemberDAO.Instance;
            }

        }
    }
}
