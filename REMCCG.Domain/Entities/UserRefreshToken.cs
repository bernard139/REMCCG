using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace REMCCG.Domain.Entities
{
    [Table("UserRefreshToken")]
    public partial class UserRefreshToken
    {
        [Key]
        public long Id { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string? UserName { get; set; }
        [StringLength(3000)]
        public string? RefreshToken { get; set; }
        public bool IsActive { get; set; }
        [StringLength(450)]
        public string? UserId { get; set; }

    }
}
