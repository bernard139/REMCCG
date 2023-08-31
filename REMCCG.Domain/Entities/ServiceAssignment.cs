using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class ServiceAssignment
    {
        public int ID { get; set; }
        public int AttendanceEventID { get; set; } // ID of the assigned attendance event
        public ServiceAttendance AttendanceEvent { get; set; } // Navigation property for the assigned attendance event
        public string LeaderId { get; set; } // ApplicationUser Id of the event leader
        public ApplicationUser Leader { get; set; } // Navigation property for the event leader
    }
}
