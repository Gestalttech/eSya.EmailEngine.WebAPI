using System;
using System.Collections.Generic;

namespace eSya.EmailEngine.DL.Entities
{
    public partial class GtEcemar
    {
        public int BusinessKey { get; set; }
        public string EmailTempId { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public string RecipientName { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
