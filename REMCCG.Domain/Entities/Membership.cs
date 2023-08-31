using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Membership
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public DateTime DateJoined { get; set; }
        public Member Member { get; set; }
    }
}
