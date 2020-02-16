
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public interface IProjectService
    {
        Task<List<CreateProjectViewModel>> GetAllProjects();
        Task<CosmosProjectDto> GetProject(string projectId);
        Task<bool> CreateProject(CreateProjectViewModel model);
        Task<bool> UpdateProject(string projectId, CosmosProjectDto updatedProject);
        Task<bool> RemoveProject(string projectId);
        Task<List<CosmosProjectDto>> Search(string projectName);
        Task<bool> AddDeveloperToProject(string projectId, Developer developer);
        Task<bool> RemoveDeveloperFromProject(string projectId, string developerId);
        // Task<bool> AddSuperVisorToProject(string projectId,string superVisorID);
    }
}
