using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class ServiceAttendance
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int TotalAttendance { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}
