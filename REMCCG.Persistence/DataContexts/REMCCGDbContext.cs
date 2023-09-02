using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Infrastructure.DataContexts
{
    public partial class REMCCGDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                                            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, 
                                            ApplicationRoleClaim, ApplicationUserToken>, IAppDbContext
    {
        public REMCCGDbContext()
        {
        }

        public REMCCGDbContext(DbContextOptions<REMCCGDbContext> options)
            : base(options)
        {
        }

        #region Tables
        public virtual DbSet<ApplicationUser> Users { get; set; }
        //public DbSet<ApplicationRole> Roles { get; set; }
        public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public virtual DbSet<Blogpost> BlogPosts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public  virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<FunctionalDepartment> FunctionalDepartments { get; set; }
        public virtual DbSet<ImageGallery> ImageGalleries { get; set; }
        public virtual DbSet<ImageGalleryImage> imageGalleryImages { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Remittance> Remittances { get; set; }
        public virtual DbSet<ServiceAssignment> ServiceAssignments { get; set; }
        public virtual DbSet<ServiceAttendance> ServiceAttendances { get; set; }
        public virtual DbSet<UserActivities> UserActivities { get; set; }


        #endregion


        #region Data Helpers

        public async Task<List<T>> GetData<T>(string query, params object[] param) where T : class
        {
            List<T> data = new();
            if (param != null && param.Length > 0)
            {
                var para = string.Join(",", param);
                data = await this.Set<T>().FromSqlRaw($"{query} {para}", param).ToListAsync();
            }
            else
            {
                data = await this.Set<T>().FromSqlRaw(query).ToListAsync();
            }
            return data;
        }
        public IQueryable<T> GetDataQueriable<T>(string query, params object[] param) where T : class
        {
            IQueryable<T> data = null;
            if (param != null && param.Length > 0)
            {
                var para = string.Join(",", param);
                data = this.Set<T>().FromSqlRaw($"{query} {para}", param);
            }
            else
            {
                data = this.Set<T>().FromSqlRaw(query);
            }
            return data;
        }

        public async Task<int> ExecuteSqlCommand(string query, params object[] param)
        {
            var para = string.Join(",", param.ToList());
            var result = await this.ExecuteSqlCommand($"{query} {para}", param);
            return result;
        }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<AttendanceRecord>().ToTable("AttendanceRecords");
            modelBuilder.Entity<Blogpost>().ToTable("BlogPosts");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Expenses>().ToTable("Expenses");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<FunctionalDepartment>().ToTable("FunctionalDepartments");
            modelBuilder.Entity<ImageGallery>().ToTable("ImageGalleries");
            modelBuilder.Entity<ImageGalleryImage>().ToTable("ImageGalleryImages");
            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Membership>().ToTable("Memberships");
            modelBuilder.Entity<Remittance>().ToTable("Remittances");
            modelBuilder.Entity<ServiceAssignment>().ToTable("ServiceAssignments");
            modelBuilder.Entity<ServiceAttendance>().ToTable("ServiceAttendances");
            modelBuilder.Entity<UserActivities>().ToTable("UserActivities");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



        public IDbContextTransaction Begin()
        {
            var trans = this.Database.CurrentTransaction;
            if (this.Database.CurrentTransaction == null)
            {
                trans = this.Database.BeginTransaction();
            }
            return trans;
        }
        public async Task CommitAsync()
        {
            var trans = Begin();

            if (trans != null)
            {

                await trans.CommitAsync();
            }

        }
        public async Task RollbackAsync()
        {
            var trans = Begin();

            if (trans != null)
            {
                await trans.RollbackAsync();
            }

        }
        public DbContext GetAppDbContext()
        {
            return this;
        }
    }
}
