using Axis.Model.Account;
using Axis.PresentationEngine.Areas.Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Areas.Account.Repository
{
    public interface IAccountRepository
    {
        AuthenticationModel Authenticate(UserViewModel user);
        Response<NavigationModel> GetNavigationItems();
    }
}
