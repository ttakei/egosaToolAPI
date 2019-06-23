using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgosaToolAPI.Models.Twitter.Response
{
    public class TwitterApiResponseStatus : IComparable
    {
        public String created_at;
        public String id_str;
        public String text;
        public TwitterApiResponseStatusUser user;

        public int CompareTo(object obj)
        {
            TwitterApiResponseStatus i = obj as TwitterApiResponseStatus;
            return this.id_str.CompareTo(i.id_str);
        }
    }
}
