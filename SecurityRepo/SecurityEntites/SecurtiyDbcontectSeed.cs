using Microsoft.AspNetCore.Identity;
using Security.Core.SecurityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Repo.SecurityEntites
{ 
    //step4
    public class SecurtiyDbcontectSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()) {
                //create user 
                var user = new AppUser()
                {
                    DisplayName = "Marina magdy",
                    Email = "marinamagdywalliam@gmail.com",
                    UserName = "marina.magdy",
                    PhoneNumber = "01270906108"
                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }

        }
    }
}
