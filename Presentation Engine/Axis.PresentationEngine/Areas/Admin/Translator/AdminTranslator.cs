using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Admin.Model;
using Axis.Model.Admin;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class AdminTranslator
    {
        public static UserAdditionalDetailsModel ToModel(this UserAdditionalDetailsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserAdditionalDetailsModel
            {
                UserDetails = new List<UserAdditionalDetailsBaseModel>()
            };
            if (entity.UserDetails != null)
            {
                entity.UserDetails.ForEach(delegate(UserAdditionalDetailsViewBaseModel sig)
                {
                    var transformedModel = sig.ToModel();
                    model.UserDetails.Add(transformedModel);
                });
            }
            return model;
        }

        public static UserAdditionalDetailsBaseModel ToModel(this UserAdditionalDetailsViewBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserAdditionalDetailsBaseModel
            {
                UserAdditionalDetailID = entity.UserAdditionalDetailID,
                UserID = entity.UserID,
                ContractingEntity = entity.ContractingEntity,
                IDNumber = entity.IDNumber,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static UserAdditionalDetailsViewModel ToModel(this UserAdditionalDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserAdditionalDetailsViewModel
            {
                 UserDetails = new List<UserAdditionalDetailsViewBaseModel>()
            };
            if (entity.UserDetails != null)
            {
                entity.UserDetails.ForEach(delegate(UserAdditionalDetailsBaseModel sig)
                {
                    var transformedModel = sig.ToModel();
                    model.UserDetails.Add(transformedModel);
                });
            }
            return model;
        }

        public static UserAdditionalDetailsViewBaseModel ToModel(this UserAdditionalDetailsBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserAdditionalDetailsViewBaseModel
            {
                UserAdditionalDetailID = entity.UserAdditionalDetailID,
                UserID = entity.UserID,
                ContractingEntity = entity.ContractingEntity,
                IDNumber = entity.IDNumber,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<UserAdditionalDetailsViewModel> ToModel(this Response<UserAdditionalDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserAdditionalDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserAdditionalDetailsModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<UserAdditionalDetailsViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }


        public static Response<UserAdditionalDetailsViewBaseModel> ToModel(this Response<UserAdditionalDetailsBaseModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserAdditionalDetailsViewBaseModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserAdditionalDetailsBaseModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<UserAdditionalDetailsViewBaseModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public static UserIdentifierDetailsModel ToModel(this UserIdentifierViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserIdentifierDetailsModel
            {
                UserDetails = new List<UserIdentifierDetailsBaseModel>()
            };
            if (entity.UserDetails != null)
            {
                entity.UserDetails.ForEach(delegate(UserIdentifierViewBaseModel sig)
                {
                    var transformedModel = sig.ToModel();
                    model.UserDetails.Add(transformedModel);
                });
            }

            return model;
        }

        public static UserIdentifierDetailsBaseModel ToModel(this UserIdentifierViewBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserIdentifierDetailsBaseModel
            {
                UserIdentifierDetailsID = entity.UserIdentifierDetailsID,
                UserID = entity.UserID,
                UserIdentifierTypeID = entity.UserIdentifierTypeID,
                IDNumber = entity.IDNumber,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static UserIdentifierViewModel ToModel(this UserIdentifierDetailsModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserIdentifierViewModel
            {
                UserDetails = new List<UserIdentifierViewBaseModel>()
            };

            if (entity.UserDetails != null)
            {
                entity.UserDetails.ForEach(delegate(UserIdentifierDetailsBaseModel details)
                {
                    var transformedModel = details.ToModel();
                    model.UserDetails.Add(transformedModel);
                });
            }

            return model;
        }

        public static UserIdentifierViewBaseModel ToModel(this UserIdentifierDetailsBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserIdentifierViewBaseModel
            {
                UserIdentifierDetailsID = entity.UserIdentifierDetailsID,
                UserID = entity.UserID,
                UserIdentifierTypeID = entity.UserIdentifierTypeID,
                IDNumber = entity.IDNumber,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<UserIdentifierViewModel> ToModel(this Response<UserIdentifierDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserIdentifierViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserIdentifierDetailsModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<UserIdentifierViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }

        public static Response<UserIdentifierViewBaseModel> ToModel(this Response<UserIdentifierDetailsBaseModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserIdentifierViewBaseModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserIdentifierDetailsBaseModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<UserIdentifierViewBaseModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public static CoSignaturesModel ToModel(this CoSignaturesViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new CoSignaturesModel
            {
                 CoSignatures = new List<CoSignaturesBaseModel>()
            };
            if (entity.CoSignatures != null)
            {
                entity.CoSignatures.ForEach(delegate(CoSignaturesViewBaseModel sig)
                {
                    var transformedModel = sig.ToModel();
                    model.CoSignatures.Add(transformedModel);
                });
            }

            return model;
        }

        public static CoSignaturesBaseModel ToModel(this CoSignaturesViewBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new CoSignaturesBaseModel
            {
                CoSignatureID = entity.CoSignatureID,
                CoSigneeID = entity.CoSigneeID,
                UserID = entity.UserID,
                DocumentTypeGroupID = entity.DocumentTypeGroupID,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static CoSignaturesViewModel ToModel(this CoSignaturesModel entity)
        {
            if (entity == null)
                return null;

            var model = new CoSignaturesViewModel
            {
                 CoSignatures = new List<CoSignaturesViewBaseModel>()
            };
            if (entity.CoSignatures != null)
            {
                entity.CoSignatures.ForEach(delegate(CoSignaturesBaseModel sig)
                {
                    var transformedModel = sig.ToModel();
                    model.CoSignatures.Add(transformedModel);
                });
            }
            return model;
        }

        public static CoSignaturesViewBaseModel ToModel(this CoSignaturesBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new CoSignaturesViewBaseModel
            {
                CoSignatureID = entity.CoSignatureID,
                CoSigneeID = entity.CoSigneeID,
                UserID = entity.UserID,
                DocumentTypeGroupID = entity.DocumentTypeGroupID,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<CoSignaturesViewModel> ToModel(this Response<CoSignaturesModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<CoSignaturesViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(CoSignaturesModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<CoSignaturesViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }

        public static Response<CoSignaturesViewBaseModel> ToModel(this Response<CoSignaturesBaseModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<CoSignaturesViewBaseModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(CoSignaturesBaseModel model)
                {
                    var transformedModel = model.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var viewmodel = new Response<CoSignaturesViewBaseModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return viewmodel;
        }

        public static UserCredentialModel ToModel(this UserCredentialViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserCredentialModel
            {
                UserCredentialID = entity.UserCredentialID,
                UserID = entity.UserID,
                CredentialID = entity.CredentialID,
                CredentialName = entity.Name,
                Description = entity.Description,
                LicenseRequired = entity.LicenseRequired,
                LicenseNbr = entity.LicenseNbr,
                LicenseIssueDate = entity.LicenseIssueDate,
                LicenseExpirationDate = entity.LicenseExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static UserCredentialViewModel ToModel(this UserCredentialModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserCredentialViewModel
            {
                UserCredentialID = entity.UserCredentialID,
                UserID = entity.UserID,
                CredentialID = entity.CredentialID,
                Name = entity.CredentialName,
                Description = entity.Description,
                LicenseRequired = entity.LicenseRequired,
                LicenseNbr = entity.LicenseNbr,
                LicenseIssueDate = entity.LicenseIssueDate,
                LicenseExpirationDate = entity.LicenseExpirationDate,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<UserCredentialViewModel> ToModel(this Response<UserCredentialModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserCredentialViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserCredentialModel userCredentialModel)
                {
                    var transformedModel = userCredentialModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<UserCredentialViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static List<UserCredentialModel> ToModel(this List<UserCredentialViewModel> entity)
        {
            if (entity == null)
                return null;

            var models = new List<UserCredentialModel>();
            entity.ForEach(x =>
                models.Add(new UserCredentialModel
                {
                    UserCredentialID = x.UserCredentialID,
                    UserID = x.UserID,
                    CredentialID = x.CredentialID,
                    CredentialName = x.Name,
                    Description = x.Description,
                    StateIssuedByID = x.StateIssuedByID,
                    LicenseRequired = x.LicenseRequired,
                    LicenseNbr = x.LicenseNbr,
                    LicenseIssueDate = x.LicenseIssueDate,
                    LicenseExpirationDate = x.LicenseExpirationDate,
                    ModifiedOn = x.ModifiedOn
                }));

            return models;
        }

        public static List<UserCredentialViewModel> ToModel(this List<UserCredentialModel> entity)
        {
            if (entity == null)
                return null;

            var models = new List<UserCredentialViewModel>();
            entity.ForEach(x =>
                models.Add(new UserCredentialViewModel
                {
                    UserCredentialID = x.UserCredentialID,
                    UserID = x.UserID,
                    CredentialID = x.CredentialID,
                    Name = x.CredentialName,
                    Description = x.Description,
                    StateIssuedByID = x.StateIssuedByID,
                    LicenseRequired = x.LicenseRequired,
                    LicenseNbr = x.LicenseNbr,
                    LicenseIssueDate = x.LicenseIssueDate,
                    LicenseExpirationDate = x.LicenseExpirationDate,
                    ModifiedOn = x.ModifiedOn
                }));

            return models;
        }
    }
}