using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Project.Api.Domain;
using Project.Api.Services;

namespace Project.Api.Installers
{
    public class NoSqlMongoInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<MongoProjectService>();
            services.Configure<ProjectStoreDatabaseSettings>(
            configuration.GetSection(nameof(ProjectStoreDatabaseSettings)));

            services.AddSingleton<IProjectStoreDatabaseSettings>(sp => {
                return sp.GetRequiredService<IOptions<ProjectStoreDatabaseSettings>>().Value;
            });

           
            services.AddSingleton<IMongoProjectService, MongoProjectService>();
        }
    }
}
