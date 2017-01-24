using System;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Registration.Models
{
    public class ConsentViewModel : BaseViewModel
    {
        public int? FormId { get; set; }
        public long? SignatureId { get; set; }
        public string SignatureBlob { get; set; }
        public long? ContactId { get; set; }
        public string AuthorizedBy { get; set; }
        public string ContactName { get; set; }
        public DateTime? ContactDateofBirth { get; set; }
    }
}
