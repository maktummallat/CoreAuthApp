using CoreApp.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Data
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hosting;

        public DatabaseSeeder(ApplicationDbContext ctx,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hosting)
        {
            _ctx = ctx;
            _userManager = userManager;
            this._hosting = hosting;
        }

        public async Task<bool> Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.Users.Any())
            {
                await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "maktum.mallat@gmail.com",
                    Email = "maktum.mallat@gmail.com"
                });

                await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "maktum.mallat@outlook.com",
                    Email = "maktum.mallat@outlook.com"
                });

                var result = await _ctx.SaveChangesAsync(true);

                if (result == 0)
                    return false;
            }

            return true;
        }
    }
}
