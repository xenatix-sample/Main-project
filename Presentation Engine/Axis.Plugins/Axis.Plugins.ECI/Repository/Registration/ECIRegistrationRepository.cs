using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;

namespace Axis.Plugins.ECI.Repository.Registration
{
    public class ECIRegistrationRepository : IECIRegistrationRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "Registration/";

        #endregion

        #region Constructors

        public ECIRegistrationRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
