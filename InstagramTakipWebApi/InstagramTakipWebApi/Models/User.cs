using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramTakipWebApi.Models
{
    public class User
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FollowPage { get; set; }
        public int FollowCount { get; set; }
    }
}
