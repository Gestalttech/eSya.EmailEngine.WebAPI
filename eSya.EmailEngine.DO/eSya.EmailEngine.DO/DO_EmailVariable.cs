using System;
using System.Collections.Generic;
using System.Text;

namespace eSya.EmailEngine.DO
{
    public class DO_EmailVariable
    {
        public string Emavariable { get; set; }
        public string Emacomponent { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string FormID { get; set; }
    }
}
