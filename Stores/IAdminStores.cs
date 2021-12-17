using AuthenticationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Stores
{
   public interface IAdminStores
    {
        IQueryable<OS_Admin> QueryAdmin();
        /// <summary>
        /// 
        /// </summary>
        IQueryable<OS_Admin>  oS_Admins { get; }
    }
}
