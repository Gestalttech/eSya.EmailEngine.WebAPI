using eSya.EmailEngine.DL.Entities;
using eSya.EmailEngine.DL.Utility;
using eSya.EmailEngine.DO;
using eSya.EmailEngine.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DL.Repository
{
    public class EmailStatementRepository : IEmailStatementRepository
    {
        public async Task<DO_Master> GetMasterDetail(int type, int id)
        {
            using (var db = new eSyaEnterprise())
            {
                DO_Master? us = new DO_Master();

                if (type == (int)emailParams.User)
                {
                    us = await db.GtEuusms.Where(x => x.UserId == id).
                           Select(r => new DO_Master
                           {
                               Emailid = r.EMailId,
                               ID = r.LoginId,
                               Name = r.LoginDesc
                           }).FirstOrDefaultAsync();
                }
                return us;
            }
        }
        public async Task<DO_EmailProviderCredential> EmailProviderCredential(int BusinessKey, int emailType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcem91s.Where(x => x.BusinessKey == BusinessKey && x.EmailType == emailType && x.ActiveStatus)
                        .Select(r => new DO_EmailProviderCredential
                        {
                            OutgoingMailServer = r.OutgoingMailServer,
                            Port = r.Port,
                            SenderEmailId = r.SenderEmailId,
                            UserName = eSyaCryptGeneration.Decrypt(r.UserName),
                            Password = eSyaCryptGeneration.Decrypt(r.Password),
                            PassKey = r.PassKey
                        }).FirstOrDefaultAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_EmailStatement>> GetEmailonSaveClick(DO_EmailParameter sp)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fs = await db.GtEcemahs.Join(db.GtEcemlos,
                           u => u.FormId,
                           b => b.FormId,
                           (u, b) => new { u, b })
                        .Where(w => w.u.FormId == sp.FormID && w.u.TeventId == sp.TEventID && w.u.SequenceNumber == sp.SequenceNumber
                                    && w.u.ActiveStatus == true && w.b.BusinessKey == sp.BusinessKey)
                        .Select(r => new DO_EmailStatement
                        {
                            EmailTempid = r.u.EmailTempId,
                            EmailTempDesc = r.u.EmailTempDesc,
                            EmailSubject = r.u.EmailSubject,
                            EmailBody = r.u.EmailBody,
                            l_EmailParam = db.GtEcemads.Where(w => w.ParmAction && w.EmailTempId == r.u.EmailTempId && w.ActiveStatus)
                                         .Select(d => new DO_EmailParam
                                         {
                                             ParameterID = d.ParameterId,
                                             ParmAction = d.ParmAction
                                         }).ToList(),
                            l_EmailRecipient = db.GtEcemars.Where(w => w.BusinessKey == sp.BusinessKey && w.EmailTempId == r.u.EmailTempId && w.ActiveStatus)
                                        .Select(x => new DO_EmailRecipient
                                        {
                                            Emailid = x.EmailId,
                                            RecipientName = x.RecipientName,
                                            Remarks = x.Remarks
                                        }).ToList()
                        }).ToListAsync();

                    foreach (var s in fs)
                    {
                        foreach (var p in s.l_EmailParam)
                        {
                            int id = 0;
                            if (p.ParameterID == (int)emailParams.Direct)
                            {
                                p.Emailid = sp.Emailid;
                                p.Name = sp.UserName;
                            }

                            if (p.ParameterID == (int)emailParams.User)
                                id = sp.UserID;
                            if (id > 0)
                            {
                                var ms = await GetMasterDetail(p.ParameterID, id);
                                p.Emailid = ms.Emailid;
                                p.ID = ms.ID;
                                p.Name = ms.Name;
                            }
                        }
                    }

                    return fs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
