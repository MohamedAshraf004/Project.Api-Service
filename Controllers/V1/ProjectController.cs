using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using Project.Api.Services;
using Project.Api.V1.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Project.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ISuperVisorService _superVisor;
        private readonly IDeveloperService _developerService;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProjectController(IProjectService projectService, ISuperVisorService superVisor
                            , IDeveloperService developerService
                            , IHostingEnvironment HostingEnvironment)
        {
            _projectService = projectService;
            _superVisor = superVisor;
            _developerService = developerService;
            hostingEnvironment = HostingEnvironment;
        }

        //
        // Project Services was implemented
        //
        #region ProjectServices
        [HttpPost(ApiRoutes.Project.CreateProject)]
        public async Task<IActionResult> Create([FromForm] CreateProjectViewModel model)
        {
            var result = await _projectService.CreateProject(model);
            if (result)
            {
                return Ok(new { status = 1, message = "Created successfully" });
            }
            return BadRequest(new { status = 0, message = "Not Created" });
        }

        [HttpPut(ApiRoutes.Project.AddDeveloperToProject)]
        public async Task<IActionResult> AddDeveloperToProject([FromRoute]string projectId, [FromForm]Developer developer)
        {
            if (ModelState.IsValid)
            {
                var result = await _projectService.AddDeveloperToProject(projectId, developer);
                if (result)
                {
                    return Ok(new { status = 1, message = "Add successfully" });
                }
                return BadRequest(new { status = 0, message = "Not Added" });

            }
            return BadRequest(new { status = 0, message = "Not Valid" });
        }

        [HttpPut(ApiRoutes.Project.UpdateProject)]
        public async Task<IActionResult> UpdateProject([FromRoute]string projectId, [FromForm] CreateProjectViewModel model)
        {
            var updatedProject = new CosmosProjectDto
            {
                ProjectName = model.Name
            };
            var result = await _projectService.UpdateProject(projectId, updatedProject);
            if (result)
            {
                return Ok(new { status = 1, message = "Updated successfully" });
            }
            return BadRequest(new { status = 0, message = "Not Updated" });
        }
        ///route if we want to change
        [HttpDelete(ApiRoutes.Project.RemoveDeveloperFromProject)]
        public async Task<IActionResult> RemoveDeveloperFromProject([FromRoute]string projectId, [FromForm]string developerId)
        {
            if (ModelState.IsValid)
            {
                var result = await _projectService.RemoveDeveloperFromProject(projectId, developerId);
                if (result)
                {
                    return Ok(new { status = 1, message = "Removed successfully" });
                }
                return BadRequest(new { status = 0, message = "Not removed" });

            }
            return BadRequest(new { status = 0, message = "Not Valid" });
        }
        [HttpGet(ApiRoutes.Project.GetAllProjects)]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            if (projects != null)
            {
                return Ok(new { status = 1, incomingProjects = projects });
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        }

        [HttpGet(ApiRoutes.Project.GetProjectById)]
        public async Task<IActionResult> GetProjectById(string projectId)
        {
            var project = await _projectService.GetProject(projectId);
            if (project != null)
            {
                return Ok(new { status = 1, incomingProject = project });
            }
            return NotFound(new { status = 0, message = "Not Found" });
        }
        [HttpGet(ApiRoutes.Project.Search)]
        public async Task<IActionResult> Search(string projectName)
        {
            var project = await _projectService.Search(projectName);
            if (project != null)
            {
                return Ok(new { status = 1, incomingProjects = project });
            }
            return NotFound(new { status = 0, message = "Not Found" });
        }
        [HttpDelete(ApiRoutes.Project.RemoveProject)]
        public async Task<IActionResult> RemoveProject(string projectId)
        {
            var project = await _projectService.RemoveProject(projectId);
            try
            {
                if (project)
                {
                    return Ok(new { status = 1, message = "Project was deleted successfully" });
                }
                return NotFound(new { status = 0, message = "Not Found this item to delete" });
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        #endregion
        /////////////
        //supervisor Services
        /////////////
        #region SupervisorServices
        [HttpPost(ApiRoutes.Project.Evalution)]
        public async Task<IActionResult> Evalution([FromRoute]string projectId, [FromForm]Evalution evalution)
        {
            if (ModelState.IsValid)
            {
                var result = await _superVisor.Evalute(projectId, evalution);
                if (result)
                {
                    return Ok(new { status = 1, message = "evaluted successfully" });
                }
                return BadRequest(new { status = 0, message = "evaluted failed" });
            }
            return BadRequest(new { status = 0, message = "invalid request" });
        }
        [HttpGet(ApiRoutes.Project.GetDevelopersInProject)]
        public async Task<IActionResult> GetDevelopersInProject(string projectId)
        {
            if (projectId != null)
            {
                var developers = await _superVisor.GetDevelopersInProject(projectId);
                if (developers.Count > 0)
                {
                    return Ok(new { status = 1, developersInProject = developers });
                }
                return NoContent();

            }
            return BadRequest(new { status = 0, message = "Invalid request" });
        }

        //mark not valid
        [HttpPost(ApiRoutes.Project.MarkProject)]
        public async Task<IActionResult> MarkProject([FromRoute]string projectId, [FromForm] double mark)
        {
            if (ModelState.IsValid)
            {
                var result = await _superVisor.MarkProject(projectId, mark);
                if (result)
                {
                    return Ok(new { satus = 1, message = "Marked successfully" });
                }
                return BadRequest(new { status = 0, message = "Invalid request" });
            }
            return BadRequest(new { status = 0, message = "Invalid data enter between 55:100" });

        }
        [HttpPost(ApiRoutes.Project.WriteWiki)]
        public async Task<IActionResult> WriteWiki([FromRoute] string projectId, IFormFile wiki)
        {
            if (ModelState.IsValid)
            {
                var wikiPath = ProcessUploadedFile(wiki);
                var result = await _superVisor.WriteWiki(projectId, wikiPath);
                if (result)
                {
                    return Ok(new { satus = 1, message = "Writed successfully" });
                }
                return BadRequest(new { status = 0, message = "Something wrong happenned" });
            }
            return BadRequest(new { status = 0, message = "Invalid request" });

        }

        //not route
        [HttpGet(ApiRoutes.Project.GetWiki)]
        public async Task<IActionResult> GetWiki([FromRoute] string projectId)
        {
            if (ModelState.IsValid)
            {
                var result = await _superVisor.GetWiki(projectId);
                if (result != null)
                {
                    return Ok(new { satus = 1, wiki = result });
                }
                return BadRequest(new { status = 0, message = "Something wrong happenned" });
            }
            return BadRequest(new { status = 0, message = "Invalid request" });

        }
        [HttpGet(ApiRoutes.Project.GetFramework)]
        public async Task<IActionResult> GetFramework([FromRoute] string projectId)
        {
            if (ModelState.IsValid)
            {
                var result = await _superVisor.GetFramework(projectId);
                if (result != null)
                {
                    return Ok(new { satus = 1, framework = result });
                }
                return BadRequest(new { status = 0, message = "Something wrong happenned" });
            }
            return BadRequest(new { status = 0, message = "Invalid request" });

        }
        #endregion 
        //
        //Developer Services
        //
        #region DeveloperServices
        //Developer Services
        [HttpGet(ApiRoutes.Project.DownloadProject)]
        public async Task<IActionResult> DownloadProject([FromRoute]string projectId)
        {
            if (projectId != null)
            {
                var result = await _developerService.DownloadProject(projectId);
                if (result != null)
                {
                    return Ok(new { status = 1, filePath = result });
                }
                return NotFound(new { status = 1, message = "there is no project by this id" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        [HttpPost(ApiRoutes.Project.CreateFramework)]
        public async Task<IActionResult> CreateFramework([FromRoute] string projectId)
        {
            if (projectId != null)
            {
                var result = await _developerService.CreateFramework(projectId);
                if (result)
                {
                    return Ok(new { status = 1, message = "Created" });
                }
                return BadRequest(new { status = 0, message = "something wrong" });
            }
            return NotFound(new { status = 0, message = "There is no project to add framework to it" });
        }

        [HttpPost(ApiRoutes.Project.AssignToDo)]
        public async Task<IActionResult> AssignToDo([FromRoute]string projectId, [FromForm] ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _developerService.AssignToDo(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add todo" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        [HttpPost(ApiRoutes.Project.Inprogress)]
        public async Task<IActionResult> Inprogress([FromRoute]string projectId, [FromForm] InProgressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _developerService.AssignInProgress(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add inprogress" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        [HttpPost(ApiRoutes.Project.AssignDone)]
        public async Task<IActionResult> AssignDone([FromRoute]string projectId, [FromForm] DoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _developerService.AssignDone(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add done" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }
        [HttpPost(ApiRoutes.Project.DesignLogo)]
        public async Task<IActionResult> DesignLogo(string projectId, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var filePath = ProcessUploadedFile(file);
                var result = await _developerService.DesignLogo(projectId, filePath);
                if (result)
                {
                    return Ok(new { status = 1, message = "success" });
                }
            }
            return BadRequest(new { status = 0, message = "Bad request" });

        }
        [HttpPost(ApiRoutes.Project.UploadProject)]
        public async Task<IActionResult> UploadProject(string projectId, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var filePath = ProcessUploadedFile(file);
                var result = await _developerService.UploadProject(projectId, filePath);
                if (result)
                {
                    return Ok(new { status = 1, message = "success" });
                }
            }
            return BadRequest(new { status = 0, message = "Bad request" });

        }


        private string ProcessUploadedFile(IFormFile model)
        {
            string filePath = null;
            if (model != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Wikis");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return filePath;
        }
        #endregion
    }
}
