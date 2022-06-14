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
using DataGridViewAutoFilter;
using MoreLinq;

namespace MyStoreWinApp
{
    public partial class frmMemberManagement : Form
    {
        IMemberRepository memberRepository = new MemberRepository();
        //BindingSource source;
        MemberDAO memberDAO = MemberDAO.Instance;

        public static bool IsAdmin;
        public static Member Member = new Member();
        public static frmLogin loginForm;
        public static frmMemberDetails DetailsMember;

        BindingSource Source;
        public frmMemberManagement()
        {
            InitializeComponent();
        }

        private void frmMemberManagement_Shown(object sender, EventArgs e)
        {
            Source = new BindingSource();

            txtMemberID.Text = Member.MemberID.ToString();
            txtMemberName.Text = Member.MemberName;
            txtEmail.Text = Member.Email;
            txtPassword.Text = Member.Password;
            txtCity.Text = Member.City;
            txtCountry.Text = Member.Country;

            city.FilteringEnabled = true;
            country.FilteringEnabled = true;

            if (!IsAdmin)
            {
                btnNew.Hide();
                btnDelete.Hide();
            }

            LoadMemberList();
        }

        private void frmMemberManagement_Load(object sender, EventArgs e)
        {
            dgvMemberList.CellDoubleClick += DgvMemberList_CellDoubleClick;
        }
        private void DgvMemberList_CellDoubleClick(object sender, EventArgs e)
        {
            frmMemberDetails frmMemberDetails = new frmMemberDetails
            {
                Text = "Update Member",
            };
            if (frmMemberDetails.ShowDialog() == DialogResult.OK)
            {
                LoadMemberList();
            }
            dgvMemberList.AutoGenerateColumns = true;
        }
        private void ClearText()
        {
            txtMemberID.Text = string.Empty;
            txtMemberName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCountry.Text = string.Empty;
        }
        private Member GetMemberObject()
        {
            Member member = null;
            try
            {
                member = new Member
                {
                    MemberID = int.Parse(txtMemberID.Text),
                    MemberName = txtMemberName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    City = txtCity.Text,
                    Country = txtCountry.Text
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Member");
            }
            return member;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            loginForm.Show();
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Member insertMember = new Member();
            insertMember.MemberID = Int32.Parse(txtMemberID.Text);
            insertMember.MemberName = txtMemberName.Text;
            insertMember.Email = txtEmail.Text;
            insertMember.Password = txtPassword.Text;
            insertMember.City = txtCity.Text;
            insertMember.Country = txtCountry.Text;

            memberDAO.AddNew(insertMember);
            LoadMemberList();
        }

        private void LoadMemberList(object sender, MouseEventArgs e)
        {
            LoadMemberList();
        }

        public void LoadMemberList()
        {
            var members = memberRepository.GetMembers();
            try
            {
                Source.DataSource = memberDAO.GetMemberList.ToDataTable();
                dgvMemberList.DataSource = Source;

                if (memberDAO.GetMemberList.Any()) {
                    btnDelete.Enabled = true;
                    return;
                }

                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }


        private void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == 5 && e.Value != null)
            {
                dgvMemberList.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('\u25CF', e.Value.ToString().Length);
                return;
            }
        }

        private void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string filterStatus = DataGridViewAutoFilterTextBoxColumn.GetFilterStatus(dgvMemberList);
            if (string.IsNullOrEmpty(filterStatus))
            {
                FilterStatusLabel.Visible = false;
            }
            else
            {
                FilterStatusLabel.Visible = true;
                FilterStatusLabel.Text = filterStatus;
            }

        }

        private void dgvMemberList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = dgvMemberList.CurrentCell.RowIndex;

            if (!String.IsNullOrEmpty(dgvMemberList.Rows[currentRowIndex].Cells[0].Value.ToString()))
            {
                if (DetailsMember == null) {
                    DetailsMember = new frmMemberDetails();
                }

                int selectedMemberId = Int32.Parse(dgvMemberList.Rows[currentRowIndex].Cells[0].Value.ToString());

                if (IsAdmin)
                {
                    frmMemberDetails.MemberInfo = memberDAO.GetMemberList.Find(member => member.MemberID == selectedMemberId);
                    DetailsMember.ShowDialog();
                    return;
                }

                if (selectedMemberId != Member.MemberID)
                {
                    MessageBox.Show("Bạn không có quyền truy cập vào đây","warning" );
                    return;
                }

                frmMemberDetails.MemberInfo = memberDAO.GetMemberList.Find(member => member.MemberID == selectedMemberId);
                DetailsMember.ShowDialog();
            }
           

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            String searchContent = txtSearch.Text.ToLower();
            int defaultId = 0;
            Member searchMember = null;
            if (rdMemberID.Checked)
            {
                int.TryParse(searchContent, out defaultId);
                searchMember = memberDAO.GetMemberList.Find(member => member.MemberID == defaultId);
            }

            if (rdMemberName.Checked)
            {
                searchMember = memberDAO.GetMemberList.Find(member => member.MemberName.ToLower().Contains(searchContent));
            }

            if (searchMember == null)
            {
                MessageBox.Show("Không tìm thấy bản ghi", "info");
                return;
            }

            Source.DataSource = new List<Member> { searchMember };


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (IsAdmin)
            {
                int currentRowIndex = dgvMemberList.CurrentCell.RowIndex;
                int selectedMemberId = Int32.Parse(dgvMemberList.Rows[currentRowIndex].Cells[0].Value.ToString());
                memberDAO.Remove(selectedMemberId);
                MessageBox.Show("Xóa Thành Công", "info");
                LoadMemberList();
            }
        }
    }
}
