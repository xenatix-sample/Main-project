using System.Linq;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System.Collections.Generic;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public class ReferralClientInformationDataProvider : IReferralClientInformationDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;
        /// <summary>
        /// The address data provider
        /// </summary>
        private IReferralClientAdditionalDetailsDataProvider clientAdditionalDetailsDataProvider;
        private IReferralClientDemographicsDataProvider clientDemographicsDataProvider;
        private IReferralClientConcernDataProvider clientConcernDataProvider;
        private IContactAddressDataProvider addressDataProvider;
        private IContactPhoneDataProvider phoneDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralClientInformationDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralClientInformationDataProvider(IUnitOfWork unitOfWork,
            IReferralClientAdditionalDetailsDataProvider _clientAdditionalDetailsDataProvider,
            IReferralClientDemographicsDataProvider _clientDemographicsDataProvider,
            IReferralClientConcernDataProvider _clientConcernDataProvider,
            IContactAddressDataProvider _addressDataProvider,
            IContactPhoneDataProvider _phoneDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.clientAdditionalDetailsDataProvider = _clientAdditionalDetailsDataProvider;
            this.clientDemographicsDataProvider = _clientDemographicsDataProvider;
            this.clientConcernDataProvider = _clientConcernDataProvider;
            this.addressDataProvider = _addressDataProvider;
            this.phoneDataProvider = _phoneDataProvider;

        }

        #endregion initializations

        #region exposed functionality

        public Response<ReferralClientInformationModel> GetClientInformation(long ReferralHeaderID)
        {
            var clientInformationResult = new Response<ReferralClientInformationModel>();

            var clientAdditionalDetails = clientAdditionalDetailsDataProvider.GetClientAdditionalDetail(ReferralHeaderID);
            if (clientAdditionalDetails.ResultCode != 0)
            {
                clientInformationResult.ResultCode = clientAdditionalDetails.ResultCode;
                clientInformationResult.ResultMessage = clientInformationResult.ResultMessage;
                return clientInformationResult;
            }

            if (clientAdditionalDetails != null && clientAdditionalDetails.DataItems.Count > 0)
            {
                var additionalDetail = clientAdditionalDetails.DataItems.FirstOrDefault();
                var clientDemographics = clientDemographicsDataProvider.GetClientDemographics(additionalDetail.ContactID);
                if (clientDemographics.ResultCode != 0)
                {
                    clientInformationResult.ResultCode = clientDemographics.ResultCode;
                    clientInformationResult.ResultMessage = clientDemographics.ResultMessage;
                    return clientInformationResult;
                }

                var clientConcern = clientConcernDataProvider.GetClientConcern(additionalDetail.ReferralAdditionalDetailID);
                if (clientConcern.ResultCode != 0)
                {
                    clientInformationResult.ResultCode = clientConcern.ResultCode;
                    clientInformationResult.ResultMessage = clientConcern.ResultMessage;
                    return clientInformationResult;
                }

                var clientAddresses = addressDataProvider.GetAddresses(additionalDetail.ContactID, 5);
                if (clientAddresses.ResultCode != 0)
                {
                    clientInformationResult.ResultCode = clientAddresses.ResultCode;
                    clientInformationResult.ResultMessage = clientAddresses.ResultMessage;
                    return clientInformationResult;
                }

                var clientPhones = phoneDataProvider.GetPhones(additionalDetail.ContactID, 5);
                if (clientPhones.ResultCode != 0)
                {
                    clientInformationResult.ResultCode = clientPhones.ResultCode;
                    clientInformationResult.ResultMessage = clientPhones.ResultMessage;
                    return clientInformationResult;
                }

                clientInformationResult.DataItems = new List<ReferralClientInformationModel>{
                    new ReferralClientInformationModel
                    {
                      referralClientAdditionalDetails = additionalDetail,
                      clientDemographicsModel = clientDemographics.DataItems.FirstOrDefault(),
                      Concern = clientConcern.DataItems.FirstOrDefault(),
                      Addresses = clientAddresses.DataItems,
                      Phones = clientPhones.DataItems
                    }
                };
            }

            return clientInformationResult;
        }

        public Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel clientInformation)
        {
            Response<ReferralClientInformationModel> clientResults = new Response<ReferralClientInformationModel>();
            clientResults.ResultCode = 0;
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                Response<ContactDemographicsModel> demographicsResult =
                        clientDemographicsDataProvider.AddClientDemographics(clientInformation.clientDemographicsModel);
                if (demographicsResult.ResultCode != 0)
                {
                    clientResults.ResultCode = demographicsResult.ResultCode;
                    clientResults.ResultMessage = demographicsResult.ResultMessage;
                    return clientResults;
                }

                clientResults.ID = demographicsResult.ID;
                clientInformation.referralClientAdditionalDetails.ContactID = demographicsResult.ID;
                clientInformation.referralClientAdditionalDetails.ReferralHeaderID = clientInformation.ReferralHeaderID;
                
                Response<ReferralClientAdditionalDetailsModel> additionalDetailsResult = 
                          clientAdditionalDetailsDataProvider.AddClientAdditionalDetail(clientInformation.referralClientAdditionalDetails);
                if(additionalDetailsResult.ResultCode != 0)
                {
                    clientResults.ResultCode = additionalDetailsResult.ResultCode;
                    clientResults.ResultMessage = additionalDetailsResult.ResultMessage;
                    return clientResults;
                }

                if (clientInformation.Concern != null)
                {
                    clientInformation.Concern.ReferralAdditionalDetailID = additionalDetailsResult.ID;

                    Response<ReferralClientConcernsModel> concernsResult =
                             clientConcernDataProvider.AddUpdateClientConcern(clientInformation.Concern);
                    if (concernsResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = concernsResult.ResultCode;
                        clientResults.ResultMessage = concernsResult.ResultMessage;
                        return clientResults;
                    }
                }

                if (clientInformation.Addresses != null)
                {
                    Response<ContactAddressModel> addressResult =
                             addressDataProvider.AddAddresses(demographicsResult.ID, clientInformation.Addresses);
                    if (addressResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = addressResult.ResultCode;
                        clientResults.ResultMessage = addressResult.ResultMessage;
                        return clientResults;
                    }
                }

                if (clientInformation.Phones != null)
                {
                    Response<ContactPhoneModel> phoneResult =
                             phoneDataProvider.AddPhones(demographicsResult.ID, clientInformation.Phones);
                    if (phoneResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = phoneResult.ResultCode;
                        clientResults.ResultMessage = phoneResult.ResultMessage;
                        return clientResults;
                    }
                }

                unitOfWork.TransactionScopeComplete(transactionScope);
            }
            return clientResults;
        }
      
        public Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel clientInformation)
        {
            Response<ReferralClientInformationModel> clientResults = new Response<ReferralClientInformationModel>();
            clientResults.ResultCode = 0;

            var contactID = clientInformation.clientDemographicsModel.ContactID;
            if(contactID <= 0)
            {
                clientResults.ResultCode = -1;
                return clientResults;
            }

            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                Response<ContactDemographicsModel> demographicsResult =
                        clientDemographicsDataProvider.UpdateClientDemographics(clientInformation.clientDemographicsModel);
                if (demographicsResult.ResultCode != 0)
                {
                    clientResults.ResultCode = demographicsResult.ResultCode;
                    clientResults.ResultMessage = demographicsResult.ResultMessage;
                    return clientResults;
                }

                Response<ReferralClientAdditionalDetailsModel> additionalDetailsResult =
                          clientAdditionalDetailsDataProvider.UpdateClientAdditionalDetail(clientInformation.referralClientAdditionalDetails);
                if (additionalDetailsResult.ResultCode != 0)
                {
                    clientResults.ResultCode = additionalDetailsResult.ResultCode;
                    clientResults.ResultMessage = additionalDetailsResult.ResultMessage;
                    return clientResults;
                }

                if (clientInformation.Concern != null)
                {
                    clientInformation.Concern.ReferralAdditionalDetailID = clientInformation.referralClientAdditionalDetails.ReferralAdditionalDetailID;
                    Response<ReferralClientConcernsModel> concernsResult =
                            clientConcernDataProvider.AddUpdateClientConcern(clientInformation.Concern);
                    if (concernsResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = concernsResult.ResultCode;
                        clientResults.ResultMessage = concernsResult.ResultMessage;
                        return clientResults;
                    }
                }

                if (clientInformation.Addresses != null)
                {
                    Response<ContactAddressModel> addressResult =
                            addressDataProvider.UpdateAddresses(contactID, clientInformation.Addresses);
                    if (addressResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = addressResult.ResultCode;
                        clientResults.ResultMessage = addressResult.ResultMessage;
                        return clientResults;
                    }
                }

                if (clientInformation.Phones != null)
                {
                    Response<ContactPhoneModel> phoneResult =
                            phoneDataProvider.UpdatePhones(contactID, clientInformation.Phones);
                    if (phoneResult.ResultCode != 0)
                    {
                        clientResults.ResultCode = phoneResult.ResultCode;
                        clientResults.ResultMessage = phoneResult.ResultMessage;
                        return clientResults;
                    }
                }

                unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return clientResults;
        }

        #endregion exposed functionality
    }
}
