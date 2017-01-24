using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common.General
{
    public class TileInfoModel
    {
        public string TileName { get; set; }
        public List<TileDetailsModel> TileDetails { get; set; }
        string url = "patientprofile.general";//TODO : Temp state till than fix all the url
        public string Url { 
            get
            {
                return url;            
            } 
            set
            {
                url = value;
            }
        }

    }
}
