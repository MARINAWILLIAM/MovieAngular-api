using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Core.SecurityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Repo.Data.Identity
{
    //step two
    public class AppSecurityDbContext:IdentityDbContext<AppUser>
    {
        public AppSecurityDbContext(DbContextOptions<AppSecurityDbContext> options):base(options)
        {
            
        }

    }
}
