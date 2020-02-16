using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Project.Api.Domain
{
    public class Developer:IUser
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }

    }
}