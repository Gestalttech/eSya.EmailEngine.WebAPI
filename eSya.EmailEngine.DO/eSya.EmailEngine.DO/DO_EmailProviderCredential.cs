using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DO
{
    public class DO_EmailProviderCredential
    {
        public int BusinessKey { get; set; }
        public int EmailType { get; set; }
        public string OutgoingMailServer { get; set; } = null!;
        public int Port { get; set; }
        public string SenderEmailId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PassKey { get; set; }
    }
    public class DO_EmailModel
    {
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public byte[] AttachmentFile { get; set; }
        public Attachment Attachment { get; set; }
    }

}
