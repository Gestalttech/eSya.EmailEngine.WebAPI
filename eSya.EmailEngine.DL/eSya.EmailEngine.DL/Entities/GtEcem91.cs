using System;
using System.Collections.Generic;

namespace eSya.EmailEngine.DL.Entities
{
    public partial class GtEcem91
    {
        public int BusinessKey { get; set; }
        public int EmailType { get; set; }
        public string OutgoingMailServer { get; set; } = null!;
        public int Port { get; set; }
        public string SenderEmailId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PassKey { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
