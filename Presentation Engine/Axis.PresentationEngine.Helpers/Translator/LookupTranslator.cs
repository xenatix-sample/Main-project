using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Helpers.Translator
{
    /// <summary>
    /// Lookup tranlator
    /// </summary>
    public static class LookupTranslator
    {
        #region Race

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static RaceModel ToModel(this RaceViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new RaceModel
            {
                RaceID = entity.RaceID,
                Race = entity.Race
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static RaceViewModel ToViewModel(this RaceModel model)
        {
            if (model == null)
                return null;

            var entity = new RaceViewModel
            {
                RaceID = model.RaceID,
                Race = model.Race
            };

            return entity;
        }

        #endregion Race

        #region Ethnicity

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static EthnicityModel ToModel(this EthnicityViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EthnicityModel
            {
                EthnicityID = entity.EthnicityID,
                Ethnicity = entity.Ethnicity
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static EthnicityViewModel ToViewModel(this EthnicityModel model)
        {
            if (model == null)
                return null;

            var entity = new EthnicityViewModel
            {
                EthnicityID = model.EthnicityID,
                Ethnicity = model.Ethnicity
            };

            return entity;
        }

        #endregion Ethnicity

        #region Vernacular

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static LanguageModel ToModel(this LanguageViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new LanguageModel
            {
                LanguageID = entity.LanguageID,
                LanguageName = entity.LanguageName
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static LanguageViewModel ToViewModel(this LanguageModel model)
        {
            if (model == null)
                return null;

            var entity = new LanguageViewModel
            {
                LanguageID = model.LanguageID,
                LanguageName = model.LanguageName
            };

            return entity;
        }

        #endregion Vernacular

        #region LegalStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static LegalStatusModel ToModel(this LegalStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new LegalStatusModel
            {
                LegalStatusID = entity.LegalStatusID,
                LegalStatus = entity.LegalStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static LegalStatusViewModel ToViewModel(this LegalStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new LegalStatusViewModel
            {
                LegalStatusID = model.LegalStatusID,
                LegalStatus = model.LegalStatus
            };

            return entity;
        }

        #endregion LegalStatus

        #region LivingArrangement

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static LivingArrangementModel ToModel(this LivingArrangementViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new LivingArrangementModel
            {
                LivingArrangementID = entity.LivingArrangementID,
                LivingArrangement = entity.LivingArrangement
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static LivingArrangementViewModel ToViewModel(this LivingArrangementModel model)
        {
            if (model == null)
                return null;

            var entity = new LivingArrangementViewModel
            {
                LivingArrangementID = model.LivingArrangementID,
                LivingArrangement = model.LivingArrangement
            };

            return entity;
        }

        #endregion LivingArrangement

        #region EducationStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static EducationStatusModel ToModel(this EducationStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EducationStatusModel
            {
                EducationStatusID = entity.EducationStatusID,
                EducationStatus = entity.EducationStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static EducationStatusViewModel ToViewModel(this EducationStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new EducationStatusViewModel
            {
                EducationStatusID = model.EducationStatusID,
                EducationStatus = model.EducationStatus
            };

            return entity;
        }

        #endregion EducationStatus

        #region Citizenship

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CitizenshipModel ToModel(this CitizenshipViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new CitizenshipModel
            {
                CitizenshipID = entity.CitizenshipID,
                Citizenship = entity.Citizenship
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CitizenshipViewModel ToViewModel(this CitizenshipModel model)
        {
            if (model == null)
                return null;

            var entity = new CitizenshipViewModel
            {
                CitizenshipID = model.CitizenshipID,
                Citizenship = model.Citizenship
            };

            return entity;
        }

        #endregion Citizenship

        #region MaritalStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static MaritalStatusModel ToModel(this MaritalStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new MaritalStatusModel
            {
                MaritalStatusID = entity.MaritalStatusID,
                MaritalStatus = entity.MaritalStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MaritalStatusViewModel ToViewModel(this MaritalStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new MaritalStatusViewModel
            {
                MaritalStatusID = model.MaritalStatusID,
                MaritalStatus = model.MaritalStatus
            };

            return entity;
        }

        #endregion MaritalStatus

        #region EmploymentStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static EmploymentStatusModel ToModel(this EmploymentStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EmploymentStatusModel
            {
                EmploymentStatusID = entity.EmploymentStatusID,
                EmploymentStatus = entity.EmploymentStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static EmploymentStatusViewModel ToViewModel(this EmploymentStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new EmploymentStatusViewModel
            {
                EmploymentStatusID = model.EmploymentStatusID,
                EmploymentStatus = model.EmploymentStatus
            };

            return entity;
        }

        #endregion EmploymentStatus

        #region Religion

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReligionModel ToModel(this ReligionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReligionModel
            {
                ReligionID = entity.ReligionID,
                Religion = entity.Religion
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReligionViewModel ToViewModel(this ReligionModel model)
        {
            if (model == null)
                return null;

            var entity = new ReligionViewModel
            {
                ReligionID = model.ReligionID,
                Religion = model.Religion
            };

            return entity;
        }

        #endregion Religion

        #region VeteranStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static VeteranStatusModel ToModel(this VeteranViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new VeteranStatusModel
            {
                VeteranStatusID = entity.VeteranStatusID,
                VeteranStatus = entity.VeteranStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static VeteranViewModel ToViewModel(this VeteranStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new VeteranViewModel
            {
                VeteranStatusID = model.VeteranStatusID,
                VeteranStatus = model.VeteranStatus
            };

            return entity;
        }

        #endregion VeteranStatus

        #region AddressType

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AddressTypeModel ToModel(this AddressTypeViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new AddressTypeModel
            {
                AddressTypeID = entity.AddressTypeID,
                AddressType = entity.AddressType
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AddressTypeViewModel ToViewModel(this AddressTypeModel model)
        {
            if (model == null)
                return null;

            var entity = new AddressTypeViewModel
            {
                AddressTypeID = model.AddressTypeID,
                AddressType = model.AddressType
            };

            return entity;
        }

        #endregion AddressType

        #region StateProvince

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static StateProvinceModel ToModel(this StateProvinceViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new StateProvinceModel
            {
                StateProvinceID = entity.StateProvinceID,
                StateProvinceName = entity.StateProvinceName
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static StateProvinceViewModel ToViewModel(this StateProvinceModel model)
        {
            if (model == null)
                return null;

            var entity = new StateProvinceViewModel
            {
                StateProvinceID = model.StateProvinceID,
                StateProvinceName = model.StateProvinceName
            };

            return entity;
        }

        #endregion StateProvince

        #region County

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CountyModel ToModel(this CountyViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new CountyModel
            {
                CountyID = entity.CountyID,
                StateProvinceID = entity.StateProvinceID,
                CountyName = entity.CountyName,
                OrganizationID = entity.OrganizationID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CountyViewModel ToViewModel(this CountyModel model)
        {
            if (model == null)
                return null;

            var entity = new CountyViewModel
            {
                CountyID = model.CountyID,
                StateProvinceID = model.StateProvinceID,
                CountyName = model.CountyName,
                OrganizationID = model.OrganizationID
            };

            return entity;
        }

        #endregion County

        #region MailPermissionType

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static MailPermissionModel ToModel(this MailPermissionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new MailPermissionModel
            {
                MailPermissionID = entity.MailPermissionID,
                MailPermission = entity.MailPermission
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MailPermissionViewModel ToViewModel(this MailPermissionModel model)
        {
            if (model == null)
                return null;

            var entity = new MailPermissionViewModel
            {
                MailPermissionID = model.MailPermissionID,
                MailPermission = model.MailPermission
            };

            return entity;
        }

        #endregion MailPermissionType

        #region DOBStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static DOBStatusModel ToModel(this DOBStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new DOBStatusModel
            {
                DOBStatusID = entity.DOBStatusID,
                DOBStatus = entity.DOBStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static DOBStatusViewModel ToViewModel(this DOBStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new DOBStatusViewModel
            {
                DOBStatusID = model.DOBStatusID,
                DOBStatus = model.DOBStatus
            };

            return entity;
        }

        #endregion DOBStatus

        #region Gender

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static GenderModel ToModel(this GenderViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new GenderModel
            {
                GenderID = entity.GenderID,
                Gender = entity.Gender
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static GenderViewModel ToViewModel(this GenderModel model)
        {
            if (model == null)
                return null;

            var entity = new GenderViewModel
            {
                GenderID = model.GenderID,
                Gender = model.Gender
            };

            return entity;
        }

        #endregion Gender

        #region ReferralSource

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralSourceModel ToModel(this ReferralSourceViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralSourceModel
            {
                ReferralSourceID = entity.ReferralSourceID,
                ReferralSource = entity.ReferralSource
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralSourceViewModel ToViewModel(this ReferralSourceModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralSourceViewModel
            {
                ReferralSourceID = model.ReferralSourceID,
                ReferralSource = model.ReferralSource
            };

            return entity;
        }

        #endregion ReferralSource

        #region SmokingStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SmokingStatusModel ToModel(this SmokingStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new SmokingStatusModel
            {
                SmokingStatusID = entity.SmokingStatusID,
                SmokingStatus = entity.SmokingStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SmokingStatusViewModel ToViewModel(this SmokingStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new SmokingStatusViewModel
            {
                SmokingStatusID = model.SmokingStatusID,
                SmokingStatus = model.SmokingStatus
            };

            return entity;
        }

        #endregion SmokingStatus

        #region PhonePermission

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PhonePermissionModel ToModel(this PhonePermissionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new PhonePermissionModel
            {
                PhonePermissionID = entity.PhonePermissionID,
                PhonePermission = entity.PhonePermission
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static PhonePermissionViewModel ToViewModel(this PhonePermissionModel model)
        {
            if (model == null)
                return null;

            var entity = new PhonePermissionViewModel
            {
                PhonePermissionID = model.PhonePermissionID,
                PhonePermission = model.PhonePermission
            };

            return entity;
        }

        #endregion PhonePermission

        #region ContactType

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactTypeModel ToModel(this ContactTypeViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactTypeModel
            {
                ContactTypeID = entity.ContactTypeID,
                ContactType = entity.ContactType
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactTypeViewModel ToViewModel(this ContactTypeModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactTypeViewModel
            {
                ContactTypeID = model.ContactTypeID,
                ContactType = model.ContactType
            };

            return entity;
        }

        #endregion ContactType

        #region ClientType

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ClientTypeModel ToModel(this ClientTypeViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ClientTypeModel
            {
                ClientTypeID = entity.ClientTypeID,
                ClientType = entity.ClientType
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ClientTypeViewModel ToViewModel(this ClientTypeModel model)
        {
            if (model == null)
                return null;

            var entity = new ClientTypeViewModel
            {
                ClientTypeID = model.ClientTypeID,
                ClientType = model.ClientType
            };

            return entity;
        }

        #endregion ClientType

        #region SSNStatus

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SSNStatusModel ToModel(this SSNStatusViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new SSNStatusModel
            {
                SSNStatusID = entity.SSNStatusID,
                SSNStatus = entity.SSNStatus
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SSNStatusViewModel ToViewModel(this SSNStatusModel model)
        {
            if (model == null)
                return null;

            var entity = new SSNStatusViewModel
            {
                SSNStatusID = model.SSNStatusID,
                SSNStatus = model.SSNStatus
            };

            return entity;
        }

        #endregion SSNStatus

        #region ContactMethod

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactMethodModel ToModel(this ContactMethodsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactMethodModel
            {
                ContactMethodID = entity.ContactMethodID,
                ContactMethod = entity.ContactMethod,
                IsSystem = entity.IsSystem
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactMethodsViewModel ToViewModel(this ContactMethodModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactMethodsViewModel
            {
                ContactMethodID = model.ContactMethodID,
                ContactMethod = model.ContactMethod,
                IsSystem=model.IsSystem
            };

            return entity;
        }

        #endregion ContactMethod

        #region Title

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static TitleModel ToModel(this TitleViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new TitleModel
            {
                TitleID = entity.TitleID,
                Title = entity.Title
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static TitleViewModel ToViewModel(this TitleModel model)
        {
            if (model == null)
                return null;

            var entity = new TitleViewModel
            {
                TitleID = model.TitleID,
                Title = model.Title
            };

            return entity;
        }

        #endregion Title

        #region Category

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static FinanceCategoryModel ToModel(this FinanceCategoryViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new FinanceCategoryModel
            {
                CategoryID = entity.CategoryID,
                Category = entity.Category
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static FinanceCategoryViewModel ToViewModel(this FinanceCategoryModel model)
        {
            if (model == null)
                return null;

            var entity = new FinanceCategoryViewModel
            {
                CategoryID = model.CategoryID,
                Category = model.Category
            };

            return entity;
        }

        #endregion Category

        #region Finance Frequency

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static FinanceFrequencyModel ToModel(this FinanceFrequencyViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new FinanceFrequencyModel
            {
                FinanceFrequencyID = entity.FinanceFrequencyID,
                FinanceFrequency = entity.FinanceFrequency
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static FinanceFrequencyViewModel ToViewModel(this FinanceFrequencyModel model)
        {
            if (model == null)
                return null;

            var entity = new FinanceFrequencyViewModel
            {
                FinanceFrequencyID = model.FinanceFrequencyID,
                FinanceFrequency = model.FinanceFrequency
            };

            return entity;
        }

        #endregion Finance Frequency

        #region Expiration

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ExpirationReasonModel ToModel(this ExpirationReasonViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ExpirationReasonModel
            {
                ExpirationReasonID = entity.ExpirationReasonID,
                ExpirationReason = entity.ExpirationReason
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ExpirationReasonViewModel ToViewModel(this ExpirationReasonModel model)
        {
            if (model == null)
                return null;

            var entity = new ExpirationReasonViewModel
            {
                ExpirationReasonID = model.ExpirationReasonID,
                ExpirationReason = model.ExpirationReason
            };

            return entity;
        }

        #endregion Expiration

        #region EmailPermissionViewModel

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static EmailPermissionModel ToModel(this EmailPermissionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EmailPermissionModel
            {
                EmailPermissionID = entity.EmailPermissionID,
                EmailPermission = entity.EmailPermission
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static EmailPermissionViewModel ToViewModel(this EmailPermissionModel model)
        {
            if (model == null)
                return null;

            var entity = new EmailPermissionViewModel
            {
                EmailPermissionID = model.EmailPermissionID,
                EmailPermission = model.EmailPermission
            };

            return entity;
        }

        #endregion EmailPermissionViewModel

        #region SecurityQuestion

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SecurityQuestionModel ToModel(this SecurityQuestionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new SecurityQuestionModel
            {
                SecurityQuestionID = entity.SecurityQuestionID,
                Question = entity.Question
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SecurityQuestionViewModel ToViewModel(this SecurityQuestionModel model)
        {
            if (model == null)
                return null;

            var entity = new SecurityQuestionViewModel
            {
                SecurityQuestionID = model.SecurityQuestionID,
                Question = model.Question
            };

            return entity;
        }

        #endregion SecurityQuestion

        #region Program

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ProgramModel ToModel(this ProgramViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ProgramModel
            {
                ProgramID = entity.ProgramID,
                ProgramName = entity.ProgramName
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ProgramViewModel ToViewModel(this ProgramModel model)
        {
            if (model == null)
                return null;

            var entity = new ProgramViewModel
            {
                ProgramID = model.ProgramID,
                ProgramName = model.ProgramName
            };

            return entity;
        }

        #endregion Program

        #region Facility

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static FacilityModel ToModel(this FacilityViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new FacilityModel
            {
                FacilityID = entity.FacilityID,
                FacilityName = entity.FacilityName
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static FacilityViewModel ToViewModel(this FacilityModel model)
        {
            if (model == null)
                return null;

            var entity = new FacilityViewModel
            {
                FacilityID = model.FacilityID,
                FacilityName = model.FacilityName
            };

            return entity;
        }

        #endregion Facility
    }
}