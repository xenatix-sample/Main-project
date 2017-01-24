using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Photo;
using Axis.PresentationEngine.Helpers.Model;
using Axis.PresentationEngine.Helpers.Translator.Photo;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.PresentationEngine.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoRepository : IPhotoRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "photo/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoRepository"/> class.
        /// </summary>
        public PhotoRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public PhotoRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public Response<PhotoViewModel> GetPhoto(long photoID)
        {
            var apiUrl = baseRoute + "getPhoto";
            var param = new NameValueCollection();

            param.Add("photoID", photoID.ToString());

            var response = communicationManager.Get<Response<PhotoModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoViewModel> AddPhoto(PhotoViewModel photo)
        {
            string apiUrl = baseRoute + "addPhoto";
            var response = communicationManager.Post<PhotoModel, Response<PhotoModel>>(photo.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        public Response<PhotoViewModel> UpdatePhoto(PhotoViewModel assessment)
        {
            string apiUrl = baseRoute + "updatePhoto";
            var response = communicationManager.Put<PhotoModel, Response<PhotoModel>>(assessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<PhotoViewModel> DeletePhoto(long photoID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "deletePhoto";
            var param = new NameValueCollection
            {
                {"photoID", photoID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            var response = communicationManager.Delete<Response<PhotoModel>>(param, apiUrl);
            return response.ToViewModel();
        }
    }
}