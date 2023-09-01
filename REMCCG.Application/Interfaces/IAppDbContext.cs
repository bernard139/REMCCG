using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces
{
    public interface IAppDbContext
    {
        #region Tables
        // DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<UserActivities> UserActivities { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<LineOfBusiness> LineOfBusinesses { get; set; }
        public DbSet<AssignedProject> AssignedProjects { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectModule> ProjectModules { get; set; }
        public DbSet<ProjectReportedIssue> ProjectReportedIssues { get; set; }
        public DbSet<ProjectResolvedIssue> ProjectResolvedIssues { get; set; }
        public DbSet<StandardOperatingProcedure> StandardOperatingProcedures { get; set; }
        public DbSet<StandardOperatingProcedureHistory> StandardOperatingProcedureHistories { get; set; }
        public DbSet<Permission> Permissions { get; set; }


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
