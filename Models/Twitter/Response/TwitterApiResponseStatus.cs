using Newtonsoft.Json;
using System;

namespace EgosaToolAPI.Models.Twitter.Response
{
    public class TwitterApiResponseStatus : IComparable
    {
        [JsonProperty(PropertyName = "created_at")]
        public String createdAt;

        [JsonProperty(PropertyName = "id_str")]
        public String idStr;

        [JsonProperty(PropertyName = "text")]
        public String text;

        [JsonProperty(PropertyName = "user")]
        public TwitterApiResponseStatusUser user;

        public int CompareTo(object obj)
        {
            TwitterApiResponseStatus i = obj as TwitterApiResponseStatus;
            return idStr.CompareTo(i.idStr);
        }
    }
}
