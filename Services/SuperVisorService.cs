using Cosmonaut;
using Project.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public class SuperVisorService : ISuperVisorService
    {
        private readonly ICosmosStore<CosmosProjectDto> _NoDb;

        public SuperVisorService(ICosmosStore<CosmosProjectDto> _NoDb)
        {
            this._NoDb = _NoDb;
        }
        public async Task<bool> Evalute(string projectId, Evalution evalution)
        {
            var EvaluatedProject = await _NoDb.FindAsync(projectId, projectId);
            EvaluatedProject.Evalution = evalution;
            var result = await _NoDb.UpdateAsync(EvaluatedProject);
            return result.IsSuccess;

        }

        public async Task<List<Developer>> GetDevelopersInProject(string projectId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var developers = project.Developer;
            if (developers.Count > 0)
                return null;
            return developers;
        }

        public async Task<Framework> GetFramework(string projectId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var framework = project.Framework;
            return framework;
        }

        public async Task<string> GetWiki(string projectId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var wiki = project.Wiki;
            return wiki;
        }
        public async Task<bool> WriteWiki(string projectId, string wikiPath)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            project.Wiki = wikiPath;
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }

        public async Task<bool> MarkProject(string projectId, double mark)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            project.Mark = mark;
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }

    }
}
