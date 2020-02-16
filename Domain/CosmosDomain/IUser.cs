using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Project.Api.Domain
{
    public interface IUser
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        string Id { get; set; }
        string Name { get; set; }

    }
}
