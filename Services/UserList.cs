using System;
using System.Collections.Generic;

namespace VsLiveSanDiego.Services
{
    public class UserList
    {
        public UserList()
        {
            CurrentUsernames = new List<string>();
        }

        public List<string> CurrentUsernames { get; }
    }
}
