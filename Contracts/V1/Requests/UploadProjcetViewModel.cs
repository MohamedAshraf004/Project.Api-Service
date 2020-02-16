using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Contracts.V1.Requests
{
    public class UploadProjcetViewModel
    {
        public IFormFile UploadedFile { get; set; }
    }
}
