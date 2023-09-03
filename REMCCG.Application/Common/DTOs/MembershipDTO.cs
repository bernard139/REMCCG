using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class MembershipDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public DateTime DateJoined { get; set; }
        public Member Member { get; set; }
    }

    public class MembershipModel
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public DateTime DateJoined { get; set; }
        public Member Member { get; set; }
    }
}
