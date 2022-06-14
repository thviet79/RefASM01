using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess;

namespace DataAccess.Repository
{
    public class MemberRepository: IMemberRepository
    {
        public Member GetMemberByID(int memberId) => MemberDAO.Instance.GetMemberByID(memberId);
        public IEnumerable<Member> GetMembers() => MemberDAO.Instance.GetMemberList;
        public void InsertMember(Member member) => MemberDAO.Instance.AddNew(member);
        public void DeleteMember(int memberId) => MemberDAO.Instance.Remove(memberId);
        public void UpdateMember(Member member) => MemberDAO.Instance.Update(member);
    }
}
