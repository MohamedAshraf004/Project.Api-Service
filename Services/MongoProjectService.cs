using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using Project.Api.Domain.MongoDomains;

namespace Project.Api.Services
{
    public class MongoProjectService : IMongoProjectService
    {



        private readonly IMongoCollection<MongoProject> _Projects;
        public MongoProjectService(IProjectStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _Projects = database.GetCollection<MongoProject>(settings.ProjectsCollectionName);
        }
        #region Get
        public async Task<IEnumerable<MongoProject>> GetAllProjects()
        {
            var result = await _Projects.FindAsync(student => true);
            var projects = result.ToEnumerable();
            return projects;

        }
        public async Task<MongoProject> GetProjectById(string id)
        {
            var result = await _Projects.FindAsync(project => project.Id == id);
            var getProject = await result.SingleOrDefaultAsync();
            return getProject;
        }
        public async Task<MongoDeveloper> GetDeveloperById(string developerId, string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var developer = project.Developer.SingleOrDefault(d => d.Id == developerId);
            return developer;

        }



        public async Task<IEnumerable<MongoProject>> GetProjectByName(string name)
        {
            var result = await _Projects.FindAsync(project => project.ProjectName.Contains(name));
            var getProjects = result.ToEnumerable();
            return getProjects;
        }

        public async Task<List<MongoToDo>> GetAllToDo(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var toDos = project.Framework.ToDos;
            return toDos;
        }

        public async Task<List<MongoInProgress>> GetAllInProgress(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var inProgress = project.Framework.InProgress;
            return inProgress;
        }

        public async Task<List<MongoDone>> GetAllDone(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var dones = project.Framework.Dones;
            return dones;
        }
        public async Task<MongoFramework> GetFramework(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var framework = project.Framework;
            return framework;
        }

        public async Task<List<MongoDeveloper>> GetDevelopersInProject(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var developers = project.Developer;
            return developers;
        }

        public async Task<string> GetWiki(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var wiki = project.Wiki;
            return wiki;
        }

        public async Task<string> DownloadProject(string projectId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var path = project.ProjectPath;
            return path;
        }
        #endregion
        #region Post
        public async Task<bool> CreateProject(CreateProjectViewModel model)
        {
            var project = new MongoProject
            {
                ProjectName = model.Name
            };
            await _Projects.InsertOneAsync(project);
            return true;
        }
        public async Task<bool> CreateFramework(string projectId)
        {
            var framework = new MongoFramework
            {
                Id = Guid.NewGuid().ToString(),
                ToDos = new List<MongoToDo>() { },
                InProgress = new List<MongoInProgress>() { },
                Dones = new List<MongoDone>() { }
            };
            var getProject = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await getProject.SingleOrDefaultAsync();
            project.Framework = framework;
            var result = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return result.IsAcknowledged;
        }
        #endregion

        #region Put
        public async Task<bool> UpdateProject(string id, MongoProject projectIn)
        {
            var oldProject = await GetProjectById(id);
            oldProject.ProjectName = projectIn.ProjectName;
            var result = await _Projects.ReplaceOneAsync(project => project.Id == id, oldProject);
            return result.IsAcknowledged;
        }
        public async Task<bool> AddDeveloperToProject(string projectId, MongoDeveloper developer)
        {
            var oldProject = await GetProjectById(projectId);
            developer.Id = Guid.NewGuid().ToString();
            oldProject.Developer.Add(developer);
            var result = await _Projects.ReplaceOneAsync(project => project.Id == projectId, oldProject);
            return result.IsAcknowledged;
        }
        public async Task<bool> AddSuperVisorToProject(string projectId, MongoSuperVisor superVisor)
        {
            var oldProject = await GetProjectById(projectId);
            superVisor.Id = Guid.NewGuid().ToString();
            oldProject.SuperVisior = superVisor;
            var result = await _Projects.ReplaceOneAsync(project => project.Id == projectId, oldProject);
            return result.IsAcknowledged;
        }

        public async Task<bool> DesignLogo(string projectId, string logoPath)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            project.ProjectLogo = logoPath;
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> UploadProject(string projectId, string projectPath)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            project.ProjectPath = projectPath;
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> AssignToDo(string projectId, ToDoViewModel model)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var toDo = new MongoToDo
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            project.Framework.ToDos.Add(toDo);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> AssignDone(string projectId, DoneViewModel model)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var done = new MongoDone
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            project.Framework.Dones.Add(done);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> AssignInProgress(string projectId, InProgressViewModel model)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var inprogress = new MongoInProgress
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            project.Framework.InProgress.Add(inprogress);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> Evalute(string projectId, MongoEvalution evalution)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            project.Evalution = evalution;
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> MarkProject(string projectId, double mark)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            project.Mark = mark;
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        public async Task<bool> WriteWiki(string projectId, string wiki)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            project.Wiki = wiki;
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }
        #endregion

        #region Delete
        public async Task<bool> RemoveProject(MongoProject projectIn)
        {
            var result = await _Projects.DeleteOneAsync(project => project.Id == projectIn.Id);
            return result.IsAcknowledged;
        }
        public async Task<bool> RemoveProject(string id)
        {
            var result = await _Projects.DeleteOneAsync(project => project.Id == id);
            return result.IsAcknowledged;
        }
        public async Task<bool> RemoveDeveloperFromProject(string projectId, string developerId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var deletedDeveloper = project.Developer.SingleOrDefault(d => d.Id == developerId);
            project.Developer.Remove(deletedDeveloper);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }
        public async Task<bool> RemoveToDOFromProject(string projectId, string todoId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var deletedToDo = project.Framework.ToDos.SingleOrDefault(d => d.Id == todoId);
            project.Framework.ToDos.Remove(deletedToDo);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }
        public async Task<bool> RemoveInProgressFromProject(string projectId, string inprogressId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var deletedInProgress = project.Framework.InProgress.SingleOrDefault(d => d.Id == inprogressId);
            project.Framework.InProgress.Remove(deletedInProgress);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }
        public async Task<bool> RemoveDoneFromProject(string projectId, string doneId)
        {
            var result = await _Projects.FindAsync(proj => proj.Id == projectId);
            var project = await result.SingleOrDefaultAsync();
            var deletedDone = project.Framework.Dones.SingleOrDefault(d => d.Id == doneId);
            project.Framework.Dones.Remove(deletedDone);
            var resultReplace = await _Projects.ReplaceOneAsync(proj => proj.Id == projectId, project);
            return resultReplace.IsAcknowledged;
        }

        #endregion
    }
}

