using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.BusinessAdmin.ServiceDetails
{

    public class ServiceDetailsDataProvider : IServiceDetailsDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The organization structure data provider
        /// </summary>
        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider;
        #endregion Class Variables



        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="organizationStructureDataProvider">The organization structure data provider.</param>
        public ServiceDetailsDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
        }

        #endregion Constructors

        #region Public Methods



        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsDataModel> GetServiceWorkflows(int servicesID)
        {
            var serviceRepository = _unitOfWork.GetRepository<ServiceDetailsDataModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", servicesID) };
            return serviceRepository.ExecuteStoredProc("usp_GetServiceWorkflowsByServicesID", procParams);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var retModel = new Response<ServiceDetailsModel>();
                if (serviceDetails.ServicesModuleComponentID == 0)
                {
                    var uow = _unitOfWork.GetRepository<ServiceDetailsModel>(SchemaName.Reference);
                    var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", serviceDetails.ServicesID), new SqlParameter("ModuleComponentID", serviceDetails.ModuleComponentID), new SqlParameter("ModifedOn", DateTime.Now) };
                    retModel = uow.ExecuteNQStoredProc("usp_AddServicesModuleComponent", procParams, idResult: true);
                    if (retModel.ResultCode != 0)
                    {
                        return retModel;
                    }
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.DeliveryMethod);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.ServiceLocation);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.Recipient);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.ServiceStatus);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.AttendanceStatus);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.TrackingField);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }

                retModel = saveService(serviceDetails, ServiceDetailParameters.Credentials);
                if (retModel.ResultCode != 0)
                {
                    return retModel;
                }
                _unitOfWork.TransactionScopeComplete(transactionScope);
                return retModel;
            }
        }

        public Response<ServiceDetailsModel> GetServiceDetails(int servicesID, long moduleComponentID)
        {
            var serviceDetailsResponse = new Response<ServiceDetailsModel>();
            var modelItem = new ServiceDetailsModel
            {
                ServicesID = servicesID,
                ModuleComponentID = moduleComponentID
            };

            var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", servicesID), new SqlParameter("ModuleComponentID", moduleComponentID) };
            modelItem.DeliveryMethods = GetDetails<DeliveryMethodModel>("usp_GetDeliveryMethodsByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }

            modelItem.ServiceLocation = GetDetails<ServiceLocationModel>("usp_GetServiceLocationsByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }

            modelItem.Recipients = GetDetails<RecipientCodeModel>("usp_GetRecipientsByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }

            modelItem.ServiceStatus = GetDetails<ServiceStatusModel>("usp_GetServiceStatusByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }

            modelItem.AttendanceStatus = GetDetails<AttendanceStatusModel>("usp_GetAttendanceStatusByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }

            modelItem.TrackingField = GetDetails<TrackingFieldModel>("usp_GetTrackingFieldsByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }



            modelItem.Credentials = GetDetails<CredentialModel>("usp_GetCredentialsByServicesModuleComponentID", serviceDetailsResponse, servicesID, moduleComponentID);
            if (serviceDetailsResponse.ResultCode != 0)
            {
                return serviceDetailsResponse;
            }
            serviceDetailsResponse.DataItems = new List<ServiceDetailsModel>();
            serviceDetailsResponse.DataItems.Add(modelItem);
            return serviceDetailsResponse;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="baseModel">The base model.</param>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        private List<T> GetDetails<T>(string spName, Response<ServiceDetailsModel> baseModel, int servicesID, long moduleComponentID) where T : class
        {
            var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", servicesID), new SqlParameter("ModuleComponentID", moduleComponentID) };
            var credentialsRepository = _unitOfWork.GetRepository<T>(SchemaName.Reference);
            var model = credentialsRepository.ExecuteStoredProc(spName, procParams);
            baseModel.ResultCode = model.ResultCode;
            baseModel.ResultMessage = model.ResultMessage;
            return model.DataItems;
        }

        /// <summary>
        /// Saves the service.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <param name="serviceOption">The service option.</param>
        /// <returns></returns>
        private Response<ServiceDetailsModel> saveService(ServiceDetailsModel serviceDetails, ServiceDetailParameters serviceOption)
        {
            List<SqlParameter> _paramList = new List<SqlParameter>();
            var procName = " ";
            var uow = _unitOfWork.GetRepository<ServiceDetailsModel>(SchemaName.Reference);

            _paramList.Add(new SqlParameter("ServicedID", serviceDetails.ServicesID));
            _paramList.Add(new SqlParameter("ModuleComponentID", serviceDetails.ModuleComponentID));


            switch (serviceOption)
            {
                case ServiceDetailParameters.DeliveryMethod:
                    _paramList.Add(new SqlParameter("DeliveryMethodXMLValue", ToXML<DeliveryMethodModel>("DeliveryMethodXMLValue", "DeliveryMethod", serviceDetails.DeliveryMethods, "DeliveryMethodID")));
                    procName = "usp_SaveDeliveryMethodModuleComponent";
                    break;
                case ServiceDetailParameters.ServiceLocation:
                    _paramList.Add(new SqlParameter("ServiceLocationXMLValue", ToXML<ServiceLocationModel>("ServiceLocationXMLValue", "ServiceLocation", serviceDetails.ServiceLocation, "ServiceLocationID")));
                    procName = "usp_SaveServiceLocationModuleComponent";
                    break;
                case ServiceDetailParameters.Recipient:
                    _paramList.Add(new SqlParameter("RecipientCodeXMLValue", ToXML<RecipientCodeModel>("RecipientCodeXMLValue", "RecipientCode", serviceDetails.Recipients, "CodeID")));
                    procName = "usp_SaveRecipientCodeModuleComponent";
                    break;
                case ServiceDetailParameters.ServiceStatus:
                    _paramList.Add(new SqlParameter("ServiceStatusXMLValue", ToXML<ServiceStatusModel>("ServiceStatusXMLValue", "ServiceStatus", serviceDetails.ServiceStatus, "ServiceStatusID")));
                    procName = "usp_SaveServiceStatusModuleComponent";
                    break;
                case ServiceDetailParameters.AttendanceStatus:
                    _paramList.Add(new SqlParameter("AttendanceStatusXMLValue", ToXML<AttendanceStatusModel>("AttendanceStatusXMLValue", "AttendanceStatus", serviceDetails.AttendanceStatus, "AttendanceStatusID")));
                    procName = "usp_SaveAttendanceStatusModuleComponent";
                    break;
                case ServiceDetailParameters.TrackingField:
                    _paramList.Add(new SqlParameter("TrackingFieldXMLValue", ToXML<TrackingFieldModel>("TrackingFieldXMLValue", "TrackingField", serviceDetails.TrackingField, "TrackingFieldID")));
                    procName = "usp_SaveTrackingFieldModuleComponent";
                    break;
                case ServiceDetailParameters.Credentials:
                    _paramList.Add(new SqlParameter("CredentialXMLValue", ToXML<CredentialModel>("CredentialXMLValue", "Credential", serviceDetails.Credentials, "CredentialID")));
                    procName = "usp_SaveCredentialModuleComponent";
                    break;
                default:
                    break;
            }

            _paramList.Add(new SqlParameter("ModifiedOn", DateTime.Now));
            return uow.ExecuteNQStoredProc(procName, _paramList);
        }


        /// <summary>
        /// Toxmls the specified root element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootElement">The root element.</param>
        /// <param name="element">The element.</param>
        /// <param name="model">The model.</param>
        /// <param name="keyID">The key identifier.</param>
        /// <returns></returns>
        private string ToXML<T>(string rootElement, string element, List<T> model, string keyID)
        {
            var xml = new XElement(rootElement,
               from item in model
               where item != null
               select new XElement(element,
                              new XElement(keyID, item.GetType().GetProperty(keyID).GetValue(item))
                          ));

            return xml.ToString();
        }

        #endregion
    }

    /// <summary>
    /// ENUM ServiceDetailParameters 
    /// </summary>
    enum ServiceDetailParameters
    {
        /// <summary>
        /// The delivery method
        /// </summary>
        DeliveryMethod = 1,
        /// <summary>
        /// The service location
        /// </summary>
        ServiceLocation = 2,
        /// <summary>
        /// The recipient
        /// </summary>
        Recipient = 3,
        /// <summary>
        /// The service status
        /// </summary>
        ServiceStatus = 4,
        /// <summary>
        /// The attendance status
        /// </summary>
        AttendanceStatus = 5,
        /// <summary>
        /// The tracking field
        /// </summary>
        TrackingField = 6,
        /// <summary>
        /// The credentials
        /// </summary>
        Credentials = 7,
    }
}



