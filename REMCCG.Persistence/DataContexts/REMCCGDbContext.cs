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
        public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public virtual DbSet<Blogpost> BlogPosts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public  virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<FunctionalDepartment> FunctionalDepartments { get; set; }
        public virtual DbSet<ImageGallery> ImageGalleries { get; set; }
        public virtual DbSet<ImageGalleryImage> ImageGalleryImages { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Remittance> Remittances { get; set; }
        public virtual DbSet<ServiceAssignment> ServiceAssignments { get; set; }
        public virtual DbSet<ServiceAttendance> ServiceAttendances { get; set; }
        public virtual DbSet<UserActivity> UserActivities { get; set; }


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

            modelBuilder.Entity<AttendanceRecord>().ToTable("AttendanceRecords").HasKey(ar => ar.ID);
            modelBuilder.Entity<Blogpost>().ToTable("BlogPosts").HasKey(b => b.ID);
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Expense>().ToTable("Expenses");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<FunctionalDepartment>().ToTable("FunctionalDepartments");
            modelBuilder.Entity<ImageGallery>().ToTable("ImageGalleries");
            modelBuilder.Entity<ImageGalleryImage>().ToTable("ImageGalleryImages");
            modelBuilder.Entity<Member>().ToTable("Members");
            modelBuilder.Entity<Membership>().ToTable("Memberships");
            modelBuilder.Entity<Remittance>().ToTable("Remittances");
            modelBuilder.Entity<ServiceAssignment>().ToTable("ServiceAssignments");
            modelBuilder.Entity<ServiceAttendance>().ToTable("ServiceAttendances");
            modelBuilder.Entity<UserActivity>().ToTable("UserActivities");

            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.AttendanceEvent)
                .WithMany(ae => ae.AttendanceRecords)
                .HasForeignKey(ar => ar.AttendanceEventID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.Member)
                .WithMany(m => m.AttendanceRecords)
                .HasForeignKey(ar => ar.MemberID)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Blogpost>()
                .ToTable("BlogPosts")
                .HasKey(b => b.ID); // Define the primary key for Blogpost

            modelBuilder.Entity<Blogpost>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Blogpost)
                .HasForeignKey(b => b.MemberID);

            modelBuilder.Entity<Report>()
                .ToTable("Reports")
                .HasOne(r => r.Member)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.MemberID);

            modelBuilder.Entity<ImageGalleryImage>()
                .ToTable("ImageGalleryImages")
                .HasOne(igi => igi.Gallery)
                .WithMany(ig => ig.Images)
                .HasForeignKey(igi => igi.GalleryID);

            modelBuilder.Entity<Member>()
                .ToTable("Members")
                .HasKey(m => m.ID);


            modelBuilder.Entity<Membership>()
                .ToTable("Memberships")
                .HasOne(m => m.Member)
                .WithMany(me => me.Memberships)
                .HasForeignKey(m => m.MemberID);

            modelBuilder.Entity<Remittance>()
                .ToTable("Remittances")
                .HasOne(r => r.Member)
                .WithMany(u => u.Remittances)
                .HasForeignKey(r => r.MemberID);

            modelBuilder.Entity<ServiceAttendance>()
                .ToTable("ServiceAttendances")
                .HasOne(sa => sa.Member)
                .WithMany(u => u.ServiceAttendances)
                .HasForeignKey(sa => sa.MemberID);


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
