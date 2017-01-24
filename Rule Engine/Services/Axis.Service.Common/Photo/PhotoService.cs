using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Photo;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Common.Photo
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoService : IPhotoService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "photo/";

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoService"/> class.
        /// </summary>
        public PhotoService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the photo.
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public Response<PhotoModel> GetPhoto(long photoID)
        {
            var apiUrl = baseRoute + "GetPhoto";
            var param = new NameValueCollection();
            param.Add("photoID", photoID.ToString());

            return communicationManager.Get<Response<PhotoModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> AddPhoto(PhotoModel photo)
        {
            var apiUrl = baseRoute + "AddPhoto";
            return communicationManager.Post<PhotoModel, Response<PhotoModel>>(photo, apiUrl);
        }

        /// <summary>
        /// Updates the photo.
        /// </summary>
        /// <param name="photo">The photo.</param>
        /// <returns></returns>
        public Response<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            var apiUrl = baseRoute + "UpdatePhoto";
            return communicationManager.Put<PhotoModel, Response<PhotoModel>>(photo, apiUrl);
        }

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="photoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<PhotoModel> DeletePhoto(long photoID, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "DeletePhoto";
            var param = new NameValueCollection
            {
                {"photoID", photoID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return communicationManager.Delete<Response<PhotoModel>>(param, apiUrl);
        }
    }
}