using Project.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public interface ISuperVisorService
    {
        Task<bool> Evalute(string projectId, Evalution evalution);
        Task<bool> MarkProject(string projectId, double mark);
        Task<List<Developer>> GetDevelopersInProject(string projectId);
        Task<bool> WriteWiki(string projectId, string wiki);
        Task<string> GetWiki(string projectId);
        Task<Framework> GetFramework(string projectId);
    }
}
