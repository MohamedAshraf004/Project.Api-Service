using Cosmonaut;
using Cosmonaut.Extensions;
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ICosmosStore<CosmosProjectDto> _NoDb;

        public ProjectService(ICosmosStore<CosmosProjectDto> _NoDb)
        {
            this._NoDb = _NoDb;
        }

        public async Task<List<CreateProjectViewModel>> GetAllProjects()
        {
            var projects = await _NoDb.Query()
                           .Select(x => new CreateProjectViewModel { Name = x.ProjectName })
                           .ToListAsync();

            return projects;
        }


        public async Task<CosmosProjectDto> GetProject(string projectId)
        {
            return await _NoDb.Query().SingleOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<List<CosmosProjectDto>> Search(string projectName)
        {
            var projects = await _NoDb.Query().Where(p => p.ProjectName == projectName).ToListAsync();
            return projects;
        }

        public async Task<bool> CreateProject(CreateProjectViewModel model)
        {
            var Project = new CosmosProjectDto()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectName = model.Name,
                ProjectLogo = null,
                Mark = 0,
                Developer = new List<Developer>() { },
                Evalution = 0,
                Framework = new Framework() { },
                ProjectPath = null,
                SuperVisior = new SuperVisor() { },
                Wiki = null
            };
            var result = await _NoDb.AddAsync(Project);
            return result.IsSuccess;
        }

        public async Task<bool> UpdateProject(string projectId, CosmosProjectDto updatedProject)
        {
            var oldProject = await _NoDb.FindAsync(projectId, projectId);
            if (oldProject != null)
            {
                oldProject.ProjectName = updatedProject.ProjectName;
                var result = await _NoDb.UpdateAsync(oldProject);
                return result.IsSuccess;
            }
            return false;
        }

        public async Task<bool> RemoveProject(string projectId)
        {
            var result = await _NoDb.RemoveByIdAsync(projectId, projectId);
            return result.IsSuccess;
        }

        public async Task<bool> AddDeveloperToProject(string projectId, Developer developer)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            if (project != null && developer != null)
            {
                developer.Id = Guid.NewGuid().ToString();
                project.Developer.Add(developer);
                var result = await _NoDb.UpdateAsync(project);
                return result.IsSuccess;
            }
            else
                return false;
        }

        public async Task<bool> AddSuperVisorToProject(string projectId, SuperVisor superVisor)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            if (project != null && superVisor != null)
            {
                project.SuperVisior = superVisor;
                var result = await _NoDb.UpdateAsync(project);
                return result.IsSuccess;
            }
            else
                return false;
        }

        public async Task<bool> RemoveDeveloperFromProject(string projectId, string developerId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var deletedDeveloper = project.Developer.SingleOrDefault(d => d.Id == developerId);

            project.Developer.Remove(deletedDeveloper);
            var result = await _NoDb.UpdateAsync(project);

            return result.IsSuccess;
        }


    }
}
