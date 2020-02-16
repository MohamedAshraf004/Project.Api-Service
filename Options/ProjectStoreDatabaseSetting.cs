using Project.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Domain
{
    public class ProjectStoreDatabaseSettings : IProjectStoreDatabaseSettings
    {
        public string ProjectsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IProjectStoreDatabaseSettings
    {
        string ProjectsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
