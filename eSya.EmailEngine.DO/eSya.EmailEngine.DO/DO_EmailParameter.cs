using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DO
{
    public enum emailParams
    {
        User = 1,
        Patient = 2,
        NextToKin = 3,
        Doctor = 4,
        Recipient = 5,
        Customer = 6,
        Vendor = 7,
        Employee = 8,
        Direct = 9,

    }

    public class DO_EmailParameter
    {
        public int BusinessKey { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? LoginID { get; set; }
        public int EmailType { get; set; }
        public int FormID { get; set; }
        public string Emailid { get; set; }
        public int TEventID { get; set; }
        public string? OTP { get; set; }

        //public string? MessageType { get; set; }
        //public string? ReminderType { get; set; }
        //public string? NavigationURL { get; set; }
        //public string? SMSID { get; set; }
        //public long ReferenceKey { get; set; }


        //public int UHID { get; set; }
        //public int DoctorID { get; set; }
        //public int CustomerID { get; set; }
        //public int VendorID { get; set; }
        //public int EmployeeID { get; set; }
        //public string? Name { get; set; }
        //public string? MobileNumber { get; set; }
        //public DateTime? ScheduleDate { get; set; }
        //public Dictionary<string, string>? SmsVariables { get; set; }

        //public string? Password { get; set; }
        //public bool IsUserPasswordInclude { get; set; }
    }

    public class DO_EmailStatement
    {
        public string EmailTempid { get; set; }
        public int FormID { get; set; }
        public string EmailTempDesc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool IsVariable { get; set; }
        public bool IsAttachmentReqd { get; set; }
        public List<DO_EmailParam> l_EmailParam { get; set; }
        public List<DO_EmailRecipient> l_EmailRecipient { get; set; }
    }

    public class DO_EmailParam
    {
        public string EmailTempid { get; set; }
        public int ParameterID { get; set; }
        public bool ParmAction { get; set; }
        public string Emailid { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
    }

    public class DO_Master
    {
        public string Emailid { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
