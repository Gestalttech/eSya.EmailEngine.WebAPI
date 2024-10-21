using eSya.EmailEngine.DL;
using eSya.EmailEngine.DL.Entities;
using eSya.EmailEngine.DO;
using eSya.EmailEngine.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSya.EmailEngine.DL.Repository;

namespace eSya.EmailEngine.DL.Repository
{
    public class EmailEngineRepository : IEmailEngineRepository
    {
        private readonly IStringLocalizer<EmailEngineRepository> _localizer;
        public EmailEngineRepository(IStringLocalizer<EmailEngineRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Email Variable

        public async Task<List<DO_EmailVariable>> GetEmailVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemavs
                         .Select(r => new DO_EmailVariable
                         {
                             Emavariable = r.Emavariable,
                             Emacomponent = r.Emacomponent,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Emavariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EmailVariable>> GetActiveEmailVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemavs
                        .Where(w => w.ActiveStatus)
                         .Select(r => new DO_EmailVariable
                         {
                             Emavariable = r.Emavariable,
                             Emacomponent = r.Emacomponent
                         }).OrderBy(o => o.Emavariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoEmailVariable(DO_EmailVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_EmailVariableExist = db.GtEcemavs.Any(a => a.Emavariable.Trim().ToUpper() == obj.Emavariable.Trim().ToUpper());
                        if (is_EmailVariableExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0072", Message = string.Format(_localizer[name: "W0072"]) };
                        }

                        bool is_EmailComponentExist = db.GtEcemavs.Any(a => a.Emacomponent.Trim().ToUpper() == obj.Emacomponent.Trim().ToUpper());
                        if (is_EmailComponentExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0073", Message = string.Format(_localizer[name: "W0073"]) };
                        }

                        var em_sv = new GtEcemav
                        {
                            Emavariable = obj.Emavariable,
                            Emacomponent = obj.Emacomponent,
                            ActiveStatus = obj.ActiveStatus,
                            FormId=obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcemavs.Add(em_sv);

                        await db.SaveChangesAsync();
                        dbContext.Commit();


                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateEmailVariable(DO_EmailVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_EmailComponentExist = db.GtEcemavs.Where(w => w.Emacomponent.Trim().ToUpper().Replace(" ", "") == obj.Emacomponent.Trim().ToUpper().Replace(" ", "")
                                && w.Emavariable != obj.Emavariable).FirstOrDefault();
                        if (is_EmailComponentExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0073", Message = string.Format(_localizer[name: "W0073"]) };
                        }

                        GtEcemav em_sv = db.GtEcemavs.Where(w => w.Emavariable == obj.Emavariable).FirstOrDefault();
                        if (em_sv == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0074", Message = string.Format(_localizer[name: "W0074"]) };
                        }

                        em_sv.Emacomponent = obj.Emacomponent;
                        em_sv.ActiveStatus = obj.ActiveStatus;
                        em_sv.ModifiedBy = obj.UserID;
                        em_sv.ModifiedOn = DateTime.Now;
                        em_sv.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveEmailVariable(bool status, string Emavariable)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcemav email_var = db.GtEcemavs.Where(w => w.Emavariable.Trim().ToUpper().Replace(" ", "") == Emavariable.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (email_var == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0074", Message = string.Format(_localizer[name: "W0074"]) };
                        }

                        email_var.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion Email Variable

        #region Email Template
        public async Task<List<DO_EMailTEvent>> GetTriggerEvent()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsts
                        .Where(w => w.ActiveStatus)
                         .Select(r => new DO_EMailTEvent
                         {
                             TEventID = r.TeventId,
                             TEventDesc = r.TeventDesc,
                             ActiveStatus = r.ActiveStatus
                         }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_Forms>> GetExistingFormsFromEmailHeader()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcemahs.Where(x => x.ActiveStatus == true).Join(db.GtEcfmfds,
                        d => d.FormId,
                        f => f.FormId,
                        (d, f) => new DO_Forms
                        {
                            FormID = d.FormId,
                            FormName = f.FormName,
                        }).GroupBy(x => x.FormID).Select(y => y.First()).Distinct().ToListAsync();
                    return await result;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EmailHeader>> GetEmailHeaderInformationByFormId(int formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemahs
                       .Where(w => w.FormId == formId)
                       .Select(r => new DO_EmailHeader
                       {
                           EmailTempid = r.EmailTempId,
                           FormId = r.FormId,
                           EmailTempDesc = r.EmailTempDesc,
                           EmailSubject = r.EmailSubject,
                           EmailBody = r.EmailBody,
                           IsAttachmentReqd = r.IsAttachmentReqd,
                           TeventId=r.TeventId,
                           ActiveStatus = r.ActiveStatus
                       }).OrderBy(o => o.EmailTempid).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_EmailHeader> GetEmailHeaderInformationByEmailId(string emailTempId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemahs
                        .Where(w => w.EmailTempId == emailTempId)
                         .Select(r => new DO_EmailHeader
                         {
                             EmailTempid = r.EmailTempId,
                             EmailType = r.EmailType,
                             EmailTempDesc = r.EmailTempDesc,
                             EmailSubject = r.EmailSubject,
                             EmailBody = r.EmailBody,
                             IsVariable = r.IsVariable,
                             TeventId=r.TeventId,
                             IsAttachmentReqd = r.IsAttachmentReqd,
                             ActiveStatus = r.ActiveStatus,
                             //l_EmailParameter = r.GtEcemads.Select(p => new DO_eSyaParameter
                             //{
                             //    ParameterID = p.ParameterId,
                             //    ParmAction = p.ParmAction
                             //}).ToList()
                             l_EmailParameter = db.GtEcemads.Where(x=>x.EmailTempId==emailTempId).Select(p => new DO_eSyaParameter
                             {
                                 ParameterID = p.ParameterId,
                                 ParmAction = p.ParmAction
                             }).ToList()
                         }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoEmailHeader(DO_EmailHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSDescExist = db.GtEcemahs.Any(a => a.EmailTempDesc.Trim().ToUpper() == obj.EmailTempDesc.Trim().ToUpper());
                        if (is_SMSDescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0071", Message = string.Format(_localizer[name: "W0071"]) };
                        }

                        var emaiIdNumber = db.GtEcemahs.Where(w => w.FormId == obj.FormId).Count();
                        string emailTempId = obj.FormId.ToString() + "_" + (emaiIdNumber + 1).ToString();

                        var sm_sh = new GtEcemah
                        {
                            EmailTempId = emailTempId,
                            EmailType = obj.EmailType,
                            FormId = obj.FormId,
                            EmailTempDesc = obj.EmailTempDesc,
                            EmailSubject = obj.EmailSubject,
                            EmailBody = obj.EmailBody,
                            IsVariable = obj.IsVariable,
                            TeventId=obj.TeventId,
                            IsAttachmentReqd = obj.IsAttachmentReqd,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcemahs.Add(sm_sh);

                        foreach (var p in obj.l_EmailParameter)
                        {
                            var sm_sd = new GtEcemad
                            {
                                EmailTempId = emailTempId,
                                ParameterId = p.ParameterID,
                                ParmAction = p.ParmAction,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId.ToString(),
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                                CreatedBy = obj.UserID,
                            };
                            db.GtEcemads.Add(sm_sd);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateEmailHeader(DO_EmailHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSComponentExist = db.GtEcemahs.Where(w => w.EmailTempDesc.Trim().ToUpper().Replace(" ", "") == obj.EmailTempDesc.Trim().ToUpper().Replace(" ", "")
                                && w.EmailTempId != obj.EmailTempid && w.FormId == obj.FormId).FirstOrDefault();
                        if (is_SMSComponentExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0071", Message = string.Format(_localizer[name: "W0071"]) };
                        }

                        GtEcemah sm_sh = db.GtEcemahs.Where(w => w.EmailTempId == obj.EmailTempid).FirstOrDefault();
                        if (sm_sh == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
                        }

                        sm_sh.EmailType = obj.EmailType;
                        sm_sh.EmailTempDesc = obj.EmailTempDesc;
                        sm_sh.EmailSubject = obj.EmailSubject;
                        sm_sh.EmailBody = obj.EmailBody;
                        sm_sh.IsVariable = obj.IsVariable;
                        sm_sh.TeventId=obj.TeventId;
                        sm_sh.IsAttachmentReqd = obj.IsAttachmentReqd;
                        sm_sh.ActiveStatus = obj.ActiveStatus;
                        sm_sh.ModifiedBy = obj.UserID;
                        sm_sh.ModifiedOn = DateTime.Now;
                        sm_sh.ModifiedTerminal = obj.TerminalID;

                        foreach (var p in obj.l_EmailParameter)
                        {
                            var sm_sd = db.GtEcemads.Where(w => w.EmailTempId == obj.EmailTempid && w.ParameterId == p.ParameterID).FirstOrDefault();
                            if (sm_sd == null)
                            {
                                sm_sd = new GtEcemad
                                {
                                    EmailTempId = obj.EmailTempid,
                                    ParameterId = p.ParameterID,
                                    ParmAction = p.ParmAction,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId.ToString(),
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                    CreatedBy = obj.UserID,
                                };
                                db.GtEcemads.Add(sm_sd);
                            }
                            else
                            {
                                sm_sd.ParmAction = p.ParmAction;
                                sm_sd.ActiveStatus = obj.ActiveStatus;
                                sm_sd.ModifiedBy = obj.UserID;
                                sm_sd.ModifiedOn = System.DateTime.Now;
                                sm_sd.ModifiedTerminal = obj.TerminalID;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Email Template

        #region Manage Email Location Wise

        public async Task<List<DO_EmailHeader>> GetEmailInformationFormLocationWise(int businessKey, int formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcemahs
                       .Where(w => w.FormId == formId)
                       .Select(r => new DO_EmailHeader
                       {
                           EmailTempid = r.EmailTempId,
                           EmailTempDesc = r.EmailTempDesc,
                           ActiveStatus = r.ActiveStatus,
                           FormId = r.FormId,
                       }).OrderBy(o => o.EmailTempid).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcemlo pf = db.GtEcemlos.Where(x => x.BusinessKey == businessKey && x.FormId == obj.FormId && x.EmailTempId == obj.EmailTempid).FirstOrDefault();
                        if (pf != null)
                        {
                            obj.ActiveStatus = pf.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;

                        }
                    }
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateEmailInformationFLW(List<DO_BusinessFormEmailLink> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var email_loc in obj)
                        {
                            GtEcemlo Emailloc = db.GtEcemlos.Where(x => x.BusinessKey == email_loc.BusinessKey && x.FormId == email_loc.FormId && x.EmailTempId == email_loc.EmailTempId).FirstOrDefault();
                            if (Emailloc != null)
                            {

                                Emailloc.ActiveStatus = email_loc.ActiveStatus;
                                Emailloc.ModifiedBy = email_loc.UserID;
                                Emailloc.ModifiedOn = System.DateTime.Now;
                                Emailloc.ModifiedTerminal = email_loc.TerminalID;
                            }
                            else
                            {
                                var Emailloc1 = new GtEcemlo
                                {
                                    BusinessKey = email_loc.BusinessKey,
                                    FormId = email_loc.FormId,
                                    EmailTempId = email_loc.EmailTempId,
                                    ActiveStatus = email_loc.ActiveStatus,
                                    CreatedBy = email_loc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = email_loc.TerminalID
                                };
                                db.GtEcemlos.Add(Emailloc1);
                            }
                            await db.SaveChangesAsync();
                        }
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Manage Email Location Wise

        #region Email Recipient

        public async Task<List<DO_EmailHeader>> GetEmailHeaderForRecipientByFormIdandParamId(int formId, int parameterId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemads
                        .Where(w => w.ParameterId == parameterId && w.ActiveStatus && w.ParmAction)
                        .Join(db.GtEcemahs.Where(x=>x.FormId==formId && x.ActiveStatus),
                        p => new {p.EmailTempId},
                        e => new {e.EmailTempId},
                        (p,e) => new {p,e})
                        
                         .Select(r => new DO_EmailHeader
                         {
                             EmailTempid = r.e.EmailTempId,
                             EmailType=r.e.EmailType,
                             EmailTempDesc=r.e.EmailTempDesc,
                             ActiveStatus = r.e.ActiveStatus
                         }).OrderBy(o => o.EmailTempid).ToListAsync();

                    return await ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EmailRecipient>> GetEmailRecipientByBusinessKeyAndEmailTempId(int businessKey, string emailTempId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcemars
                        .Where(w => w.BusinessKey == businessKey && w.EmailTempId == emailTempId)
                         .Select(r => new DO_EmailRecipient
                         {
                             EmailTempid = r.EmailTempId,
                             Emailid = r.EmailId,
                             RecipientName = r.RecipientName,
                             Remarks = r.Remarks,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.RecipientName).ToListAsync();

                    return await ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoEmailRecipient(DO_EmailRecipient obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_emailtempExist = db.GtEcemars.Any(a => a.BusinessKey == obj.BusinessKey && a.EmailTempId.Trim() == obj.EmailTempid.Trim() && a.EmailId == obj.Emailid);
                        if (is_emailtempExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0069", Message = string.Format(_localizer[name: "W0069"]) };
                        }

                        var em_sr = new GtEcemar
                        {
                            BusinessKey = obj.BusinessKey,
                            EmailTempId = obj.EmailTempid,
                            EmailId = obj.Emailid,
                            RecipientName = obj.RecipientName,
                            Remarks = obj.Remarks,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId1,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcemars.Add(em_sr);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateEmailRecipient(DO_EmailRecipient obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcemar email_sr = db.GtEcemars.Where(a => a.BusinessKey == obj.BusinessKey && a.EmailTempId.Trim() == obj.EmailTempid.Trim() && a.EmailId == obj.Emailid).FirstOrDefault();
                        if (email_sr == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0070", Message = string.Format(_localizer[name: "W0070"]) };
                        }

                        email_sr.RecipientName = obj.RecipientName;
                        email_sr.Remarks = obj.Remarks;
                        email_sr.ActiveStatus = obj.ActiveStatus;
                        email_sr.ModifiedBy = obj.UserID;
                        email_sr.ModifiedOn = DateTime.Now;
                        email_sr.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }


        #endregion SMS Recipient

        
    }
}
