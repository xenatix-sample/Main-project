using Axis.Model.Common;
using Axis.Plugins.ReportingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ReportingServices.Translator
{
    public static class ReportingServicesTranslator
    {
        public static ReportServicesViewModel ToViewModel(this ReportServicesViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReportServicesViewModel
            { 
                 
                ReportGroupID=entity.ReportGroupID,
                ReportID=entity.ReportID,
                ReportModel=entity.ReportModel,
                ReportName=entity.ReportName,
                ReportTypeID=entity.ReportTypeID,
                ReportTypeName=entity.ReportTypeName,
                ReportURL=entity.ReportURL,
                ReportDisplayName = entity.ReportDisplayName
     
            };

            return model;
        }

        public static Response<ReportServicesViewModel> ToViewModel(this Response<ReportServicesViewModel> entity)
        {
            if (entity == null)
                return null;

            var viewModel = entity.CloneResponse<ReportServicesViewModel>();

            viewModel.DataItems = entity.DataItems.Select(x => x.ToViewModel()).ToList();

            return viewModel;
        }
    }
}
