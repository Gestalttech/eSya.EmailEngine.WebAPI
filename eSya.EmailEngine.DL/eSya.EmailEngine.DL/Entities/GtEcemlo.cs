using System;
using System.Collections.Generic;

namespace eSya.EmailEngine.DL.Entities
{
    public partial class GtEcemlo
    {
        public int BusinessKey { get; set; }
        public int FormId { get; set; }
        public string EmailTempId { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
