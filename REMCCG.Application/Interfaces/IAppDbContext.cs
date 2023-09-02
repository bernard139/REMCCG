using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Domain.Entities;

namespace REMCCG.Application.Interfaces
{
    public interface IAppDbContext
    {
        #region Tables
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Blogpost> BlogPosts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<FunctionalDepartment> FunctionalDepartments { get; set; }
        public DbSet<ImageGallery> ImageGalleries { get; set; }
        public DbSet<ImageGalleryImage> imageGalleryImages { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Remittance> Remittances { get; set; }
        public DbSet<ServiceAssignment> ServiceAssignments { get; set; }
        public DbSet<ServiceAttendance> ServiceAttendances { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }


        #endregion







        Task<List<T>> GetData<T>(string query, params object[] param) where T : class;
        Task<int> ExecuteSqlCommand(string query, params object[] param);
        IDbContextTransaction Begin();
        Task CommitAsync();
        Task RollbackAsync();
        DbContext GetAppDbContext();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

    }
}
