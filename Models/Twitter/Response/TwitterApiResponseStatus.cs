using Newtonsoft.Json;
using System;

namespace EgosaToolAPI.Models.Twitter.Response
{
    public class TwitterApiResponseStatus
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "user")]
        public TwitterApiResponseStatusUser User { get; set; }
    }
}
