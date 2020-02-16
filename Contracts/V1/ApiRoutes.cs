namespace Project.Api.V1.Contracts
{

    public class ApiRoutes
    {
        public const string Root = "api/";

        public const string Version = "v1";

        public const string Base = Root + Version + "/project/";
        public static class Project
        {
            //project services routes
            public const string GetAllProjects = Base + "GetAllProjects";
            public const string GetProjectById = Base + "GetProjectById/{projectId}";
            public const string GetDeveloperById = Base + "GetDeveloperById/{projectId}/{developerId}";
            public const string Search = Base + "Search/{projectName}";
            public const string CreateProject = Base + "CreateProject";
            public const string UpdateProject = Base + "UpdateProject/{projectId}";
            public const string RemoveProject = Base + "RemoveProject/";
            public const string RemoveProjectById = Base + "RemoveProjectById/{projectId}";
            public const string AddDeveloperToProject = Base + "AddDeveloperToProject/{projectId}";
            public const string AddSuperVisorToProject = Base + "AddSuperVisorToProject/{projectId}";
            public const string RemoveDeveloperFromProject = Base + "RemoveDeveloperFromProject/{projectId}";

            //supervisor services routes
            public const string Evalution = Base + "Evalution/{projectId}";
            public const string GetDevelopersInProject = Base + "GetDevelopersInProject/{projectId}";
            public const string MarkProject = Base + "MarkProject/{projectId}";
            public const string WriteWiki = Base + "WriteWiki/{projectId}";
            public const string GetWiki = Base + "GetWiki/{projectId}";
            public const string GetFramework = Base + "GetFramework/{projectId}";

            //developer services routes
            public const string DownloadProject = Base + "DownloadProject/{projectId}";
            public const string CreateFramework = Base + "CreateFramework/{projectId}";
            public const string AssignToDo = Base + "AssignToDo/{projectId}";
            public const string AssignDone = Base + "AssignDone/{projectId}";
            public const string Inprogress = Base + "Inprogress/{projectId}";
            public const string GetAllDone = Base + "GetAllDone/{projectId}";
            public const string GetAllTodo = Base + "GetAllTodo/{projectId}";
            public const string GetAllInprogress = Base + "GetAllInprogress/{projectId}";
            public const string DesignLogo = Base + "DesignLogo/{projectId}";
            public const string UploadProject = Base + "UploadProject/{projectId}";


        }

    }
}
