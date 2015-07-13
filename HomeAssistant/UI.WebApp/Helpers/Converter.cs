using System.Collections.Generic;
using System.Linq;
using BL.Discussions;
using UI.WebApp.Models.Users;

namespace UI.WebApp.Helpers
{
    public class Converter
    {
        public object GetDataObject(Message msg)
        {
            return new
            {
                Id = msg.Id,
                Author = new UserInfo
                {
                    Id = msg.Author.Id,
                    Login = msg.Author.Login,
                    PermissionIds = new List<int>(msg.Author.Permissions.Select(p => p.Id))
                },
                Body = msg.Body,
                Date = msg.Date.ToString()
            };
        }
    }
}
