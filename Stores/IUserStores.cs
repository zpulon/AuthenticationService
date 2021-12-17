using AuthenticationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
   public interface IUserStores
    {
        IQueryable<OS_User> QueryUser();
        /// <summary>
        /// 
        /// </summary>
        IQueryable<OS_User>  oS_Users { get; }
    }
}
