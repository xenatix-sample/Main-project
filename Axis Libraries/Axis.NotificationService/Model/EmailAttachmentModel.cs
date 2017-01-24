using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.NotificationService.Model
{
    public class EmailAttachmentModel
    {
        public string AttachmentName { get; set; }
        public string MediaType { get; set; }
        public byte[] AttachmentContent { get; set; }
    }
}
