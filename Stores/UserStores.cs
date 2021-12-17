using AuthenticationService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    public class UserStores : IUserStores
    {
        protected ApplicationDbContext context { get; }

        public IQueryable<OS_User> oS_Users { get; }

        public UserStores(ApplicationDbContext  applicationDbContext)
        {
            context = applicationDbContext;
            oS_Users = applicationDbContext.OS_Users;
           
        }
        public IQueryable<OS_User> QueryUser()
        {
            return oS_Users.AsNoTracking();
        }
    }
}
