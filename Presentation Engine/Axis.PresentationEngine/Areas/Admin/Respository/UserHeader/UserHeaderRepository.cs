using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class UserHeaderRepository : IUserHeaderRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "userheader/";

        #endregion Class Variables

        #region Constructors

        public UserHeaderRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserHeaderRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

        #endregion
    }
}