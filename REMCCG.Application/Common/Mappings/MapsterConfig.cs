using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            #region   the mappings start here

            TypeAdapterConfig<ApplicationUser, ApplicationUserDto>
                .NewConfig()
                .Map(dest => dest.UserId, src => src.Id);

            #endregion Mapping ends here



            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
