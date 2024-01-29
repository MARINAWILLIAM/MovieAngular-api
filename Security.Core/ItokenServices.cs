using Microsoft.AspNetCore.Identity;
using Security.Core.SecurityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core
{
    public interface ItokenServices
    {
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser>userManager);
    }
}
