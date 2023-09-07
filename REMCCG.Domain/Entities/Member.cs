using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Member
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public string Password { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public ICollection<Remittance> Remittances { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ICollection<ServiceAttendance> ServiceAttendances { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<Blogpost> Blogpost { get; set; }
    }
}
