
using REMCCG.Application.Implementations.ImageGalleries;
using REMCCG.Application.Services;

namespace REMCCG.Persistence.Configurations
{
    public static class ServiceRegistry
    {
        public static IServiceCollection PushService(this IServiceCollection services, IConfiguration conf)
        {
            //Add services here

            #region Mapster 
            TypeAdapterConfig.GlobalSettings.Default
           .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
           .IgnoreNullValues(true)
           .AddDestinationTransform((string x) => x.Trim())
           .AddDestinationTransform((string x) => x ?? "")
           .AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
            services.RegisterMapsterConfiguration();

            #endregion

            #region Other services
            
            var conString = conf["ConnectionStrings:REMCCGDbContextConnection"];
            services.AddDbContext<IAppDbContext, REMCCGDbContext>(options => options.UseSqlServer(conString));
            services.AddDefaultIdentity<ApplicationUser>(opt => opt.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<REMCCGDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<IAccountLogin, AccountLogin>();

            //User services:

            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IAccountLogout, AccountLogout>();
            services.AddScoped<IAccountRegister, AccountRegister>();
            services.AddScoped<IUserService, UserService>();

            //Other Services:

            services.AddScoped<IAppDbContext, REMCCGDbContext>();
            services.AddScoped<IAttendanceRecordService, AttendanceRecordService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IFunctionalDepartmentService, FunctionalDepartmentService>();
            services.AddScoped<IImageGalleryImageService, ImageGalleryImageService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IImageGalleryService, ImageGalleryService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IRemittanceService, RemittanceService>();
            services.AddScoped<IServiceAssignmentService, ServiceAssignmentService>();
            services.AddScoped<IServiceAttendanceService, ServiceAttendanceService>();

            #endregion


            return services;
        }
    }
}
