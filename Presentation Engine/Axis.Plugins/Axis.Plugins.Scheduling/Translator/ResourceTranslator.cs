using Axis.Helpers.Infrastructure;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.PresentationEngine.Helpers.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Plugins.Scheduling.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ResourceTranslator
    {
        private static List<dynamic> _dayOfWeekLookup = null;
        public static List<dynamic> GetDayOfWeekLookup()
        {
            if (_dayOfWeekLookup == null)
            {
                var dowlookup = EngineContext.Current.Resolve<ILookupRepository>().GetLookupsByType(LookupType.DayOfWeek);
                _dayOfWeekLookup = dowlookup.DataItems[0][LookupType.DayOfWeek.ToString()];
            }
            return _dayOfWeekLookup;
        }

        private static List<dynamic> _scheduleTypeLookup = null;
        public static List<dynamic> GetScheduleTypeLookup()
        {
            if (_scheduleTypeLookup == null)
            {
                var dowlookup = EngineContext.Current.Resolve<ILookupRepository>().GetLookupsByType(LookupType.ScheduleType);
                _scheduleTypeLookup = dowlookup.DataItems[0][LookupType.ScheduleType.ToString()];
            }
            return _scheduleTypeLookup;
        }

        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static RoomViewModel ToViewModel(this RoomModel entity)
        {
            if (entity == null)
                return null;

            var model = new RoomViewModel
            {
                FacilityID = entity.FacilityID,
                RoomCapacity = entity.RoomCapacity,
                RoomID = entity.RoomID,
                RoomName = entity.RoomName,
                ModifiedOn = entity.ModifiedOn,
                IsSchedulable = entity.IsSchedulable
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<RoomViewModel> ToViewModel(this Response<RoomModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<RoomViewModel>();
            var roomList = new List<RoomViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(RoomModel room)
            {
                var transformModel = room.ToViewModel();
                roomList.Add(transformModel);
            });
            model.DataItems = roomList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static RoomModel ToModel(this RoomViewModel model)
        {
            if (model == null)
                return null;
            var entity = new RoomModel
            {
                FacilityID = model.FacilityID,
                RoomCapacity = model.RoomCapacity,
                RoomID = model.RoomID,
                RoomName = model.RoomName,
                ModifiedOn = model.ModifiedOn,
                IsSchedulable = model.IsSchedulable
            };
            return entity;
        }

        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentCredentialViewModel ToViewModel(this AppointmentCredentialModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentCredentialViewModel
            {
                AppointmentTypeID = entity.AppointmentTypeID,
                CredentialAbbreviation = entity.CredentialAbbreviation,
                CredentialCode = entity.CredentialCode,
                CredentialID = entity.CredentialID,
                CredentialName = entity.CredentialName,
                ModifiedOn = entity.ModifiedOn
            };

            entity.Providers.ForEach(delegate(ProviderModel provider)
            {
                model.Providers.Add(provider.ToViewModel());
            });

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentCredentialViewModel> ToViewModel(this Response<AppointmentCredentialModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentCredentialViewModel>();
            var appointmentCredentialList = new List<AppointmentCredentialViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentCredentialModel appointmentCredential)
            {
                var transformModel = appointmentCredential.ToViewModel();
                appointmentCredentialList.Add(transformModel);
            });
            model.DataItems = appointmentCredentialList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentCredentialModel ToModel(this AppointmentCredentialViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentCredentialModel
            {
                AppointmentTypeID = model.AppointmentTypeID,
                CredentialAbbreviation = model.CredentialAbbreviation,
                CredentialCode = model.CredentialCode,
                CredentialID = model.CredentialID,
                CredentialName = model.CredentialName,
                ModifiedOn = model.ModifiedOn
            };

            model.Providers.ForEach(delegate(ProviderViewModel provider)
            {
                entity.Providers.Add(provider.ToModel());
            });

            return entity;
        }

        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ProviderViewModel ToViewModel(this ProviderModel entity)
        {
            if (entity == null)
                return null;

            var model = new ProviderViewModel
            {
                CredentialId = entity.CredentialId,
                ProviderId = entity.ProviderId,
                ProviderName = entity.ProviderName,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ProviderViewModel> ToViewModel(this Response<ProviderModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ProviderViewModel>();
            var providerList = new List<ProviderViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(ProviderModel provider)
            {
                var transformModel = provider.ToViewModel();
                providerList.Add(transformModel);
            });
            model.DataItems = providerList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ProviderModel ToModel(this ProviderViewModel model)
        {
            if (model == null)
                return null;
            var entity = new ProviderModel
            {
                CredentialId = model.CredentialId,
                ProviderId = model.ProviderId,
                ProviderName = model.ProviderName,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ResourceViewModel ToViewModel(this ResourceModel entity)
        {
            if (entity == null)
                return null;

            var model = new ResourceViewModel
            {
                ResourceID = entity.ResourceID,
                ResourceName = entity.ResourceName,
                ResourceTypeID = entity.ResourceTypeID,
                FacilityID=entity.FacilityID,
                ModifiedOn = entity.ModifiedOn
            };

            entity.ResourceAvailabilities.ForEach(delegate(ResourceAvailabilityModel resourceAvailability)
            {
                model.ResourceAvailabilities.Add(resourceAvailability.ToViewModel());
            });

            entity.ResourceOverrides.ForEach(delegate(ResourceOverridesModel resourceOverride)
            {
                model.ResourceOverrides.Add(resourceOverride.ToViewModel());
            });

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ResourceViewModel> ToViewModel(this Response<ResourceModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ResourceViewModel>();
            var resourceList = new List<ResourceViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(ResourceModel resource)
            {
                var transformModel = resource.ToViewModel();
                resourceList.Add(transformModel);
            });
            model.DataItems = resourceList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ResourceModel ToModel(this ResourceViewModel model)
        {
            if (model == null)
                return null;
            var entity = new ResourceModel
            {
                ResourceID = model.ResourceID,
                ResourceName = model.ResourceName,
                ResourceTypeID = model.ResourceTypeID,
                FacilityID=model.FacilityID,
                ModifiedOn = model.ModifiedOn
            };

            model.ResourceAvailabilities.ForEach(delegate(ResourceAvailabilityViewModel resourceAvailability)
            {
                entity.ResourceAvailabilities.Add(resourceAvailability.ToModel());
            });

            model.ResourceOverrides.ForEach(delegate(ResourceOverridesViewModel resourceOverride)
            {
                entity.ResourceOverrides.Add(resourceOverride.ToModel());
            });

            return entity;
        }

        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ResourceAvailabilityViewModel ToViewModel(this ResourceAvailabilityModel entity)
        {
            if (entity == null)
                return null;

            var model = new ResourceAvailabilityViewModel
            {
                AvailabilityEndTime = entity.AvailabilityEndTime,
                AvailabilityStartTime = entity.AvailabilityStartTime,
                Days = (string)GetDayOfWeekLookup().First(x => x.ID == entity.DayOfWeekID).Name,
                FacilityID = entity.FacilityID,
                ResourceAvailabilityID = entity.ResourceAvailabilityID,
                ResourceID = entity.ResourceID,
                ResourceTypeID = entity.ResourceTypeID,
                ModifiedOn = entity.ModifiedOn
            };

            var schedtype = GetScheduleTypeLookup().FirstOrDefault(x => x.ID == entity.ScheduleTypeID);
            model.ScheduleType = (schedtype == null) ? "" : (string)schedtype.ScheduleType;

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ResourceAvailabilityViewModel> ToViewModel(this Response<ResourceAvailabilityModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ResourceAvailabilityViewModel>();
            var resourceAvailabilityList = new List<ResourceAvailabilityViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(ResourceAvailabilityModel resourceAvailability)
            {
                var transformModel = resourceAvailability.ToViewModel();
                resourceAvailabilityList.Add(transformModel);
            });
            model.DataItems = resourceAvailabilityList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ResourceAvailabilityModel ToModel(this ResourceAvailabilityViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ResourceAvailabilityModel
            {
                AvailabilityEndTime = model.AvailabilityEndTime,
                AvailabilityStartTime = model.AvailabilityStartTime,
                DayOfWeekID = (int)GetDayOfWeekLookup().First(x => x.Name == model.Days).ID,
                ScheduleTypeID = (short)GetScheduleTypeLookup().First(x => x.ScheduleType == model.ScheduleType).ID,
                FacilityID = model.FacilityID,
                ResourceAvailabilityID = model.ResourceAvailabilityID,
                ResourceID = model.ResourceID,
                ResourceTypeID = model.ResourceTypeID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

                /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ResourceOverridesViewModel ToViewModel(this ResourceOverridesModel entity)
        {
            if (entity == null)
                return null;

            var model = new ResourceOverridesViewModel
            {
                ResourceOverrideID = entity.ResourceOverrideID,
                ResourceID = entity.ResourceID,
                ResourceTypeID = entity.ResourceTypeID,
                OverrideDate = entity.OverrideDate,
                FacilityID = entity.FacilityID,
                Comments = entity.Comments,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ResourceOverridesViewModel> ToViewModel(this Response<ResourceOverridesModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ResourceOverridesViewModel>();
            var resourceOverridesList = new List<ResourceOverridesViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(ResourceOverridesModel resourceOverrides)
            {
                var transformModel = resourceOverrides.ToViewModel();
                resourceOverridesList.Add(transformModel);
            });
            model.DataItems = resourceOverridesList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ResourceOverridesModel ToModel(this ResourceOverridesViewModel model)
        {
            if (model == null)
                return null;
            var entity = new ResourceOverridesModel
            {
                ResourceOverrideID = model.ResourceOverrideID,
                ResourceID = model.ResourceID,
                ResourceTypeID = model.ResourceTypeID,
                OverrideDate = model.OverrideDate,
                FacilityID = model.FacilityID,
                Comments = model.Comments,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}