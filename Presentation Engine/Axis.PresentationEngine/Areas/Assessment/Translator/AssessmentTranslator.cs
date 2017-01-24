using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using Axis.PresentationEngine.Areas.Assessment.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Assessment.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class AssessmentTranslator
    {
        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentViewModel ToViewModel(this AssessmentModel entity)
        {
            if (entity == null)
                return null;

            var model = new AssessmentViewModel
            {
                AssessmentID = entity.AssessmentID,
                AssessmentName = entity.AssessmentName,
                CategoryAbbreviation = entity.CategoryAbbreviation,
                CategoryID = entity.CategoryID,
                CategoryName = entity.CategoryName,
                Frequency = entity.Frequency,
                FrequencyID = entity.FrequencyID,
                ImageContent = entity.ImageContent,
                ImageID = entity.ImageID,
                ProgramID = entity.ProgramID,
                ProgramName = entity.ProgramName,
                VersionID = entity.VersionID,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback,
                DocumentTypeID = entity.DocumentTypeID
            };
            var assessmentSectionList = new List<AssessmentSectionsViewModel>();
            if (entity.AssessmentSections != null)
            {
                entity.AssessmentSections.ForEach(delegate (AssessmentSectionsModel assessmentSection)
                {
                    assessmentSectionList.Add(assessmentSection.ToViewModel());
                });

                model.AssessmentSections = assessmentSectionList;
            }
            return model;
        }

        /// <summary>
        /// Translate the entity data to for View to Process.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static AssessmentQuestionLogicViewModel ToViewModel(this AssessmentQuestionLogicModel entity)
        {
            if (entity == null)
                return null;

            var model = new AssessmentQuestionLogicViewModel
            {
                LogicID = entity.LogicID,
                QuestionDataKey = entity.QuestionDataKey,
                LogicCode = entity.LogicCode,
                LogicLocationId = entity.LogicLocationId,
                LogicOrder = entity.LogicOrder,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };

            return model;
        }

        public static Response<AssessmentQuestionLogicViewModel> ToViewModel(this Response<AssessmentQuestionLogicModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentQuestionLogicViewModel>();
            var assessmentsLogic = new List<AssessmentQuestionLogicViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentQuestionLogicModel assessmentLogic)
                {
                    assessmentsLogic.Add(assessmentLogic.ToViewModel());
                });

                model.DataItems = assessmentsLogic;
            }

            return model;
        }

        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentQuestionOptionViewModel ToViewModel(this AssessmentQuestionOptionModel entity)
        {
            if (entity == null)
                return null;

            var model = new AssessmentQuestionOptionViewModel
            {
                Options = entity.Options,
                OptionsGroupID = entity.OptionsGroupID,
                OptionsGroupName = entity.OptionsGroupName,
                OptionsID = entity.OptionsID,
                QuestionID = entity.QuestionID,
                Attributes = entity.Attributes,
                SortOrder = entity.SortOrder,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentQuestionViewModel ToViewModel(this AssessmentQuestionModel entity)
        {
            if (entity == null)
                return null;
            var model = new AssessmentQuestionViewModel
            {
                AssessmentSectionID = entity.AssessmentSectionID,
                ImageID = entity.ImageID,
                InputType = entity.InputType,
                InputTypeID = entity.InputTypeID,
                InputTypePosition = entity.InputTypePosition,
                InputTypePositionID = entity.InputTypePositionID,
                IsAnswerRequired = entity.IsAnswerRequired,
                IsNumberingRequired = entity.IsNumberingRequired,
                OptionsGroupID = entity.OptionsGroupID,
                ParentQuestionID = entity.ParentQuestionID,
                ParentOptionsID = entity.ParentOptionsID,
                Question = entity.Question,
                QuestionID = entity.QuestionID,
                QuestionType = entity.QuestionType,
                QuestionTypeID = entity.QuestionTypeID,
                SortOrder = entity.SortOrder,
                DataKey = entity.DataKey,
                ContainerAttributes = entity.ContainerAttributes,
                Attributes = entity.Attributes,
                ModifiedOn = entity.ModifiedOn,
            };
            var assessmentQuestionOptionList = new List<AssessmentQuestionOptionViewModel>();

            if (entity.AssessmentQuestionOptions != null)
            {
                entity.AssessmentQuestionOptions.ForEach(delegate (AssessmentQuestionOptionModel assessmentQuestionOption)
                {
                    assessmentQuestionOptionList.Add(assessmentQuestionOption.ToViewModel());
                });

                model.AssessmentQuestionOptions = assessmentQuestionOptionList;
            }
            return model;
        }

        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentResponseViewModel ToViewModel(this AssessmentResponseModel entity)
        {
            if (entity == null)
                return null;
            var model = new AssessmentResponseViewModel
            {
                AssessmentID = entity.AssessmentID,
                ContactID = entity.ContactID,
                EnterDate = entity.EnterDate,
                ResponseID = entity.ResponseID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentResponseDetailsViewModel ToViewModel(this AssessmentResponseDetailsModel entity)
        {
            if (entity == null)
                return null;
            var model = new AssessmentResponseDetailsViewModel
            {
                ResponseDetailsID = entity.ResponseDetailsID,
                QuestionID = entity.QuestionID,
                OptionsID = entity.OptionsID,
                Options = entity.Options,
                ResponseID = entity.ResponseID,
                ResponseText = entity.ResponseText,
                Rating = entity.Rating,
                ModifiedOn = entity.ModifiedOn,
                SignatureBLOB = entity.SignatureBLOB,
                AuditXML = entity.AuditXML
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AssessmentSectionsViewModel ToViewModel(this AssessmentSectionsModel entity)
        {
            if (entity == null)
                return null;

            var model = new AssessmentSectionsViewModel
            {
                AssessmentSectionID = entity.AssessmentSectionID,
                SectionName = entity.SectionName,
                AssessmentID = entity.AssessmentID,
                SortOrder = entity.SortOrder,
                PermissionKey = entity.PermissionKey,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback
            };

            return model;
        }

        /// <summary>
        /// Translate model to view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AssessmentViewModel> ToViewModel(this Response<AssessmentModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentViewModel>();
            var assessments = new List<AssessmentViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentModel assessment)
                {
                    assessments.Add(assessment.ToViewModel());
                });

                model.DataItems = assessments;
            }

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AssessmentSectionsViewModel> ToViewModel(this Response<AssessmentSectionsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentSectionsViewModel>();
            var assessmentSections = new List<AssessmentSectionsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentSectionsModel assessmentSectionsModel)
                {
                    assessmentSections.Add(assessmentSectionsModel.ToViewModel());
                });

                model.DataItems = assessmentSections;
            }

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AssessmentQuestionViewModel> ToViewModel(this Response<AssessmentQuestionModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentQuestionViewModel>();
            var assessmentQuestion = new List<AssessmentQuestionViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentQuestionModel assessmentQuestionModel)
                {
                    assessmentQuestion.Add(assessmentQuestionModel.ToViewModel());
                });

                model.DataItems = assessmentQuestion;
            }

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AssessmentResponseViewModel> ToViewModel(this Response<AssessmentResponseModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentResponseViewModel>();
            var assessmentResponse = new List<AssessmentResponseViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentResponseModel assessmentResponseModel)
                {
                    assessmentResponse.Add(assessmentResponseModel.ToViewModel());
                });

                model.DataItems = assessmentResponse;
            }

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AssessmentResponseDetailsViewModel> ToViewModel(this Response<AssessmentResponseDetailsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AssessmentResponseDetailsViewModel>();
            var assessmentResponseDetails = new List<AssessmentResponseDetailsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (AssessmentResponseDetailsModel assessmentResponseDetailsModel)
                {
                    assessmentResponseDetails.Add(assessmentResponseDetailsModel.ToViewModel());
                });

                model.DataItems = assessmentResponseDetails;
            }

            return model;
        }

        /// <summary>
        /// Translate view model to model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentModel ToModel(this AssessmentViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AssessmentModel
            {
                AssessmentID = model.AssessmentID,
                AssessmentName = model.AssessmentName,
                CategoryAbbreviation = model.CategoryAbbreviation,
                CategoryID = model.CategoryID,
                CategoryName = model.CategoryName,
                Frequency = model.Frequency,
                FrequencyID = model.FrequencyID,
                ImageContent = model.ImageContent,
                ImageID = model.ImageID,
                ProgramID = model.ProgramID,
                ProgramName = model.ProgramName,
                VersionID = model.VersionID,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback,
                DocumentTypeID = model.DocumentTypeID
            };
            var assessmentSectionList = new List<AssessmentSectionsModel>();
            if (model.AssessmentSections != null)
            {
                model.AssessmentSections.ForEach(delegate (AssessmentSectionsViewModel assessmentSection)
                {
                    assessmentSectionList.Add(assessmentSection.ToModel());
                });

                entity.AssessmentSections = assessmentSectionList;
            }
            return entity;
        }

        /// <summary>
        /// Translate view model to model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentQuestionOptionModel ToModel(this AssessmentQuestionOptionViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AssessmentQuestionOptionModel
            {
                Options = model.Options,
                OptionsGroupID = model.OptionsGroupID,
                OptionsGroupName = model.OptionsGroupName,
                OptionsID = model.OptionsID,
                QuestionID = model.QuestionID,
                Attributes = model.Attributes,
                SortOrder = model.SortOrder,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        /// <summary>
        /// Translate view model to model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentQuestionModel ToModel(this AssessmentQuestionViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AssessmentQuestionModel
            {
                AssessmentSectionID = model.AssessmentSectionID,
                ImageID = model.ImageID,
                InputType = model.InputType,
                InputTypeID = model.InputTypeID,
                InputTypePosition = model.InputTypePosition,
                InputTypePositionID = model.InputTypePositionID,
                IsAnswerRequired = model.IsAnswerRequired,
                IsNumberingRequired = model.IsNumberingRequired,
                OptionsGroupID = model.OptionsGroupID,
                ParentQuestionID = model.ParentQuestionID,
                ParentOptionsID = model.ParentOptionsID,
                Question = model.Question,
                QuestionID = model.QuestionID,
                QuestionType = model.QuestionType,
                QuestionTypeID = model.QuestionTypeID,
                SortOrder = model.SortOrder,
                DataKey = model.DataKey,
                ContainerAttributes = model.ContainerAttributes,
                Attributes = model.Attributes,
                ModifiedOn = model.ModifiedOn
            };
            var assessmentQuestionOptionList = new List<AssessmentQuestionOptionModel>();

            if (model.AssessmentQuestionOptions != null)
            {
                model.AssessmentQuestionOptions.ForEach(delegate (AssessmentQuestionOptionViewModel assessmentQuestionOption)
                {
                    assessmentQuestionOptionList.Add(assessmentQuestionOption.ToModel());
                });

                entity.AssessmentQuestionOptions = assessmentQuestionOptionList;
            }
            return entity;
        }

        /// <summary>
        /// Translate view model to model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentResponseModel ToModel(this AssessmentResponseViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AssessmentResponseModel
            {
                AssessmentID = model.AssessmentID,
                ContactID = model.ContactID,
                EnterDate = model.EnterDate,
                ResponseID = model.ResponseID,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        /// <summary>
        /// Translate view model to model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentResponseDetailsModel ToModel(this AssessmentResponseDetailsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AssessmentResponseDetailsModel
            {
                ResponseDetailsID = model.ResponseDetailsID,
                QuestionID = model.QuestionID,
                OptionsID = model.OptionsID,
                Options = model.Options,
                ResponseID = model.ResponseID,
                ResponseText = model.ResponseText,
                Rating = model.Rating,
                ModifiedOn = model.ModifiedOn,
                SignatureBLOB = model.SignatureBLOB,
                AuditXML = model.AuditXML
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AssessmentSectionsModel ToModel(this AssessmentSectionsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AssessmentSectionsModel
            {
                AssessmentSectionID = model.AssessmentSectionID,
                SectionName = model.SectionName,
                AssessmentID = model.AssessmentID,
                SortOrder = model.SortOrder,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }
    }
}