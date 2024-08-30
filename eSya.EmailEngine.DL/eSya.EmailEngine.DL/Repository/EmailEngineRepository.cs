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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0113", Message = string.Format(_localizer[name: "W0113"]) };
                        }

                        bool is_EmailComponentExist = db.GtEcemavs.Any(a => a.Emacomponent.Trim().ToUpper() == obj.Emacomponent.Trim().ToUpper());
                        if (is_EmailComponentExist)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0114", Message = string.Format(_localizer[name: "W0114"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0114", Message = string.Format(_localizer[name: "W0114"]) };
                        }

                        GtEcemav em_sv = db.GtEcemavs.Where(w => w.Emavariable == obj.Emavariable).FirstOrDefault();
                        if (em_sv == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0115", Message = string.Format(_localizer[name: "W0115"]) };
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
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0115", Message = string.Format(_localizer[name: "W0115"]) };
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

    }
}
