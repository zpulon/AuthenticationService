using AuthenticationService.Model;
using AuthenticationService.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthenticationService.Manager
{
    public class AdminManager
    {
        public readonly IAdminStores _iadminStores;
        public AdminManager(IAdminStores adminStores)
        {
            _iadminStores = adminStores;
        }
        public Task<OS_Admin> Get_AdminById(long id)
        {
            return _iadminStores.QueryAdmin().Where(z => z.Id == id).Select(z=>new OS_Admin {Id=z.Id,Name=z.Name  } ).FirstOrDefaultAsync();
        }
        public Task<OS_Admin> Get_AdminByPredicate(Expression<Func<OS_Admin, bool>> predicate)
        {
            return _iadminStores.QueryAdmin().Where(predicate).FirstOrDefaultAsync();
        }
    }
}
