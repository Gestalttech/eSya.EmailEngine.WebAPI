using eSya.EmailEngine.DL.Entities;
using Microsoft.EntityFrameworkCore;
using eSya.EmailEngine.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DL.Repository
{
    public class CommonMethod
    {
        public static string GetValidationMessageFromException(DbUpdateException ex)
        {
            string msg = ex.InnerException == null ? ex.ToString() : ex.InnerException.Message;

            if (msg.LastIndexOf(',') == msg.Length - 1)
                msg = msg.Remove(msg.LastIndexOf(','));
            return msg;
        }
    }
}
