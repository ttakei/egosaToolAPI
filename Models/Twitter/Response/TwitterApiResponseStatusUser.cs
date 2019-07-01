using Newtonsoft.Json;
using System;

namespace EgosaToolAPI.Models.Twitter.Response
{
    public class TwitterApiResponseStatusUser
    {
        [JsonProperty(PropertyName = "id_str")]
        public String idStr;

        [JsonProperty(PropertyName = "name")]
        public String name;
    }
}
