using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class ServiceAttendanceDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int TotalAttendance { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
    }

    public class ServiceAttendanceModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int TotalAttendance { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
    }
}
