using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using Project.Api.Domain.MongoDomains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public interface IMongoProjectService
    {
        //get
        Task<IEnumerable<MongoProject>> GetAllProjects();
        Task<MongoProject> GetProjectById(string projectId);
        Task<IEnumerable<MongoProject>> GetProjectByName(string projectName);
        Task<MongoFramework> GetFramework(string projectId);
        Task<List<MongoDeveloper>> GetDevelopersInProject(string projectId);
        Task<MongoDeveloper> GetDeveloperById(string developerId, string projectId);
        Task<string> GetWiki(string projectId);
        Task<string> DownloadProject(string projectId);
        Task<List<MongoToDo>> GetAllToDo(string projectId);
        Task<List<MongoInProgress>> GetAllInProgress(string projectId);
        Task<List<MongoDone>> GetAllDone(string projectId);

        //create
        Task<bool> CreateProject(CreateProjectViewModel model);
        Task<bool> CreateFramework(string projectId);
        Task<bool> AddDeveloperToProject(string projectId, MongoDeveloper developer);
        Task<bool> AddSuperVisorToProject(string projectId, MongoSuperVisor superVisor);
        Task<bool> DesignLogo(string projectId, string logoPath);
        Task<bool> UploadProject(string projectId, string projectPath);
        //update
        Task<bool> UpdateProject(string projectId, MongoProject updatedProject);
        Task<bool> AssignToDo(string projectId, ToDoViewModel model);
        Task<bool> AssignDone(string projectId, DoneViewModel model);
        Task<bool> AssignInProgress(string projectId, InProgressViewModel model);
        Task<bool> Evalute(string projectId, MongoEvalution evalution);
        Task<bool> MarkProject(string projectId, double mark);

        Task<bool> WriteWiki(string projectId, string wiki);
        //remove
        Task<bool> RemoveProject(string projectId);
        Task<bool> RemoveProject(MongoProject project);

        Task<bool> RemoveDeveloperFromProject(string projectId, string developerId);
        Task<bool> RemoveToDOFromProject(string projectId, string todoId);
        Task<bool> RemoveInProgressFromProject(string projectId, string inprogressId);
        Task<bool> RemoveDoneFromProject(string projectId, string doneId);


    }
}
