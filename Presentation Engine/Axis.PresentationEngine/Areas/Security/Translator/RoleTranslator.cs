using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Security.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Security.Translator
{
    public static class RoleTranslator
    {

        #region RoleModel Permissions

        public static RoleViewModel ToModel(this RoleModel entity)
        {
            if (entity == null)
                return null;

            var model = new RoleViewModel
            {
                RoleID = entity.RoleID,
                Name = entity.Name,
                Description = entity.Description,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
                
            };

            return model;
        }

        public static RoleModel ToEntity(this RoleViewModel model)
        {
            if (model == null)
                return null;

            var entity = new RoleModel
            {
                RoleID = model.RoleID,
                Name = model.Name,
                Description = model.Description,
                EffectiveDate = model.EffectiveDate,
                ExpirationDate = model.ExpirationDate,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
                
            };

            return entity;
        }

        public static ModuleViewModel ToModel(this ModuleModel entity)
        {
            if (entity == null)
                return null;

            var Permissions = new List<PermissionViewModel>();

            if (entity.Permissions != null)
            {
                entity.Permissions.ForEach(delegate (PermissionModel permission)
                {
                    var transformedModel = permission.ToModel();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var model = new ModuleViewModel
            {
                ModuleID = entity.ModuleID,
                Name = entity.Name,
                Description = entity.Description,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ModuleModel ToEntity(this ModuleViewModel model)
        {
            if (model == null)
                return null;

            var Permissions = new List<PermissionModel>();

            if (model.Permissions != null)
            {
                model.Permissions.ForEach(delegate (PermissionViewModel permission)
                {
                    var transformedModel = permission.ToEntity();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var entity = new ModuleModel
            {
                ModuleID = model.ModuleID,
                Name = model.Name,
                Description = model.Description,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static RoleModuleViewModel ToModel(this RoleModuleModel entity)
        {
            if (entity == null)
                return null;

            var model = new RoleModuleViewModel
            {
                RoleModuleID = entity.RoleModuleID ?? 0,
                RoleId = entity.RoleId ?? 0,
                RoleName = entity.RoleName,
                ModuleId = entity.ModuleId,
                ModuleName = entity.ModuleName,
                Selected = entity.RoleModuleID==null? false:true,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn,
                Name = entity.Name,
                Description = entity.Description
            };

            return model;
        }

        public static RoleModuleModel ToEntity(this RoleModuleViewModel model)
        {
            if (model == null)
                return null;

            var entity = new RoleModuleModel
            {
                RoleModuleID = model.RoleModuleID,
                RoleId = model.RoleId,
                RoleName = model.RoleName,
                ModuleId = model.ModuleId,
                ModuleName = model.ModuleName,
                Selected = model.Selected,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static FeatureViewModel ToModel(this FeatureModel entity)
        {
            if (entity == null)
                return null;

            var Permissions = new List<PermissionViewModel>();

            if (entity.Permissions != null)
            {
                entity.Permissions.ForEach(delegate (PermissionModel permission)
                {
                    var transformedModel = permission.ToModel();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var model = new FeatureViewModel
            {
                FeatureID = entity.FeatureID,
                Name = entity.Name,
                Description = entity.Description,
                ModuleID = entity.ModuleID,
                ParentFeatureID = entity.ParentFeatureID,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static FeatureModel ToEntity(this FeatureViewModel model)
        {
            if (model == null)
                return null;
            var Permissions = new List<PermissionModel>();

            if (model.Permissions != null)
            {
                model.Permissions.ForEach(delegate (PermissionViewModel permission)
                {
                    var transformedModel = permission.ToEntity();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var entity = new FeatureModel
            {
                FeatureID = model.FeatureID,
                Name = model.Name,
                Description = model.Description,
                ModuleID = model.ModuleID,
                ParentFeatureID = model.ParentFeatureID,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static ModuleFeatureViewModel ToModel(this ModuleFeatureModel entity)
        {
            if (entity == null)
                return null;

            var Permissions = new List<PermissionViewModel>();

            if (entity.Permissions != null)
            {
                entity.Permissions.ForEach(delegate (PermissionModel permission)
                {
                    var transformedModel = permission.ToModel();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var model = new ModuleFeatureViewModel
            {
                ModuleFeatureID = entity.ModuleFeatureID,
                ModuleId = entity.ModuleId,
                ModuleName = entity.ModuleName,
                FeatureID = entity.FeatureID,
                FeatureName = entity.FeatureName,
                Description = entity.Description,
                ParentFeatureID = entity.ParentFeatureID,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static ModuleFeatureModel ToEntity(this ModuleFeatureViewModel model)
        {
            if (model == null)
                return null;

            var Permissions = new List<PermissionModel>();

            if (model.Permissions != null)
            {
                model.Permissions.ForEach(delegate (PermissionViewModel permission)
                {
                    var transformedModel = permission.ToEntity();
                    Permissions.Add(transformedModel);
                }
                    );
            }
            else
                Permissions = null;

            var entity = new ModuleFeatureModel
            {
                ModuleFeatureID = model.ModuleFeatureID,
                ModuleId = model.ModuleId,
                ModuleName = model.ModuleName,
                FeatureID = model.FeatureID,
                FeatureName = model.FeatureName,
                Description = model.Description,
                ParentFeatureID = model.ParentFeatureID,
                Permissions = Permissions,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static PermissionViewModel ToModel(this PermissionModel entity)
        {
            if (entity == null)
                return null;

            var model = new PermissionViewModel
            {
                PermissionID = entity.PermissionID,
                PermissionLevelID = entity.PermissionLevelID,
                PermissionLevelName = entity.PermissionLevelName,
                PermissionName = entity.PermissionName,
                Description = entity.Description,
                Code = entity.Code,
                Selected = entity.Selected,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static PermissionModel ToEntity(this PermissionViewModel model)
        {
            if (model == null)
                return null;

            var entity = new PermissionModel
            {
                PermissionID = model.PermissionID,
                PermissionLevelID = model.PermissionLevelID,
                PermissionLevelName = model.PermissionLevelName,
                PermissionName = model.PermissionName,
                Description = model.Description,
                Code = model.Code,
                Selected = model.Selected,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static UserPermissionViewModel ToModel(this UserPermissionModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserPermissionViewModel
            {
                RoleModuleComponentID = entity.RoleModuleComponentID,
                RoleModuleID = entity.RoleModuleID,
                PermissionID = entity.PermissionID,
                PermissionLevelID = entity.PermissionLevelID,
                PermissionLevelName = entity.PermissionLevelName,
                PermissionName = entity.PermissionName,
                Description = entity.Description,
                Code = entity.Code,
                ModifiedBy = 1,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static UserPermissionModel ToEntity(this UserPermissionViewModel model)
        {
            if (model == null)
                return null;

            var entity = new UserPermissionModel
            {
                PermissionID = model.PermissionID,
                PermissionLevelID = model.PermissionLevelID,
                PermissionLevelName = model.PermissionLevelName,
                PermissionName = model.PermissionName,
                Description = model.Description,
                Code = model.Code,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static RolePermissionsViewModel ToModel(this RolePermissionsModel entity)
        {
            if (entity == null)
                return null;

            var Role = new RoleViewModel();
            if (entity.Role != null)
            {
                var roleModel = new RoleModel();
                var roleView = roleModel.ToModel();
                Role = roleView;
            }
            else
                Role = null;

            var Modules = new List<ModuleViewModel>();

            if (entity.Modules != null)
            {
                entity.Modules.ForEach(delegate (ModuleModel modules)
                {
                    var transformedModel = modules.ToModel();
                    Modules.Add(transformedModel);
                }
                    );
            }
            else
                Modules = null;

            var Features = new List<FeatureViewModel>();

            if (entity.Features != null)
            {
                entity.Features.ForEach(delegate (FeatureModel features)
                {
                    var transformedModel = features.ToModel();
                    Features.Add(transformedModel);
                }
                    );
            }
            else
                Features = null;

            var model = new RolePermissionsViewModel
            {
                Role = Role,
                Modules = Modules,
                Features = Features
            };

            return model;
        }

        public static RolePermissionsModel ToEntity(this RolePermissionsViewModel model)
        {
            if (model == null)
                return null;

            var role = new RoleModel();
            if (model.Role != null)
            {
                var roleModel =
                role = model.Role.ToEntity();
            }

            var Modules = new List<ModuleModel>();

            if (model.Modules != null)
            {
                model.Modules.ForEach(delegate (ModuleViewModel modules)
                {
                    var transformedModel = modules.ToEntity();
                    Modules.Add(transformedModel);
                }
                    );
            }
            else
                Modules = null;

            var Features = new List<FeatureModel>();

            if (model.Features != null)
            {
                model.Features.ForEach(delegate (FeatureViewModel features)
                {
                    var transformedModel = features.ToEntity();
                    Features.Add(transformedModel);
                }
                    );
            }
            else
                Features = null;

            var entity = new RolePermissionsModel
            {
                Role = role,
                Modules = Modules,
                Features = Features
            };

            return entity;
        }

        public static RoleSecurityViewModel ToModel(this RoleSecurityModel entity)
        {
            if (entity == null)
                return null;

            var modulePermissions = new List<UserPermissionViewModel>();
            if (entity.ModulePermissions != null && entity.ModulePermissions.Count > 0)
            {
                entity.ModulePermissions.ForEach(delegate (UserPermissionModel permissionModel)
                {
                    modulePermissions.Add(permissionModel.ToModel());
                });
            }

            var componentPermissions = new List<UserPermissionViewModel>();
            if (entity.ComponentPermissions != null && entity.ComponentPermissions.Count > 0)
            {
                entity.ComponentPermissions.ForEach(delegate (UserPermissionModel permissionModel)
                {
                    componentPermissions.Add(permissionModel.ToModel());
                });
            }

            var model = new RoleSecurityViewModel
            {
                RoleID = entity.RoleID,
                RoleName = entity.RoleName,
                ModuleID = entity.ModuleID,
                ModuleName = entity.ModuleName,
                RoleModuleID = entity.RoleModuleID,
                DataKey = entity.DataKey,
                ComponentID = entity.ComponentID,
                ComponentName = entity.ComponentName,
                ModulePermissions = modulePermissions,
                ComponentPermissions = componentPermissions,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static RoleSecurityModel ToEntity(this RoleSecurityViewModel model)
        {
            if (model == null)
                return null;

            var modulePermissions = new List<UserPermissionModel>();
            if (model.ModulePermissions != null && model.ModulePermissions.Count > 0)
            {
                model.ModulePermissions.ForEach(delegate (UserPermissionViewModel permissionModel)
                {
                    modulePermissions.Add(permissionModel.ToEntity());
                });
            }

            var componentPermissions = new List<UserPermissionModel>();
            if (model.ComponentPermissions != null && model.ComponentPermissions.Count > 0)
            {
                model.ComponentPermissions.ForEach(delegate (UserPermissionViewModel permissionModel)
                {
                    componentPermissions.Add(permissionModel.ToEntity());
                });
            }

            var entity = new RoleSecurityModel
            {
                RoleID = model.RoleID,
                RoleName = model.RoleName,
                ModuleID = model.ModuleID,
                ModuleName = model.ModuleName,
                RoleModuleID = model.RoleModuleID,
                DataKey = model.DataKey,
                ComponentID = model.ComponentID,
                ComponentName = model.ComponentName,
                ModulePermissions = modulePermissions,
                ComponentPermissions = componentPermissions,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        public static Response<RoleViewModel> ToModel(this Response<RoleModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<RoleViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (RoleModel roleModel)
                {
                    var transformedModel = roleModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<RoleViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID=entity.ID
            };

            return model;
        }

        public static Response<RoleModel> ToEntity(this Response<RoleViewModel> model)
        {
            if (model == null)
                return null;

            var dataItems = new List<RoleModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (RoleViewModel roleViewModel)
                {
                    var transformedModel = roleViewModel.ToEntity();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var entity = new Response<RoleModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = dataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<RoleModuleViewModel> ToModel(this Response<RoleModuleModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<RoleModuleViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (RoleModuleModel roleModuleModel)
                {
                    var transformedModel = roleModuleModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<RoleModuleViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<RoleModuleModel> ToEntity(this Response<RoleModuleViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<RoleModuleModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (RoleModuleViewModel roleModuleViewModel)
                {
                    var transformedModel = roleModuleViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<RoleModuleModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<ModuleViewModel> ToModel(this Response<ModuleModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<ModuleViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (ModuleModel moduleModel)
                {
                    var transformedModel = moduleModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<ModuleViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<ModuleModel> ToEntity(this Response<ModuleViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<ModuleModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (ModuleViewModel moduleViewModel)
                {
                    var transformedModel = moduleViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<ModuleModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<FeatureViewModel> ToModel(this Response<FeatureModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<FeatureViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (FeatureModel featureModel)
                {
                    var transformedModel = featureModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<FeatureViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<FeatureModel> ToEntity(this Response<FeatureViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<FeatureModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (FeatureViewModel featureViewModel)
                {
                    var transformedModel = featureViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<FeatureModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<ModuleFeatureViewModel> ToModel(this Response<ModuleFeatureModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<ModuleFeatureViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (ModuleFeatureModel moduleFeatureModel)
                {
                    var transformedModel = moduleFeatureModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<ModuleFeatureViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<ModuleFeatureModel> ToEntity(this Response<ModuleFeatureViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<ModuleFeatureModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (ModuleFeatureViewModel moduleFeatureViewModel)
                {
                    var transformedModel = moduleFeatureViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<ModuleFeatureModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<PermissionViewModel> ToModel(this Response<PermissionModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<PermissionViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (PermissionModel permissionModel)
                {
                    var transformedModel = permissionModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<PermissionViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<PermissionModel> ToEntity(this Response<PermissionViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<PermissionModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (PermissionViewModel permissionViewModel)
                {
                    var transformedModel = permissionViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<PermissionModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<RolePermissionsViewModel> ToModel(this Response<RolePermissionsModel> entity)
        {
            if (entity == null)
                return null;

            var DataItems = new List<RolePermissionsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (RolePermissionsModel rolePermissionsModel)
                {
                    var transformedModel = rolePermissionsModel.ToModel();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var model = new Response<RolePermissionsViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = DataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<RolePermissionsModel> ToEntity(this Response<RolePermissionsViewModel> model)
        {
            if (model == null)
                return null;

            var DataItems = new List<RolePermissionsModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (RolePermissionsViewModel rolePermissionsViewModel)
                {
                    var transformedModel = rolePermissionsViewModel.ToEntity();
                    DataItems.Add(transformedModel);
                });
            }
            else
                DataItems = null;

            var entity = new Response<RolePermissionsModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = DataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        public static Response<RoleSecurityViewModel> ToModel(this Response<RoleSecurityModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<RoleSecurityViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (RoleSecurityModel roleSecurityModel)
                {
                    var transformedModel = roleSecurityModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<RoleSecurityViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<RoleSecurityModel> ToEntity(this Response<RoleSecurityViewModel> model)
        {
            if (model == null)
                return null;

            var dataItems = new List<RoleSecurityModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate (RoleSecurityViewModel roleSecurityViewModel)
                {
                    var transformedModel = roleSecurityViewModel.ToEntity();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var entity = new Response<RoleSecurityModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = dataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult
            };

            return entity;
        }

        #endregion RoleModel Permissions

        #region Credential Model Permissions

        public static CredentialSecurityViewModel ToModel(this CredentialSecurityModel entity)
        {
            if (entity == null)
                return null;

            var model = new CredentialSecurityViewModel
            {
                CredentialID = entity.CredentialID,
                CredentialName = entity.CredentialName,
                CredentialActionID = entity.CredentialActionID,
                CredentialAction = entity.CredentialAction,
                CredentialActionForm = entity.CredentialActionForm,
                ModifiedOn = entity.ModifiedOn,
                ServicesID = entity.ServicesID
            };

            return model;
        }

        public static Response<CredentialSecurityViewModel> ToModel(this Response<CredentialSecurityModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<CredentialSecurityViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (CredentialSecurityModel credentialSecurityModel)
                {
                    var transformedModel = credentialSecurityModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<CredentialSecurityViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        #endregion Credential Model Permissions
    }
}