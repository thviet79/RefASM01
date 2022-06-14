using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BusinessObject;
namespace DataAccess
{
    public class MemberDAO
    {
        private static List<Member> MemberList = new List<Member>()
        {
            new Member{MemberID=1, MemberName="John", Email="John@gmail.com", Password="12345", City="New York",Country="USA"},
            new Member{MemberID=2, MemberName="Peter", Email="Peter@gmail.com", Password="12345", City="Munich",Country="Germany"},
        };
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }
        public List<Member> GetMemberList => MemberList;  // GetMemberList => MemberList;
        public Member GetMemberByID(int memberID)
        {
            Member member = MemberList.SingleOrDefault(pro => pro.MemberID == memberID);
            return member;
        }
        public void AddNew(Member member)
        {
            Member pro = GetMemberByID(member.MemberID);
            if (pro == null)
            {
                MemberList.Add(member);
            }
            else
            {
                throw new Exception("Member is already exits.");
            }
        }
        public void Update(Member member)
        {
            Member m = GetMemberByID(member.MemberID);
            if (m != null)
            {
                var index = MemberList.IndexOf(m);
                MemberList[index] = member;
            }
            else
            {
                throw new Exception("Member does not already exits.");
            }
        }
        public void Remove(int MemberID)
        {
            Member m = GetMemberByID(MemberID);
            if (m != null)
            {
                MemberList.Remove(m);
            }
            else
            {
                throw new Exception("Member does not already exits.");
            }
        }
    }
}
