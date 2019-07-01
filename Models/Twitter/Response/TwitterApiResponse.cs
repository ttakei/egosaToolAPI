using Newtonsoft.Json;
using System.Collections.Generic;

namespace EgosaToolAPI.Models.Twitter.Response
{
    public class TwitterApiResponse
    {
        [JsonProperty(PropertyName = "statuses")]
        public List<TwitterApiResponseStatus> Statuses { get; set; }
    }
}
