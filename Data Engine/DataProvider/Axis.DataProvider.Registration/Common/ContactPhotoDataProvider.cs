using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoDataProvider : IContactPhotoDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactPhotoDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhoto(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactPhoto", spParameters);

            return results;
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhotoById(long contactPhotoID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactPhotoID", contactPhotoID) };
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactPhotoById", spParameters);

            return results;
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhotoThumbnails(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactPhotoThumbnails", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> AddContactPhoto(ContactPhotoModel contactPhoto)
        {
            var spParameters = BuildContactPhotoSpParams(contactPhoto, false);
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_AddContactPhoto",
                    spParameters,
                    forceRollback: contactPhoto.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> UpdateContactPhoto(ContactPhotoModel contactPhoto)
        {
            var spParameters = BuildContactPhotoSpParams(contactPhoto, true);
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateContactPhoto", spParameters,
                          forceRollback: contactPhoto.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhotoModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<ContactPhotoModel>(SchemaName.Registration);
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactPhotoID", contactPhotoID), new SqlParameter("ModifiedOn", modifiedOn) };
            var result = repository.ExecuteNQStoredProc("usp_DeleteContactPhoto", spParameters);
            return result;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the contact photo sp parameters.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildContactPhotoSpParams(ContactPhotoModel contactPhoto, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactPhotoID", contactPhoto.ContactPhotoID));

            spParameters.Add(new SqlParameter("ContactID", (object)contactPhoto.ContactID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PhotoID", (object)contactPhoto.PhotoID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsPrimary", (object)contactPhoto.IsPrimary ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", contactPhoto.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
