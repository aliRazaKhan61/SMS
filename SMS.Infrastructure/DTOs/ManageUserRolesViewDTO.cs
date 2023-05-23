using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Infrastructure.DTOs
{
    public class ManageUserRolesViewDTO
    {
        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }

    public class UserRolesViewModel
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
