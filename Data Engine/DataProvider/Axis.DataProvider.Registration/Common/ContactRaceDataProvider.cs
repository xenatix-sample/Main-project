using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Registration.Common.IContactRaceDataProvider" />
    public class ContactRaceDataProvider : IContactRaceDataProvider
    {
        #region initializations

        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactRaceDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region Public Methods

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRaceModel> GetContactRace(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            var repository = unitOfWork.GetRepository<ContactRaceModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactRaces", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRaceModel> AddContactRace(List<ContactRaceModel> contactRace)
        {
            var repository = unitOfWork.GetRepository<ContactRaceModel>(SchemaName.Registration);

            var spParameters = BuildContactRaceSpParams(contactRace.FirstOrDefault(), false);
            return unitOfWork.EnsureInTransaction<Response<ContactRaceModel>>(repository.ExecuteNQStoredProc, "usp_AddContactRace", spParameters, forceRollback: contactRace.Any(x => x.ForceRollback.GetValueOrDefault(false)), idResult: true);
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRaceModel> UpdateContactRace(List<ContactRaceModel> contactRace)
        {
            var repository = unitOfWork.GetRepository<ContactRaceModel>(SchemaName.Registration);

            var spParameters = BuildContactRaceSpParams(contactRace.FirstOrDefault(), true);
            return unitOfWork.EnsureInTransaction<Response<ContactRaceModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactRace", spParameters, forceRollback: contactRace.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRaceModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("contactRaceID", contactRaceID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = unitOfWork.GetRepository<ContactRaceModel>(SchemaName.Registration);
            Response<ContactRaceModel> spResults = new Response<ContactRaceModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactRace", procsParameters);
            return spResults;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds the contact Race sp parameters.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildContactRaceSpParams(ContactRaceModel contactRace, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactRaceID", contactRace.ContactRaceID));

            spParameters.Add(new SqlParameter("ContactID", contactRace.ContactID));
            spParameters.Add(new SqlParameter("RaceID", (object)contactRace.RaceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", (object)contactRace.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
