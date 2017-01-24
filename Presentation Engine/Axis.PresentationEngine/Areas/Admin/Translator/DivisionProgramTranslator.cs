
using System.Linq;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class DivisionProgramTranslator
    {
        public static DivisionProgramViewModel ToViewModel(this DivisionProgramModel entity)
        {
            if (entity == null)
                return null;

            var model = new DivisionProgramViewModel
            {
                userID = entity.userID,
                MappingID = entity.MappingID,
                Name = entity.Name,
                CompanyMappingID = entity.CompanyMappingID,
                CompanyName = entity.CompanyName,
                Programs = new List<ProgramsViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.Programs != null)
            {
                entity.Programs.ForEach(delegate (ProgramsModel progModel)
                {
                    var transformedModel = progModel.ToViewModel();
                    model.Programs.Add(transformedModel);
                    model.RowCount += transformedModel.RowCount;
                });
            }

            var firstProgram = model.Programs.FirstOrDefault();
            if (firstProgram != null) firstProgram.IsFirst = true;

            var lastProgram = model.Programs.LastOrDefault();
            if (lastProgram != null) lastProgram.IsLast = true;

            return model;
        }

        public static ProgramsViewModel ToViewModel(this ProgramsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ProgramsViewModel
            {
                MappingID = entity.MappingID,
                Name = entity.Name,
                ProgramUnits = new List<DivisionProgramBaseViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.ProgramUnits != null)
            {
                entity.ProgramUnits.ForEach(delegate (DivisionProgramBaseModel userProgram)
                {
                    var transformedModel = userProgram.ToViewModel();
                    transformedModel.IsActive = true;
                    model.ProgramUnits.Add(transformedModel);
                });
            }

            model.RowCount = model.ProgramUnits.Any() ? model.ProgramUnits.Count() : 1;
            var firstProgramUnit = model.ProgramUnits.FirstOrDefault();
            if (firstProgramUnit != null) firstProgramUnit.IsFirst = true;

            var lastProgramUnit = model.ProgramUnits.LastOrDefault();
            if (lastProgramUnit != null) lastProgramUnit.IsLast = true;

            return model;
        }

        public static DivisionProgramBaseViewModel ToViewModel(this DivisionProgramBaseModel entity)
        {
            if (entity == null)
                return null;

            var model = new DivisionProgramBaseViewModel
            {
                MappingID = entity.MappingID,
                Name = entity.Name,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }
        public static Response<DivisionProgramViewModel> ToViewModel(this Response<DivisionProgramModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<DivisionProgramViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (DivisionProgramModel divisionProgramModel)
                {
                    var transformedModel = divisionProgramModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<DivisionProgramViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }

        public static DivisionProgramModel ToModel(this DivisionProgramViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new DivisionProgramModel
            {
                userID = entity.userID,
                MappingID = entity.MappingID,
                Name = entity.Name,
                IsActive = entity.IsActive,
                Programs = new List<ProgramsModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.Programs != null)
            {
                entity.Programs.ForEach(delegate (ProgramsViewModel progModel)
                {
                    var transformedModel = progModel.ToModel();
                    model.Programs.Add(transformedModel);
                });
            }

            return model;
        }

        public static ProgramsModel ToModel(this ProgramsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ProgramsModel
            {
                MappingID = entity.MappingID,
                Name = entity.Name,
                IsActive = entity.IsActive,
                ProgramUnits = new List<DivisionProgramBaseModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.ProgramUnits != null)
            {
                entity.ProgramUnits.ForEach(delegate (DivisionProgramBaseViewModel programUnit)
                {
                    var transformedModel = programUnit.ToModel();
                    model.ProgramUnits.Add(transformedModel);
                });
            }

            return model;
        }

        public static DivisionProgramBaseModel ToModel(this DivisionProgramBaseViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new DivisionProgramBaseModel
            {
                MappingID = entity.MappingID,
                Name = entity.Name,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }
    }
}