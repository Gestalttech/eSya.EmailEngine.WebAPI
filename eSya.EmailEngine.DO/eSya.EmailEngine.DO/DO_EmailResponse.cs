using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DO
{
    public class DO_EmailResponse
    {
        public string RequestMessage { get; set; }
        public string ResponseMessage { get; set; }
        public bool SendStatus { get; set; }
    }
}
