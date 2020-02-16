using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using System.Threading.Tasks;

namespace Project.Api.Services
{
    public interface IDeveloperService
    {
        Task<bool> DesignLogo(string projectId, string logoPath);
        Task<bool> UploadProject(string projectId, string projectPath);
        Task<string> DownloadProject(string projectId);
        Task<bool> AssignToDo(string projectId, ToDoViewModel model);
        Task<bool> AssignDone(string projectId, DoneViewModel model);
        Task<bool> AssignInProgress(string projectId, InProgressViewModel model);
        Task<Framework> GetFramework(string projectId);
        Task<bool> CreateFramework(string projectId);
    }
}
