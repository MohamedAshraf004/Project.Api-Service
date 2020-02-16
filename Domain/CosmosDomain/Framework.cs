using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Project.Api.Domain
{
    public class Framework
    {
        public Framework()
        {
            Dones = new List<Done>() { };
            ToDos = new List<ToDo>() { };
            InProgress = new List<InProgress>() { };
        }
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }
        public List<Done> Dones { get; set; }

        public List<ToDo> ToDos { get; set; }

        public List<InProgress> InProgress { get; set; }

    }
}