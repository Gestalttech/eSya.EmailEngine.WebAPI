﻿using System;
using System.Collections.Generic;

namespace eSya.EmailEngine.DL.Entities
{
    public partial class GtEcemah
    {
        public string EmailTempId { get; set; } = null!;
        public int EmailType { get; set; }
        public int FormId { get; set; }
        public string EmailTempDesc { get; set; } = null!;
        public string EmailSubject { get; set; } = null!;
        public string EmailBody { get; set; } = null!;
        public bool IsVariable { get; set; }
        public int TeventId { get; set; }
        public bool IsAttachmentReqd { get; set; }
        public int SequenceNumber { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
