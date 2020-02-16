using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Api.Domain
{
    public class CosmosProjectDto
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLogo { get; set; }
        public string ProjectPath { get; set; }
        public string Wiki { get; set; }

        [Range(55, 100)]
        public double Mark { get; set; }
        public Framework Framework { get; set; }
        public List<Developer> Developer { get; set; }
        public SuperVisor SuperVisior { get; set; }
        public Evalution Evalution { get; set; }
    }
}
