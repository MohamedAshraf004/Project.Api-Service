using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Configuration;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Project.Api.Services;

namespace Project.Api.Installers
{
    public class NoSqlCosmosInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<ISuperVisorService, SuperVisorService>();
            services.AddSingleton<IDeveloperService, DeveloperService>();
            var cosmosStoreSettings = new CosmosStoreSettings(
                configuration["CosmosSettings:DatabaseName"],
                configuration["CosmosSettings:AccountUrl"],
                configuration["CosmosSettings:AccountKey"],
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp  }
                );

            services.AddCosmosStore<Domain.CosmosProjectDto>(cosmosStoreSettings);

            



        }
    }
}
