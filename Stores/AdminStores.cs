using AuthenticationService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
    public class AdminStores : IAdminStores
    {
        protected ApplicationDbContext context { get; }



        public AdminStores(ApplicationDbContext applicationDbContext)
        {
            context = applicationDbContext;
            oS_Admins = applicationDbContext.OS_Admins;

        }

        public IQueryable<OS_Admin> oS_Admins { get; }

        public IQueryable<OS_Admin> QueryAdmin()
        {
            return oS_Admins.AsNoTracking();
        }
    }
}
