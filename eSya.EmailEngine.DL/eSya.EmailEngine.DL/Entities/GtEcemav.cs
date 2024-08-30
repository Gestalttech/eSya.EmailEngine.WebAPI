using System;
using System.Collections.Generic;

namespace eSya.EmailEngine.DL.Entities
{
    public partial class GtEcemav
    {
        public string Emavariable { get; set; } = null!;
        public string Emacomponent { get; set; } = null!;
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
