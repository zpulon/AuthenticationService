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
    public class UserManager
    {
        private readonly IUserStores _iuserStores;
         public UserManager(IUserStores userStores)
        {
            _iuserStores = userStores;
        }

        public Task<OS_User> Get_UserById(long id)
        {
          return _iuserStores.QueryUser().Where(z => z.Id == id).Select(z=>new OS_User { Id=z.Id,Name=z.Name,SchoolName=z.SchoolName,GraduationYear=z.GraduationYear,StudentNumber=z.StudentNumber,ClassName=z.ClassName} ).FirstOrDefaultAsync();
        }
        public Task<OS_User> Get_UserByPredicate(Expression<Func<OS_User, bool>> predicate)
        {
            return _iuserStores.QueryUser().Where(predicate).FirstOrDefaultAsync();
        }
    }
}
