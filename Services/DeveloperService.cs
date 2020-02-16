using Cosmonaut;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly ICosmosStore<CosmosProjectDto> _NoDb;

        public DeveloperService(ICosmosStore<CosmosProjectDto> _NoDb)
        {
            this._NoDb = _NoDb;
        }

        public async Task<bool> AssignDone(string projectId, DoneViewModel model)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var done = new Done
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            project.Framework.Dones.Add(done);
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }

        public async Task<bool> AssignInProgress(string projectId, InProgressViewModel model)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var inprogress = new InProgress
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            project.Framework.Id = Guid.NewGuid().ToString();
            project.Framework.InProgress.Add(inprogress);
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }

        public async Task<bool> AssignToDo(string projectId, ToDoViewModel model)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var todo = new ToDo
            {
                Id = Guid.NewGuid().ToString(),
                Descreption = model.Descreption,
                Name = model.Name,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow
            };
            //give here null referrence exception
            project.Framework.Id= Guid.NewGuid().ToString();
            
            project.Framework.ToDos.Add(todo);
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;

        }

        public async Task<bool> DesignLogo(string projectId, string logoPath)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            project.ProjectLogo = logoPath;
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;

        }

        public async Task<string> DownloadProject(string projectId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            return project.ProjectPath;

        }

        public async Task<Framework> GetFramework(string projectId)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            var frameworks = project.Framework;
            return frameworks;
        }
        public async Task<bool> CreateFramework([FromRoute]string projectId)
        {

            var framework = new Framework
            {
                Id = Guid.NewGuid().ToString(),
                ToDos = new List<ToDo>() { },
                InProgress = new List<InProgress>() { },
                Dones = new List<Done>() { }
            };
            var project = await _NoDb.FindAsync(projectId,  projectId);
            project.Framework = framework;
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }


        public async Task<bool> UploadProject(string projectId, string projectPath)
        {
            var project = await _NoDb.FindAsync(projectId, projectId);
            project.ProjectPath = projectPath;
            var result = await _NoDb.UpdateAsync(project);
            return result.IsSuccess;
        }



    }
}
