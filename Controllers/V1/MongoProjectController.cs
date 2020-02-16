using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Contracts.V1.Requests;
using Project.Api.Domain;
using Project.Api.Domain.MongoDomains;
using Project.Api.Services;
using Project.Api.V1.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Project.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoProjectController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMongoProjectService _mongoProjectService;

        public MongoProjectController(IHostingEnvironment HostingEnvironment,
                                        IMongoProjectService mongoProjectService)
        {

            _hostingEnvironment = HostingEnvironment;
            _mongoProjectService = mongoProjectService;
        }

        #region Get


        [HttpGet(ApiRoutes.Project.GetAllProjects)]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _mongoProjectService.GetAllProjects();
            if (projects != null)
            {
                return Ok(new { incomingProjects = projects });
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        }

        [HttpGet(ApiRoutes.Project.GetProjectById)]
        public async Task<IActionResult> GetProjectById(string projectId)
        {
            var project = await _mongoProjectService.GetProjectById(projectId);
            if (project != null)
            {
                return Ok(new { incomingProject = project });
            }
            return NotFound(new { status = 0, message = "Not Found" });
        }
        [HttpGet(ApiRoutes.Project.GetDeveloperById)]
        public async Task<IActionResult> GetDeveloperById([FromRoute] string projectId, [FromRoute]string developerId)
        {
            var developer = await _mongoProjectService.GetDeveloperById(developerId, projectId);
            if (developer != null)
            {
                return Ok(new { incomingDeveloper = developer });
            }
            return NotFound(new { status = 0, message = "Not Found" });
        }

        [HttpGet(ApiRoutes.Project.Search)]
        public async Task<IActionResult> Search(string projectName)
        {
            var project = await _mongoProjectService.GetProjectByName(projectName);
            if (project != null)
            {
                return Ok(new { incomingProjects = project });
            }
            return NotFound(new { status = 0, message = "Not Found" });
        }
        [HttpGet(ApiRoutes.Project.GetDevelopersInProject)]
        public async Task<IActionResult> GetDevelopersInProject(string projectId)
        {
            if (projectId != null)
            {
                var developers = await _mongoProjectService.GetDevelopersInProject(projectId);
                if (developers.Count > 0)
                {
                    return Ok(new { developersInProject = developers });
                }
                return NoContent();

            }
            return BadRequest(new { status = 0, message = "Invalid request" });
        }

        [HttpGet(ApiRoutes.Project.GetAllDone)]
        public async Task<IActionResult> GetAllDones(string projectId)
        {
            var dones = await _mongoProjectService.GetAllDone(projectId);
            if (dones != null)
            {
                return Ok(new { incomingProjects = dones });
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        }

        [HttpGet(ApiRoutes.Project.GetAllInprogress)]
        public async Task<IActionResult> GetAllInprogress(string projectId)
        {
            var inprogress = await _mongoProjectService.GetAllInProgress(projectId);
            if (inprogress != null)
            {
                return Ok(new { incomingProjects = inprogress });
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        }

        [HttpGet(ApiRoutes.Project.GetAllTodo)]
        public async Task<IActionResult> GetAllTodo(string projectId)
        {
            var todos = await _mongoProjectService.GetAllToDo(projectId);
            if (todos != null)
            {
                return Ok(new { incomingProjects = todos });
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        }

        [HttpGet(ApiRoutes.Project.GetWiki)]
        public async Task<IActionResult> GetWiki([FromRoute] string projectId)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.GetWiki(projectId);
                if (result != null)
                {
                    return Ok(new { wiki = result });
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
                var result = await _mongoProjectService.GetFramework(projectId);
                if (result != null)
                {
                    return Ok(new { framework = result });
                }
                return BadRequest(new { status = 0, message = "Something wrong happenned" });
            }
            return BadRequest(new { status = 0, message = "Invalid request" });
        }
        [HttpGet(ApiRoutes.Project.DownloadProject)]
        public async Task<IActionResult> DownloadProject([FromRoute]string projectId)
        {
            if (projectId != null)
            {
                var result = await _mongoProjectService.DownloadProject(projectId);
                if (result != null)
                {
                    return Ok(new { filePath = result });
                }
                return NotFound(new { message = "there is no project by this id" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        #endregion

        #region Post
        [HttpPost(ApiRoutes.Project.CreateProject)]
        public async Task<IActionResult> Create([FromForm] CreateProjectViewModel model)
        {

            var result = await _mongoProjectService.CreateProject(model);
            if (result)
            {
                return Ok(new { status = 1, message = "Created successfully" });
            }
            return BadRequest(new { status = 0, message = "Not Created" });
        }
        [HttpPost(ApiRoutes.Project.CreateFramework)]
        public async Task<IActionResult> CreateFramework([FromRoute] string projectId)
        {
            if (projectId != null)
            {
                var result = await _mongoProjectService.CreateFramework(projectId);
                if (result)
                {
                    return Ok(new { status = 1, message = "Created" });
                }
                return BadRequest(new { status = 0, message = "something wrong" });
            }
            return NotFound(new { status = 0, message = "There is no project to add framework to it" });
        }
        #endregion

        #region Put
        [HttpPut(ApiRoutes.Project.AddDeveloperToProject)]
        public async Task<IActionResult> AddDeveloperToProject([FromRoute]string projectId, [FromForm]CreateDeveloperViewModel model)
        {
            if (ModelState.IsValid)
            {
                var developer = new MongoDeveloper
                {

                    Name = model.DeveloperName
                };
                var result = await _mongoProjectService.AddDeveloperToProject(projectId, developer);
                if (result)
                {
                    return Ok(new { status = 1, message = "Add successfully" });
                }
                return BadRequest(new { status = 0, message = "Not Added" });

            }
            return BadRequest(new { status = 0, message = "Not Valid" });
        }

        [HttpPut(ApiRoutes.Project.AddSuperVisorToProject)]
        public async Task<IActionResult> AddSuperVisorToProject([FromRoute]string projectId, [FromForm]CreateSuperVisorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var superVisor = new MongoSuperVisor
                {

                    Name = model.SuperVisorName
                };
                var result = await _mongoProjectService.AddSuperVisorToProject(projectId, superVisor);
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
            var updatedProject = new MongoProject
            {
                ProjectName = model.Name
            };
            var result = await _mongoProjectService.UpdateProject(projectId, updatedProject);
            if (result)
            {
                return Ok(new { status = 1, message = "Updated successfully" });
            }
            return BadRequest(new { status = 0, message = "Not Updated" });
        }
  
        [HttpPut(ApiRoutes.Project.Evalution)]
        public async Task<IActionResult> Evalution([FromRoute]string projectId, [FromForm]MongoEvalution evalution)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.Evalute(projectId, evalution);
                if (result)
                {
                    return Ok(new { status = 1, message = "evaluted successfully" });
                }
                return BadRequest(new { status = 0, message = "evaluted failed" });
            }
            return BadRequest(new { status = 0, message = "invalid request" });
        }



        [HttpPut(ApiRoutes.Project.MarkProject)]
        public async Task<IActionResult> MarkProject([FromRoute]string projectId, [FromForm] double mark)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.MarkProject(projectId, mark);
                if (result)
                {
                    return Ok(new { satus = 1, message = "Marked successfully" });
                }
                return BadRequest(new { status = 0, message = "Invalid request" });
            }
            return BadRequest(new { status = 0, message = "Invalid data enter between 55:100" });

        }
        [HttpPut(ApiRoutes.Project.WriteWiki)]
        public async Task<IActionResult> WriteWiki([FromRoute] string projectId, IFormFile wiki)
        {
            var wikiLocation = "Wikis";
            if (ModelState.IsValid)
            {
                var wikiPath = ProcessUploadedFile(wiki, wikiLocation);
                var result = await _mongoProjectService.WriteWiki(projectId, wikiPath);
                if (result)
                {
                    return Ok(new { satus = 1, message = "Writed successfully" });
                }
                return BadRequest(new { status = 0, message = "Something wrong happenned" });
            }
            return BadRequest(new { status = 0, message = "Invalid request" });

        }

        [HttpPut(ApiRoutes.Project.AssignToDo)]
        public async Task<IActionResult> AssignToDo([FromRoute]string projectId, [FromForm] ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.AssignToDo(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add todo" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        [HttpPut(ApiRoutes.Project.Inprogress)]
        public async Task<IActionResult> Inprogress([FromRoute]string projectId, [FromForm] InProgressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.AssignInProgress(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add inprogress" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }

        [HttpPut(ApiRoutes.Project.AssignDone)]
        public async Task<IActionResult> AssignDone([FromRoute]string projectId, [FromForm] DoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.AssignDone(projectId, model);
                if (result)
                {
                    return Ok(new { status = 1, message = "Success to add done" });
                }
                return BadRequest(new { status = 1, message = "Something wrong happen" });
            }
            return BadRequest(new { status = 0, message = "Bad request" });
        }
        [HttpPut(ApiRoutes.Project.DesignLogo)]
        public async Task<IActionResult> DesignLogo(string projectId, IFormFile file)
        {
            var logoLocation = "Logos";
            if (ModelState.IsValid)
            {
                var filePath = ProcessUploadedFile(file, logoLocation);
                var result = await _mongoProjectService.DesignLogo(projectId, filePath);
                if (result)
                {
                    return Ok(new { status = 1, message = "success" });
                }
            }
            return BadRequest(new { status = 0, message = "Bad request" });

        }
        [HttpPut(ApiRoutes.Project.UploadProject)]
        public async Task<IActionResult> UploadProject(string projectId, IFormFile file)
        {
            var projectLocation = "Projects";
            if (ModelState.IsValid)
            {
                var filePath = ProcessUploadedFile(file, projectLocation);
                var result = await _mongoProjectService.UploadProject(projectId, filePath);
                if (result)
                {
                    return Ok(new { status = 1, message = "success" });
                }
            }
            return BadRequest(new { status = 0, message = "Bad request" });

        }
        #endregion

        #region Delete
        [HttpDelete(ApiRoutes.Project.RemoveDeveloperFromProject)]
        public async Task<IActionResult> RemoveDeveloperFromProject([FromRoute]string projectId, [FromForm]string developerId)
        {
            if (ModelState.IsValid)
            {
                var result = await _mongoProjectService.RemoveDeveloperFromProject(projectId, developerId);
                if (result)
                {
                    return Ok(new { status = 1, message = "Removed successfully" });
                }
                return BadRequest(new { status = 0, message = "Not removed" });

            }
            return BadRequest(new { status = 0, message = "Not Valid" });
        }

        [HttpDelete(ApiRoutes.Project.RemoveProjectById)]
        public async Task<IActionResult> RemoveProjectById(string projectId)
        {
            var result = await _mongoProjectService.RemoveProject(projectId);
            try
            {
                if (result)
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
        [HttpDelete(ApiRoutes.Project.RemoveProject)]
        public async Task<IActionResult> RemoveProject([FromForm]MongoProject project)
        {
            var result = await _mongoProjectService.RemoveProject(project);
            try
            {
                if (result)
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

        #region upload
        private string ProcessUploadedFile(IFormFile model, string location)
        {
            string filePath = null;
            if (model != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"{location}");
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
